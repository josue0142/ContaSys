using ContaSys.Models;

namespace ContaSys.DTOs
{
    public class CuentaAsientoContableDTO
    {
        public int IdCuentaContable { get; set; }
        public string Descripcion { get; set; }
        public string TipoMovimiento { get; set; }
        public decimal Monto { get; set; }
    }
}
