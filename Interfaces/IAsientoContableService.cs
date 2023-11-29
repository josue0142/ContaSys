using ContaSys.Models;
using ContaSys.Utilities;

namespace ContaSys.Intefaces
{
    public interface IAsientoContableService
    {
        Task<APIResponse<AsientoContable>> InsertarAsientoContableAsync(AsientoContable asientoContable);
    }
}
