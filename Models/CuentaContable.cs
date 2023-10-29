using System;
using System.Collections.Generic;

namespace ContaSys.Models
{
    public partial class CuentaContable
    {
        public CuentaContable()
        {
            InverseCuentaMayor = new HashSet<CuentaContable>();
        }

        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public string? PermiteMov { get; set; }
        public int? TipoId { get; set; }
        public int? Nivel { get; set; }
        public decimal? Balance { get; set; }
        public int? CuentaMayorId { get; set; }
        public bool? Estado { get; set; }

        public virtual CuentaContable? CuentaMayor { get; set; }
        public virtual TipoCuenta? Tipo { get; set; }
        public virtual ICollection<CuentaContable> InverseCuentaMayor { get; set; }
    }
}
