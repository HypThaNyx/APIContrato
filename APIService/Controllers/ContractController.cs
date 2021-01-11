using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIService.Data;
using APIService.Models;
using APIService.Services;

namespace APIService.Controllers
{
    [ApiController]
    [Route("")]
    public class ContractController : ControllerBase
    {
        private DataContext _context;
        private IPrestacaoService _service;

        public ContractController(
            DataContext context,
            IPrestacaoService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        [Route("contrato")]
        public async Task<ActionResult<List<Contrato>>> GetContracts()
        {
            var contracts = await _context.Contratos.ToListAsync();
            return contracts;         
        }

        [HttpPost]
        [Route("contrato")]
        public async Task<ActionResult<Contrato>> PostContract(
            [FromBody] Contrato model)
        {
            if (ModelState.IsValid)
            {
                List<Prestacao> prestacoes = await _service.GerarPrestacoes(model);
                foreach (Prestacao prestacao in prestacoes)
                {
                    _context.Prestacoes.Add(prestacao);
                }
                model.Prestacoes = prestacoes;
                _context.Contratos.Add(model);
                await _context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);         
            }
        }
    }
}