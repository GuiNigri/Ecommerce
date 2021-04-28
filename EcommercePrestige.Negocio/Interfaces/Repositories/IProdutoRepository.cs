using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IProdutoRepository : IBaseRepository<ProdutoModel>
    {
        Task<IEnumerable<ProdutoModel>> GetFilterAsync(IEnumerable<ProdutoModel> lista, FiltroModel filtroModel, string statusAtivacao);
        Task<IEnumerable<ProdutoModel>> OrderListAsync(IEnumerable<ProdutoModel> lista, string orderType);
        Task<IEnumerable<ProdutoModel>> GetListByCategoryAsync(string category, int marcaId,string statusAtivacao);
        Task<IEnumerable<ProdutoModel>> GetOfflineProducts(string statusAtivacao);
        Task<ProdutoModel> GetProductByIdAndCor(int idProd, int corId, string statusAtivacao);
        Task<int> CreateProdutoReturningIdAsync(ProdutoModel produtoModel);
        Task<IEnumerable<ProdutoModel>> GetAllAsync(string statusAtivacao);
        Task<bool> VerificarReferencia(string referencia);
        Task<IEnumerable<ProdutoModel>> FilterBarraPesquisar(string termo);
        Task<IEnumerable<ProdutoModel>> GetFilterByTermoAsync(string termo, string statusAtivacao);
        Task AtualizarEstoqueMassa(int quantidade);
    }
}
