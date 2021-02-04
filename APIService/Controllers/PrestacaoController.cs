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
        private IPrestacaoService _service;

        public PrestacaoController(
            IPrestacaoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{idContrato:int}")]
        public async Task<ActionResult<List<Prestacao>>> GetPrestacoesByContrato(int idContrato)
        {
            var prestacoes = await _service.ListarPrestacoesPorContrato(idContrato);
            
            if (prestacoes == null)
                return NotFound();

            return prestacoes;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Prestacao>>> GetPrestacoes()
        {
            var prestacoes = await _service.ListarTodasPrestacoes();

            if (prestacoes == null)
                return NotFound();

            return prestacoes;
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> PutPrestacao(int id, [FromBody] Prestacao prestacao)
        {
            if (prestacao == null)
                return BadRequest();
            
            var resultado = await _service.AlterarPrestacao(id, prestacao);

            return resultado ? Ok() : NotFound();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> DeletePrestacao(int id)
        {
            bool resultado = await _service.RemoverPrestacao(id);
            return resultado ? Ok() : NotFound();
        }
    }
}