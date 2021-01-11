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
            var contracts = await _context.Contratos
                .Include(x => x.Prestacoes)
                .ToListAsync();
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
            var contrato = await _context.Contratos
                .Include(x => x.Prestacoes)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return contrato;
        }

        [HttpGet]
        [Route("prestacao/{idContrato:int}")]
        public async Task<List<Prestacao>> GetPrestacoesByContrato(int idContrato)
        {
            var prestacoes = await _context.Prestacoes
                .AsNoTracking()
                .Where(x => x.IdContrato == idContrato)
                .ToListAsync();
            return prestacoes;
        }

        [HttpGet]
        [Route("prestacao")]
        public async Task<List<Prestacao>> GetPrestacoes()
        {
            var prestacoes = await _context.Prestacoes
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

                model.Prestacoes = prestacoes;
                _context.Contratos.Update(model);
                await _context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);         
            }
        }

        [NonAction]
        public async Task<string> CheckStatus(string dataPagamento, DateTime dataVencimento)
        {
            await Task.Delay(0);
            
            if (!string.IsNullOrEmpty(dataPagamento))
            {
                return "Baixada";
            }
            else if (dataVencimento.CompareTo(DateTime.Now) < 0)
            {
                return "Atrasada";
            }
            else return "Aberta";
        }
    }
}