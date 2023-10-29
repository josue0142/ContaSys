using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContaSys.Models;
using ContaSys.Utilities;

namespace ContaSys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaContableController : ControllerBase
    {
        private readonly ContaSysContext _context;

        public CuentaContableController(ContaSysContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<List<CuentaContable>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CuentaContable>>> GetCuentasContables()
        {
            if (_context.CuentaContables == null)
            {
                return NotFound();
            }

            return Ok(new APIResponse<List<CuentaContable>>
            {
                data = await _context.CuentaContables.ToListAsync()
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<CuentaContable>), StatusCodes.Status200OK)]
        public async Task<ActionResult<CuentaContable>> GetCuentaContable(int id)
        {
            if (_context.CuentaContables == null)
            {
                return NotFound();
            }
            var cuentaContable = await _context.CuentaContables.FindAsync(id);

            if (cuentaContable == null)
            {
                return NotFound();
            }

            return Ok(new APIResponse<CuentaContable>
            {
                data = cuentaContable
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIResponse<CuentaContable>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PutCuentaContable(int id, CuentaContable cuentaContable)
        {
            if (id != cuentaContable.Id)
            {
                return BadRequest();
            }

            _context.Entry(cuentaContable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuentaContableExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<CuentaContable>
            {
                data = cuentaContable
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<CuentaContable>), StatusCodes.Status201Created)]
        public async Task<ActionResult<CuentaContable>> PostCuentaContable(CuentaContable cuentaContable)
        {
            if (_context.CuentaContables == null)
            {
                return BadRequest(new APIResponse<CuentaContable>
                {
                    success = false,
                    messageList = new List<string> { "Entity set 'ContaSysContext.CuentaContable' is null." }
                });
            }

            _context.CuentaContables.Add(cuentaContable);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(new APIResponse<CuentaContable>
                {
                    success = false,
                    messageList = new List<string> { "Error al insertar la cuenta contable en la base de datos." }
                });
            }

            return CreatedAtAction(nameof(GetCuentaContable), new { id = cuentaContable.Id }, cuentaContable);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuentaContable(int id)
        {
            if (_context.CuentaContables == null)
            {
                return NotFound();
            }
            var cuentaContable = await _context.CuentaContables.FindAsync(id);
            if (cuentaContable == null)
            {
                return NotFound();
            }

            _context.CuentaContables.Remove(cuentaContable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CuentaContableExists(int id)
        {
            return (_context.CuentaContables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
