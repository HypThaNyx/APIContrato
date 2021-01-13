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
    [Route("prestacao")]
    public class PrestacaoController : ControllerBase
    {
        private DataContext _context;
        private IPrestacaoService _service;

        public PrestacaoController(
            DataContext context,
            IPrestacaoService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        [Route("{idContrato:int}")]
        public async Task<ActionResult<List<Prestacao>>> GetPrestacoesByContrato(int idContrato)
        {
            var prestacoes = await _context.Prestacoes
                .AsNoTracking()
                .Where(x => x.IdContrato == idContrato)
                .ToListAsync();
            
            if (prestacoes == null)
                return NotFound();

            return prestacoes;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Prestacao>>> GetPrestacoes()
        {
            var prestacoes = await _context.Prestacoes
                .ToListAsync();

            if (prestacoes == null)
                return NotFound();

            return prestacoes;
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> AtualizarPrestacao(int id, [FromBody] Prestacao prestacao)
        {
            if (prestacao == null)
                return BadRequest();
            
            var _prestacao = await _context.Prestacoes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (_prestacao == null)
                return NotFound();
            
            _prestacao.DataVencimento = prestacao.DataVencimento;
            _prestacao.DataPagamento = prestacao.DataPagamento;
            _prestacao.Status = await _service.CheckStatusPrestacao(_prestacao.DataPagamento, _prestacao.DataVencimento);

            _context.Prestacoes.Update(_prestacao);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> FinalizarPrestacao(int id)
        {
            var _prestacao = await _context.Prestacoes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (_prestacao == null)
                return NotFound();
            
            _context.Prestacoes.Remove(_prestacao);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}