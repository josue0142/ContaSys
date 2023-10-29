using System;
using System.Collections.Generic;

namespace ContaSys.Models
{
    public partial class Moneda
    {
        public int Id { get; set; }
        public string? CodigoIso { get; set; }
        public string? Descripcion { get; set; }
        public decimal? TasaCambio { get; set; }
        public bool? Estado { get; set; }
    }
}
