using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tp_ProgramacionIII.Models;
using Microsoft.EntityFrameworkCore;

namespace Tp_ProgramacionIII.Controllers
{
    [ApiController]
    [Route("Clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Get()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return Ok(clientes);
        }
        [HttpPost]
        public async Task<ActionResult<Cliente>> AddTransaction(Cliente nuevoCliente)
        { 
            if (string.IsNullOrEmpty(nuevoCliente.Name) || string.IsNullOrEmpty(nuevoCliente.Email))
            {
                return BadRequest("El nombre y el correo electrónico son obligatorios.");
            }
            _context.Clientes.Add(nuevoCliente);
            await _context.SaveChangesAsync();
            return Ok(nuevoCliente);
        }

    }
}