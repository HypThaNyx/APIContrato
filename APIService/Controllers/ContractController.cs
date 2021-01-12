using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIService.Data;
using APIService.Models;
using APIService.Services;
using System.Linq;
using System;

namespace APIService.Controllers
{
    [ApiController]
    [Route("contrato")]
    public class ContractController : ControllerBase
    {
        private DataContext _context;
        private IPrestacaoService _service;

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
        public async Task<ActionResult<List<Contrato>>> GetContracts()
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
        public async Task<ActionResult<Contrato>> GetContractById(int id)
        {
            var contrato = await _context.Contratos
                .Include(x => x.Prestacoes)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (contrato == null)
                return NotFound();

            return contrato;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> FinalizarContrato(int id)
        {
            var contrato = await _context.Contratos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (contrato == null)
                return NotFound();
            
            _context.Contratos.Remove(contrato);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}