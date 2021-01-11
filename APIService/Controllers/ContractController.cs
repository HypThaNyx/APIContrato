using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIService.Data;
using APIService.Models;

namespace APIService.Controllers
{
    [ApiController]
    [Route("contrato")]
    public class ContractController : ControllerBase
    {
        private DataContext _context;

        public ContractController(
            DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Contrato>>> GetContracts()
        {
            var contracts = await _context.Contratos.ToListAsync();
            return contracts;         
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Contrato>> PostContract(
            [FromBody] Contrato model)
        {
            if (ModelState.IsValid)
            {
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