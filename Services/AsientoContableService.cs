using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContaSys.Models;
using ContaSys.Utilities;
using ContaSys.Intefaces;
using ContaSys.Controllers;
using System.Runtime.ConstrainedExecution;
using Force.DeepCloner;

namespace ContaSys.Services
{
    public class AsientoContableService : IAsientoContableService
    {
        private readonly ContaSysContext _context;

        public AsientoContableService(ContaSysContext context)
        {
            _context = context;
        }

        public async Task<APIResponse<AsientoContable>> InsertarAsientoContableAsync(AsientoContable asientoContable)
        {

            if (asientoContable == null)
            {
                return new APIResponse<AsientoContable>
                {
                    success = false,
                    messageList = new System.Collections.Generic.List<string> { "El asiento contable es nulo." }
                };
            }

            // Validar que todos los detalles tengan "CR" o "DB"
            if (!ValidarTiposMovimiento(asientoContable))
            {
                return new APIResponse<AsientoContable>
                {
                    success = false,
                    messageList = new List<string> { "Las cuentas del Asiento Contable deben tener el tipo de movimiento 'CR' o 'DB'." }
                };
            }

            // Validar que la cantidad acumulada de montos para "CR" y "DB" sea la misma
            if (!ValidarDoblePartida(asientoContable))
            {
                return new APIResponse<AsientoContable>
                {
                    success = false,
                    messageList = new List<string> { "La cantidad acumulada de montos para 'CR' y 'DB' no coincide." }
                };
            }


            try
            {

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        List<DetalleAsientoContable> detalleAsientoContables = (List<DetalleAsientoContable>)asientoContable.DetalleAsientoContables;
                        asientoContable.DetalleAsientoContables = new List<DetalleAsientoContable>();//Inicializa para evitar error de EntityFramework Tracked Id
                        _context.AsientoContables.Add(asientoContable);
                        _context.SaveChanges();

                        foreach (var detalle in detalleAsientoContables)
                        {
                            detalle.AsientoContableId = asientoContable.Id;
                            _context.DetalleAsientoContables.Add(detalle);
                        }
                        _context.SaveChanges();


                        //Modificar cuentas destino del Asiento Contable
                        foreach (var detalle in detalleAsientoContables)
                        {
                            var cuentaContable = _context.CuentaContables.Find(detalle.CuentaContableId);
                            if (cuentaContable != null)
                            {
                                var tipoCuenta = _context.TipoCuenta.Find(cuentaContable.TipoId);
                                if (tipoCuenta != null)
                                {
                                    //Se aumenta o disminuye balance segun el tipo movimiento y su cuenta origen (lado normal o donde aumenta la cuenta de balance)
                                    cuentaContable.Balance += (detalle.TipoMovimiento == tipoCuenta.Origen ? detalle.Monto : -detalle.Monto);
                                }
                            }
                            else
                            {
                                return new APIResponse<AsientoContable>
                                {
                                    success = false,
                                    messageList = new List<string> { $"La cuenta contable con el Id {detalle.CuentaContableId} no existe." }
                                };
                            }
                        }

                        _context.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        return new APIResponse<AsientoContable>
                        {
                            success = false,
                            messageList = new List<string> { "Error al insertar el asiento contable y sus detalles." }
                        };
                    }
                }


                return new APIResponse<AsientoContable>
                {
                    success = true,
                    data = asientoContable
                };
            }
            catch (Exception ex)
            {
                // Manejar errores específicos si es necesario
                return new APIResponse<AsientoContable>
                {
                    success = false,
                    messageList = new System.Collections.Generic.List<string> { $"Error al insertar el asiento contable: {ex.Message}" }
                };
            }
        }

        private bool ValidarTiposMovimiento(AsientoContable asientoContable)
        {
            return asientoContable.DetalleAsientoContables.All(detalle =>
                detalle.TipoMovimiento == "CR" || detalle.TipoMovimiento == "DB");
        }

        private bool ValidarDoblePartida(AsientoContable asientoContable)
        {
            decimal sumaCR = 0;
            decimal sumaDB = 0;

            foreach (var detalle in asientoContable.DetalleAsientoContables)
            {
                if (detalle.TipoMovimiento == "CR")
                {
                    sumaCR += detalle.Monto;
                }
                else if (detalle.TipoMovimiento == "DB")
                {
                    sumaDB += detalle.Monto;
                }
            }

            return sumaCR == sumaDB;
        }
    }
}
