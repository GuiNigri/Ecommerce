using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;

namespace EcommercePrestige.Application.AppServices
{
    public interface IProdutoCorAppServices
    {
        Task<IEnumerable<ProdutoCorViewModel>> GetByProdutoAsync(int id, string statusAtivacao);
        Task<IEnumerable<ProdutoCorViewModel>> GetAllGroupedAsync(string statusAtivacao);
        Task<bool> VerificarEstoque(int prodId, int idCor, int quantidade);
        Task<(bool, string)> AddListaCor(ProdutoCorInputModel produtoCorInputModel);
        Task RemoveListaCor(int id);
        Task<IEnumerable<ProdutoCorInputModel>> RetornarListaDeCorDoProduto(int id, string statusAtivacao);
        Task UpdateAsync(int id);
        Task AlterarAtivacaoKitNoProduto(int idCor, string kit);
        Task AlterarEstoqueAsync(int id, int quantidade);
        Task AlterarCodigoBarrasAsync(int id, string codigoBarras);
    }
}
