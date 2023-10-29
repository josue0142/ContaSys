using System;
using System.Collections.Generic;

namespace ContaSys.Models
{
    public partial class TipoCuenta
    {
        public TipoCuenta()
        {
            CuentaContables = new HashSet<CuentaContable>();
        }

        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public string? Origen { get; set; }
        public bool? Estado { get; set; }

        public virtual ICollection<CuentaContable> CuentaContables { get; set; }
    }
}
