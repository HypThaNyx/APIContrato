using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIService.Data;
using APIService.Models;
using APIService.Services;
using System.Linq;

namespace APIService.Controllers
{
    [ApiController]
    [Route("")]
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

        [HttpGet]
        [Route("contrato")]
        public async Task<List<Contrato>> GetContracts()
        {
            var contracts = await _context.Contratos.ToListAsync();
            foreach (Contrato contract in contracts)
            {
                contract.Prestacoes = await GetPrestacoesByContrato(contract.Id);
                _context.Contratos.Update(contract);
            }
            return contracts;
        }

        [HttpGet]
        [Route("contrato/{id:int}")]
        public async Task<Contrato> GetContractById(int id)
        {
            var contrato = await _context.Contratos.Include(x => x.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return contrato;
        }

        [HttpGet]
        [Route("prestacao/{idContrato:int}")]
        public async Task<List<Prestacao>> GetPrestacoesByContrato(int idContrato)
        {
            var prestacoes = await _context.Prestacoes
                .Include(x => x.Contrato)
                .AsNoTracking()
                .Where(x => x.IdContrato == idContrato)
                .ToListAsync();
            return prestacoes;
        }

        [HttpPost]
        [Route("contrato")]
        public async Task<ActionResult<Contrato>> PostContract([FromBody] Contrato model)
        {
            if (ModelState.IsValid)
            {
                _context.Contratos.Add(model);
                await _context.SaveChangesAsync();

                List<Prestacao> prestacoes = await _service.GerarPrestacoes(model);
                foreach (Prestacao prestacao in prestacoes)
                {
                    _context.Prestacoes.Add(prestacao);
                }

                Contrato contrato = await GetContractById(model.Id);
                contrato.Prestacoes = prestacoes;
                _context.Contratos.Update(contrato);
                await _context.SaveChangesAsync();
                return contrato;
            }
            else
            {
                return BadRequest(ModelState);         
            }
        }
    }
}