using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContaSys.Models;
using ContaSys.Utilities;

namespace ContaSys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuxiliaresController : ControllerBase
    {
        private readonly ContaSysContext _context;

        public AuxiliaresController(ContaSysContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<List<Auxiliares>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Auxiliares>>> GetAuxiliares()
        {
            if (_context.Auxiliares == null)
            {
                return NotFound();
            }

            return Ok(new APIResponse<List<Auxiliares>>
            {
                data = await _context.Auxiliares.ToListAsync()
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<Auxiliares>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Auxiliares>> GetAuxiliares(int id)
        {
            if (_context.Auxiliares == null)
            {
                return NotFound();
            }
            var auxiliares = await _context.Auxiliares.FindAsync(id);

            if (auxiliares == null)
            {
                return NotFound();
            }

            return Ok(new APIResponse<Auxiliares>
            {
                data = auxiliares
            });
        }

        // PUT: api/Auxiliares/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(APIResponse<Auxiliares>), StatusCodes.Status200OK)]
        public async Task<IActionResult> PutAuxiliares(int id, Auxiliares auxiliares)
        {
            if (id != auxiliares.Id)
            {
                return BadRequest();
            }

            _context.Entry(auxiliares).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuxiliaresExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new APIResponse<Auxiliares>
            {
                data = auxiliares
            });
        }

        // POST: api/Auxiliares
        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<Auxiliares>), StatusCodes.Status201Created)]
        public async Task<ActionResult<Auxiliares>> PostAuxiliares(Auxiliares auxiliares)
        {
            if (_context.Auxiliares == null)
            {
                return BadRequest(new APIResponse<Auxiliares>
                {
                    success = false,
                    messageList = new List<string> { "Entity set 'ContaSysContext.Auxiliares' is null." }
                });
            }

            _context.Auxiliares.Add(auxiliares);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(new APIResponse<Auxiliares>
                {
                    success = false,
                    messageList = new List<string> { "Error al insertar el auxiliar en la base de datos." }
                });
            }

            return CreatedAtAction(nameof(GetAuxiliares), new { id = auxiliares.Id }, auxiliares);
        }

        // DELETE: api/Auxiliares/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuxiliares(int id)
        {
            if (_context.Auxiliares == null)
            {
                return NotFound();
            }
            var auxiliares = await _context.Auxiliares.FindAsync(id);
            if (auxiliares == null)
            {
                return NotFound();
            }

            _context.Auxiliares.Remove(auxiliares);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuxiliaresExists(int id)
        {
            return (_context.Auxiliares?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
