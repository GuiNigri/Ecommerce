using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IKitsRepository:IBaseRepository<KitModel>
    {
        Task<IEnumerable<KitModel>> FilterAsync(string termo);
    }
}
