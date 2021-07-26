using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IMaterialRepository:IBaseRepository<MaterialModel>
    {
        Task<MaterialModel> GetByNameAsync(string name);
        Task<IEnumerable<MaterialModel>> FilterAsync(string termo);
    }
}
