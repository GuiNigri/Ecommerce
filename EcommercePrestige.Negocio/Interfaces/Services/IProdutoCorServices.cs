using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface IProdutoCorServices:IBaseServices<ProdutoCorModel>
    {
        Task<IEnumerable<ProdutoCorModel>> GetByProdutoAsync(int id, string statusAtivacao);
        Task<IEnumerable<ProdutoCorModel>> GetAllGroupedAsync(string statusAtivacao);
        Task<(IEnumerable<ProdutoCorModel>, bool)> GetListByKitAsync(string kitName, string statusAtivacao);
        Task<ProdutoCorModel> GetCorByProdutoAsync(int idCor, int prodId, string statusAtivacao);
        Task<bool> VerificarEstoque(int prodId, int idCor, int quantidade, string statusAtivacao);
        Task<(bool, string)> AddListaCor(ProdutoCorModel produtoCorModel);
        Task<IEnumerable<ProdutoCorModel>> RetornarListaDeCorDoProduto(int id, string statusAtivacao);
        Task<bool> VerificarSeCorExiste(int idProd, int idCor);
        Task UpdateAsync(int id);
        Task AlterarAtivacaoKitNoProduto(int idCor, string kit);
        Task AlterarEstoqueAsync(int id, int quantidade);
        Task<ProdutoCorModel> ObterPeloCodigoBarrasAsync(string codigoBarras);
        Task AlterarCodigoBarrasAsync(int id, string codigoBarras);
    }
}
