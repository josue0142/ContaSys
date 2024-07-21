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
        private readonly ITasaCambiariaService _tasaCambiariaService;

        public AsientosContablesController(ContaSysContext context, 
            IAsientoContableService asientoContableService,
            ITasaCambiariaService tasaCambiariaService
            )
        {
            _context = context;
            _asientoContableService = asientoContableService;
            _tasaCambiariaService = tasaCambiariaService;
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

            //Convertir monto por tasa de WS
            if(request.IdMonedaWS != 0 || request.IdMonedaWS != null)
            {
                var tasas = _tasaCambiariaService.ObtenerTasasCambiarias().Result;
                if (tasas.Any()) 
                {
                    var tasa = tasas.FirstOrDefault(field=>field.Id ==  request.IdMonedaWS);

                    if (tasa != null) {

                        foreach (var item in request.Cuentas)
                        {
                            item.Monto = item.Monto * tasa.Tasa;
                        }

                    }
                }
            }

            // Crear el asiento contable y asociar los detalles
            var nuevoAsientoContable = new AsientoContable
            {
                Fecha = DateTime.Now,
                Descripcion = request.Descripcion,
                Estado = "R", //Valor por defecto R
                AuxiliarId = request.IdAuxiliar,
                IdMonedaWS = request.IdMonedaWS,
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

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<List<AsientoContable>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AsientoContable>>> GetAsientosContables(
            [FromQuery] DateTime? fechaDesde = null,
            [FromQuery] DateTime? fechaHasta = null,
            [FromQuery] int? idAuxiliar = null,
            [FromQuery] int? idAsiento = null)
        {
            IQueryable<AsientoContable> query = _context.AsientoContables.Include(a => a.DetalleAsientoContables);

            if (fechaDesde.HasValue)
            {
                query = query.Where(a => a.Fecha.Date >= fechaDesde.Value.Date);
            }

            if (fechaHasta.HasValue)
            {
                query = query.Where(a => a.Fecha.Date <= fechaHasta.Value.Date);
            }

            if (idAuxiliar.HasValue)
            {
                query = query.Where(a => a.AuxiliarId == idAuxiliar.Value);
            }

            if (idAsiento.HasValue)
            {
                query = query.Where(a => a.Id == idAsiento.Value);
            }

            var asientosContables = await query.ToListAsync();

            return Ok(new APIResponse<List<AsientoContable>>
            {
                data = asientosContables
            });
        }


    }


}
