using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;

namespace EcommercePrestige.Application.AppServices
{
    public interface IHomeAppServices
    {
        Task<IEnumerable<TextoHomeViewModel>> GetAllTextosAsync();
        Task CreateTextoAsync(TextoHomeViewModel textoHomeViewModel);
        Task DeleteTextoAsync(int id);

        Task<IEnumerable<BannersHomeViewModel>> GetAllBannersAsync();
        Task CreateBannerAsync(Stream banner);
        Task DeleteBannerAsync(int id);
    }
}
