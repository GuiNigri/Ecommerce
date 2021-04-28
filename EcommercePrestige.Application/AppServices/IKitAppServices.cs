using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;

namespace EcommercePrestige.Application.AppServices
{
    public interface IKitAppServices
    {
        Task<KitsViewModel> GetByIdAsync(int id);
        Task<KitsViewModel> GetKitsAsync(int id, string statusAtivacao);
        Task<IEnumerable<KitsViewModel>> GetAllAsync();
        Task<IEnumerable<KitsViewModel>> FilterAsync(string termo);
        Task UpdateAsync(KitsViewModel kitsViewModel);
    }
}
