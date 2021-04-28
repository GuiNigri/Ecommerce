using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IUsuarioRepository:IBaseRepository<UsuarioModel>
    {
        Task<UsuarioModel> GetByUserIdAsync(string userId);
        Task<IEnumerable<UsuarioModel>> Filter(string termo, bool pendente, bool bloqueado);
        Task<IEnumerable<UsuarioModel>> GetBloqueadosAsync();
        Task<IEnumerable<UsuarioModel>> GetPendentesAsync();
    }
}
