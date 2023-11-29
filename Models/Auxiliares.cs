using System;
using System.Collections.Generic;

namespace ContaSys.Models
{
    public partial class Auxiliares
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public bool? Estado { get; set; }

        //public virtual ICollection<AsientoContable> AsientoContables { get; set; }
    }
}
