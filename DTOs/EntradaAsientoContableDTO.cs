namespace ContaSys.DTOs
{
    public class EntradaAsientoContableDTO
    {
        public string Descripcion { get; set; }
        public int IdAuxiliar { get; set; }
        public int? IdMonedaWS { get; set; }
        public virtual ICollection<CuentaAsientoContableDTO> Cuentas { get; set; }
    }
}
