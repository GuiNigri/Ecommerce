using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;

namespace EcommercePrestige.Application.AppServices
{
    public interface IMaterialAppServices
    {
        Task<IEnumerable<MaterialViewModel>> GetAllAsync();
        Task<MaterialViewModel> GetByIdAsync(int id);
        Task CreateAsync(MaterialViewModel materialViewModel);
        Task UpdateAsync(MaterialViewModel materialViewModel);
        Task<IEnumerable<MaterialViewModel>> FilterAsync(string termo);
    }
}
