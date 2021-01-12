using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIService.Data;
using APIService.Models;
using APIService.Services;
using System.Linq;
using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.FeatureManagement;

namespace APIService.Controllers
{
    [ApiController]
    [Route("contrato")]
    public class ContractController : ControllerBase
    {
        private DataContext _context;
        private IPrestacaoService _service;
        private IFeatureManager _featureManager;

        public ContractController(
            DataContext context)
        {
            _context = context;
            _service = new PrestacaoService();
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Contrato>> PostContract([FromBody] Contrato model)
        {
            if (ModelState.IsValid)
            {
                model.SetDataContratacao(DateTime.Now);
                _context.Contratos.Add(model);
                await _context.SaveChangesAsync();

                List<Prestacao> prestacoes = await _service.GerarPrestacoes(model);
                foreach (Prestacao prestacao in prestacoes)
                {
                    _context.Prestacoes.Add(prestacao);
                }

                model.SetPrestacoes(prestacoes);
                _context.Contratos.Update(model);
                await _context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);         
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Contrato>>> GetContracts(
            [FromServices] IMemoryCache _cache)
        {
            var contratos = await _context.Contratos
                .Include(x => x.Prestacoes)
                .ToListAsync();

            if (contratos == null)
                return NotFound();

            // foreach (Contrato contrato in contratos)
            // {
            //     contrato.SetPrestacoes(await GetPrestacoesByContrato(contrato.Id));
            //     _context.Contratos.Update(contrato);
            // }

            return contratos;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Contrato>> GetContractByIdWithCache(
            int id,
            [FromServices] IMemoryCache _cache)
        {
            TimeSpan cacheExpirationTime = await _service.TempoAteAmanha();

            if (await _featureManager.IsEnabledAsync("CacheFeatureFlag"))
            {
                var cacheEntry = await _cache.GetOrCreateAsync(id, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = cacheExpirationTime;
                    entry.SetPriority(CacheItemPriority.High);
                    
                    return await GetContractById(id);
                });
                return cacheEntry;
                
            } else return await GetContractById(id);
        }

        [NonAction]
        public async Task<Contrato> GetContractById(
            int id)
        {            
            var contrato = await _context.Contratos
                .Include(x => x.Prestacoes)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return contrato;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> FinalizarContrato(int id)
        {
            var contrato = await _context.Contratos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (contrato == null)
                return NotFound();

            var prestacoes = await _context.Prestacoes
                .AsNoTracking()
                .Where(x => x.IdContrato == contrato.Id)
                .ToListAsync();

            _context.RemoveRange(prestacoes);
            
            _context.Contratos.Remove(contrato);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}