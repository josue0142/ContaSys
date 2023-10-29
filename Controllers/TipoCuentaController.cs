using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContaSys.Models;
using ContaSys.Utilities;

namespace ContaSys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoCuentaController : ControllerBase
    {
        private readonly ContaSysContext _context;

        public TipoCuentaController(ContaSysContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<List<TipoCuenta>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TipoCuenta>>> GetTiposCuenta()
        {
            if (_context.TipoCuenta == null)
            {
                return NotFound();
            }

            return Ok(new APIResponse<List<TipoCuenta>>
            {
                data = await _context.TipoCuenta.ToListAsync()
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<TipoCuenta>), StatusCodes.Status200OK)]
        public async Task<ActionResult<TipoCuenta>> GetTipoCuenta(int id)
        {
            if (_context.TipoCuenta == null)
            {
                return NotFound();
            }
            var tipoCuenta = await _context.TipoCuenta.FindAsync(id);

            if (tipoCuenta == null)
            {
                return NotFound();
            }

            return Ok(new APIResponse<TipoCuenta>
            {
                data = tipoCuenta
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIResponse<TipoCuenta>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PutTipoCuenta(int id, TipoCuenta tipoCuenta)
        {
            if (id != tipoCuenta.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipoCuenta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoCuentaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<TipoCuenta>
            {
                data = tipoCuenta
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<TipoCuenta>), StatusCodes.Status201Created)]
        public async Task<ActionResult<TipoCuenta>> PostTipoCuenta(TipoCuenta tipoCuenta)
        {
            if (_context.TipoCuenta == null)
            {
                return BadRequest(new APIResponse<TipoCuenta>
                {
                    success = false,
                    messageList = new List<string> { "Entity set 'ContaSysContext.TipoCuenta' is null." }
                });
            }

            _context.TipoCuenta.Add(tipoCuenta);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(new APIResponse<TipoCuenta>
                {
                    success = false,
                    messageList = new List<string> { "Error al insertar el tipo de cuenta en la base de datos." }
                });
            }

            return CreatedAtAction(nameof(GetTipoCuenta), new { id = tipoCuenta.Id }, tipoCuenta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoCuenta(int id)
        {
            if (_context.TipoCuenta == null)
            {
                return NotFound();
            }
            var tipoCuenta = await _context.TipoCuenta.FindAsync(id);
            if (tipoCuenta == null)
            {
                return NotFound();
            }

            _context.TipoCuenta.Remove(tipoCuenta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoCuentaExists(int id)
        {
            return (_context.TipoCuenta?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
