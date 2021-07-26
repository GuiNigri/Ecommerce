using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface IProdutoFotoServices:IBaseServices<ProdutoFotoModel>
    {
        Task<IEnumerable<ProdutoFotoModel>> GetByProdutoAsync(int id);
        Task<ProdutoFotoModel> GetPrincipalByProdutoAsync(int id);
        Task<ProdutoFotoModel> GetFotosToKitAsync(int id);
        Task<(bool, string)> AddFotoList(Stream stream, int idCor, ProdutoFotoModel produtoFotoModel, string statusAtivacao);
        Task<IEnumerable<ProdutoFotoModel>> RetornarListaFotoInput(int idProd);
        Task RemoveFotoList(int id);
        Task CreateAsync(int idProduto, string statusAtivacao);
        Task<bool> CheckFotoAndPrincipal(int idProd);
        Task UpdateAsync(int id);
        Task<IEnumerable<ProdutoFotoModel>> GetFotosbyCorAsync(int id);
        Task<ProdutoFotoModel> GetFotosbyCorIndexAsync(int id);
    }
}
