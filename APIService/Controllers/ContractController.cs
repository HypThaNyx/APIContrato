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

        [HttpGet]
        [Route("")]
        public async Task<List<Contrato>> GetContracts()
        {
            var contracts = await _context.Contratos
                .Include(x => x.Prestacoes)
                .ToListAsync();
            // foreach (Contrato contract in contracts)
            // {
            //     contract.SetPrestacoes(await GetPrestacoesByContrato(contract.Id));
            //     _context.Contratos.Update(contract);
            // }
            return contracts;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<Contrato> GetContractById(int id)
        {
            var contrato = await _context.Contratos
                .Include(x => x.Prestacoes)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return contrato;
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
    }
}