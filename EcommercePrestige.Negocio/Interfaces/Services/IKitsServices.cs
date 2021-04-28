using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface IKitsServices:IBaseServices<KitModel>
    {
        Task<IEnumerable<KitModel>> FilterAsync(string termo);
    }
}
