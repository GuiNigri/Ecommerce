using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IMarcaRepository:IBaseRepository<MarcaModel>
    {
        Task<MarcaModel> GetByNameAsync(string name);
        Task<IEnumerable<MarcaModel>> Filter(string termo);
        Task<bool> VerificarMarca(string marca);
    }
}
