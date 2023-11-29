using System;
using System.Collections.Generic;

namespace ContaSys.Models
{
    public partial class DetalleAsientoContable
    {
        public int Id { get; set; }
        public int CuentaContableId { get; set; }
        public int AsientoContableId { get; set; }
        public decimal Monto { get; set; }
        public string TipoMovimiento { get; set; } = null!;

        public virtual AsientoContable AsientoContable { get; set; } = null!;
    }
}
