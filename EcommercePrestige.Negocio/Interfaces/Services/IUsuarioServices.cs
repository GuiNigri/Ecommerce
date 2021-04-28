using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface IUsuarioServices:IBaseServices<UsuarioModel>
    {
        Task<UsuarioModel> GetByUserIdAsync(string userId);
        Task<IEnumerable<UsuarioModel>> Filter(string termo, bool pendente, bool bloqueado);
        Task<IEnumerable<UsuarioModel>> GetBloqueadosAsync();
        Task<IEnumerable<UsuarioModel>> GetPendentesAsync();
        Task SetarStatusCadastroAsync(string status, string userId, string email);
        Task CreateAsync(UsuarioModel usuarioModel, string email, string callBackUrl);
    }
}
