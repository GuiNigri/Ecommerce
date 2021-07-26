using System.IO;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface IBannersHomeServices:IBaseServices<BannersHomeModel>
    {
        Task CreateAsync(Stream banner);
    }
}
