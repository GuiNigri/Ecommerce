using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface IProdutoServices : IBaseServices<ProdutoModel>
    {

        Task<IEnumerable<ProdutoModel>> FilterAndOrderListAsync(IEnumerable<ProdutoModel> listaProdutos,
            FiltroModel filtroModel, string statusAtivacao);

        Task<IEnumerable<ProdutoModel>> GetListByCategoryAsync(FiltroModel filtroModel, string statusAtivacao);
        Task<ProdutoModel> GetProductByIdAndCor(int idProd, int corId, string statusAtivacao);
        Task<(int, bool, string)> CreateProdutoReturningIdAsync(ProdutoModel produtoModel);
        Task<IEnumerable<ProdutoModel>> GetAllAsync(string statusAtivacao);
        Task<(bool, string)> UpdateProductAsync(ProdutoModel produtoModel, string novaReferencia);
        Task AlterarStatusAtivacaoAsync(int id);
        Task<IEnumerable<ProdutoModel>> FilterBarraPesquisar(string termo);
        Task<IEnumerable<ProdutoModel>> GetCategoryForHome(string category, int marcaId);
        Task<IEnumerable<ProdutoModel>> GetListByTermoAsync(FiltroModel filtroModel, string statusAtivacao);
        Task AtualizarEstoqueMassa(int quantidade);

    }
}
