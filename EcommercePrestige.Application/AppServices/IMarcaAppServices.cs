using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;

namespace EcommercePrestige.Application.AppServices
{
    public interface IMarcaAppServices
    {
        Task<IEnumerable<MarcaViewModel>> GetAllAsync();
        Task<MarcaViewModel> GetByIdAsync(int id);
        Task CreateAsync(MarcaViewModel marcaViewModel);
        Task UpdateAsync(MarcaViewModel marcaViewModel);
        Task<IEnumerable<MarcaViewModel>> Filter(string termo);
    }
}
