using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContaSys.Models;
using ContaSys.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ContaSys.Intefaces;
using ContaSys.DTOs;
using System;

namespace ContaSys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsientosContablesController : ControllerBase
    {
        private readonly ContaSysContext _context;
        private readonly IAsientoContableService _asientoContableService;

        public AsientosContablesController(ContaSysContext context, IAsientoContableService asientoContableService)
        {
            _context = context;
            _asientoContableService = asientoContableService;
        }

        [HttpPost("InsertarAsientoContable")]
        [ProducesResponseType(typeof(APIResponse<AsientoContable>), StatusCodes.Status201Created)]
        public async Task<ActionResult<AsientoContable>> InsertarAsientoContable(EntradaAsientoContableDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new APIResponse<AsientoContable>
                {
                    success = false,
                    messageList = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var auxiliar = await _context.Auxiliares.FindAsync(request.IdAuxiliar);
            if (auxiliar == null)
            {
                return BadRequest(new APIResponse<AsientoContable>
                {
                    success = false,
                    messageList = new List<string> { $"El auxiliar con Id {request.IdAuxiliar} no existe." }
                });
            }
            else if (auxiliar.Estado == false) {
                return BadRequest(new APIResponse<AsientoContable>
                {
                    success = false,
                    messageList = new List<string> { $"El auxiliar con Id {request.IdAuxiliar} esta inactivo." }
                });
            }

            if (request.Cuentas == null || request.Cuentas.Count() < 2)
            {
                return BadRequest(new APIResponse<AsientoContable>
                {
                    success = false,
                    messageList = new List<string> { "El asiento contable debe tener al menos dos cuentas." }
                });
            }

            // Crear el asiento contable y asociar los detalles
            var nuevoAsientoContable = new AsientoContable
            {
                Fecha = DateTime.Now,
                Descripcion = request.Descripcion,
                Estado = "R", //Valor por defecto R
                AuxiliarId = request.IdAuxiliar,
                DetalleAsientoContables = request.Cuentas.Select(d => new DetalleAsientoContable
                {
                    CuentaContableId = d.IdCuentaContable,
                    Monto = d.Monto,
                    TipoMovimiento = d.TipoMovimiento
                }).ToList()
            };

            // Lógica para insertar el asiento contable en la base de datos
            var result = await _asientoContableService.InsertarAsientoContableAsync(nuevoAsientoContable);

            if (result.success)
            {
                return CreatedAtAction(nameof(InsertarAsientoContable), new { id = result.data.Id }, result.data);
            }
            else
            {
                return Conflict(result);
            }
        }
    }


}
