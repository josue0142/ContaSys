using System;
using System.Collections.Generic;

namespace ContaSys.Models
{
    public partial class AsientoContable
    {
        public AsientoContable()
        {
            DetalleAsientoContables = new HashSet<DetalleAsientoContable>();
        }

        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public int? AuxiliarId { get; set; }
        public int? IdMonedaWS { get; set; }

        public virtual ICollection<DetalleAsientoContable> DetalleAsientoContables { get; set; }
    }
}
