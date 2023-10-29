using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContaSys.Models;
using ContaSys.Utilities;

namespace ContaSys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonedaController : ControllerBase
    {
        private readonly ContaSysContext _context;

        public MonedaController(ContaSysContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<List<Moneda>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Moneda>>> GetMonedas()
        {
            if (_context.Moneda == null)
            {
                return NotFound();
            }

            return Ok(new APIResponse<List<Moneda>>
            {
                data = await _context.Moneda.ToListAsync()
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<Moneda>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Moneda>> GetMoneda(int id)
        {
            if (_context.Moneda == null)
            {
                return NotFound();
            }
            var moneda = await _context.Moneda.FindAsync(id);

            if (moneda == null)
            {
                return NotFound();
            }

            return Ok(new APIResponse<Moneda>
            {
                data = moneda
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIResponse<Moneda>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PutMoneda(int id, Moneda moneda)
        {
            if (id != moneda.Id)
            {
                return BadRequest();
            }

            _context.Entry(moneda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonedaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<Moneda>
            {
                data = moneda
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<Moneda>), StatusCodes.Status201Created)]
        public async Task<ActionResult<Moneda>> PostMoneda(Moneda moneda)
        {
            if (_context.Moneda == null)
            {
                return BadRequest(new APIResponse<Moneda>
                {
                    success = false,
                    messageList = new List<string> { "Entity set 'ContaSysContext.Moneda' is null." }
                });
            }

            _context.Moneda.Add(moneda);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(new APIResponse<Moneda>
                {
                    success = false,
                    messageList = new List<string> { "Error al insertar la moneda en la base de datos." }
                });
            }

            return CreatedAtAction(nameof(GetMoneda), new { id = moneda.Id }, moneda);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoneda(int id)
        {
            if (_context.Moneda == null)
            {
                return NotFound();
            }
            var moneda = await _context.Moneda.FindAsync(id);
            if (moneda == null)
            {
                return NotFound();
            }

            _context.Moneda.Remove(moneda);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MonedaExists(int id)
        {
            return (_context.Moneda?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
