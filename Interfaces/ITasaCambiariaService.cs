using ContaSys.DTOs;
using ContaSys.Models;
using ContaSys.Utilities;

namespace ContaSys.Intefaces
{
    public interface ITasaCambiariaService
    {
        Task<List<TasaCambiariaDTO>> ObtenerTasasCambiarias();
    }
}
