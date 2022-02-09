using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IProdutoCorRepository:IBaseRepository<ProdutoCorModel>
    {
        Task<IEnumerable<ProdutoCorModel>> GetByProdutoAsync(int id, string statusAtivacao);
        Task<IEnumerable<ProdutoCorModel>> GetListByKitAsync(string kit, string statusAtivacao);
        Task<IEnumerable<ProdutoCorModel>> GetAllGroupedAsync(string statusAtivacao);
        Task<bool> VerificarEstoqueNegativo(IEnumerable<ProdutoCorModel> listCorModels);
        Task<ProdutoCorModel> GetCorByProdutoAsync(int corId, int prodId, string statusAtivacao);
        Task<bool> VerificarEstoque(int prodId, int corId, int quantidade, string statusAtivacao);
        Task BaixaEstoque (int id, int quantidade);
        Task<bool> VerificarSeCorExiste(int idProd,int idCor);
        Task<ProdutoCorModel> ObterPeloCodigoBarrasAsync(string codigoBarras);
    }
}
