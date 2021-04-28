using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IProdutoFotoRepository:IBaseRepository<ProdutoFotoModel>
    {
        Task<IEnumerable<ProdutoFotoModel>> GetByProdutoAsync(int id);
        Task<ProdutoFotoModel> GetPrincipalByProdutoAsync(int id);
        Task<ProdutoFotoModel> GetFotosToKitAsync(int id);
        Task<IEnumerable<ProdutoFotoModel>> RetornarListaFotoInput(int idProd);
        Task<bool> CheckFotoAndPrincipal(int idProd);
        Task<IEnumerable<ProdutoFotoModel>> GetFotosbyCorAsync(int idCor);
        Task<ProdutoFotoModel> GetFotosbyCorIndexAsync(int idCor);
    }
}
