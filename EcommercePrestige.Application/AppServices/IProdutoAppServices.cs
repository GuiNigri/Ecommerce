using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;

namespace EcommercePrestige.Application.AppServices
{
    public interface IProdutoAppServices
    {
        Task<(int, bool, string)> CreateProdutoAsync(ProdutoCreateEtapaBasicaModel produtoCreateEtapaBasicaModel);
        Task<(bool, string)> UpdateAsync(ProdutoCreateEtapaBasicaModel produtoCreateEtapaBasicaModel, string novaReferencia);
        Task<IEnumerable<ProdutoViewModel>> FilterAndOrderAsync(FiltroProdutoViewModel filtroViewModel, string statusAtivacao);
        Task<IEnumerable<ProdutoViewModel>> GetCategoryAsync(FiltroProdutoViewModel filtroViewModel, string statusAtivacao);
        Task<IEnumerable<ProdutoViewModel>> GetAllAsync(string statusAtivacao);
        Task<ProdutoViewModel> GetByIdAsync(int id);
        Task<ProdutoViewModel> GetProductByIdAndCor(int idProd, int corId, string statusAtivacao);
        Task AlterarStatusAtivacaoProduto(int idProduto);
        Task<IEnumerable<ProdutoViewModel>> FilterBarraPesquisar(string termo);
        Task<IEnumerable<ProdutoViewModel>> GetCategoryForHomeAsync(string category, int marcaId);
        Task<IEnumerable<ProdutoViewModel>> GetFilterTermoAsync(FiltroProdutoViewModel filtroViewModel,
            string statusAtivacao);
        Task CadastrarAviseMeAsync(int corId, string email);
        Task<IEnumerable<AviseMeViewModel>> GetAviseMe();
        Task AtualizarEstoqueMassa(int quantidade);
    }
}
