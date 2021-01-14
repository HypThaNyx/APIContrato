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
        private IFeatureManager _featureManager;
        private IPrestacaoService _service;

        public ContractController(
            IFeatureManager featureManager,
            IPrestacaoService service)
        {
            _featureManager = featureManager;
            _service = service;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Contrato>> PostContract([FromBody] Contrato model)
        {
            if (ModelState.IsValid)
            {                
                return await _service.GerarNovoContrato(model);
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
            var contratos = await _service.ListContratos();

            if (contratos == null)
                return NotFound();

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
                    
                    return await _service.GetContractById(id);
                });
                return cacheEntry;
                
            } else return await _service.GetContractById(id);
        }        

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> FinalizarContrato(int id)
        {
            bool resultado = await _service.RemoverContrato(id);
            return resultado ? Ok() : NotFound();
        }
    }
}