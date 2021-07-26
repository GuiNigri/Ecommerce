using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface IMarcaServices:IBaseServices<MarcaModel>
    {
        Task<MarcaModel> GetByNameAsync(string name);
        Task<IEnumerable<MarcaModel>> Filter(string termo);
    }
}
