using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;

namespace EcommercePrestige.Application.AppServices
{
    public interface IUsuarioAppServices
    {
        Task<UsuarioViewModel> GetByUserIdAsync(string userId);
        Task UpdateAsync(UsuarioViewModel usuarioViewModel);
        Task<IEnumerable<UsuarioViewModel>> GetAllAsync();
        Task<UsuarioViewModel> GetByIdAsync(int id);
        Task<IEnumerable<UsuarioViewModel>> Filter(string termo, bool pendente, bool bloqueado);
        Task<IEnumerable<UsuarioViewModel>> GetBloqueadosAsync();
        Task<IEnumerable<UsuarioViewModel>> GetPendentesAsync();
        Task SetarStatusCadastroAsync(string status, string userId, string email);
        Task CreateAsync(UsuarioViewModel usuarioViewModel, string email,string callBackUrl);
    }
}
