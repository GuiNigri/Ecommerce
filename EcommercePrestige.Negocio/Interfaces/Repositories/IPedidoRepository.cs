using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IPedidoRepository:IBaseRepository<PedidoModel>
    {
        Task<int> CreateReturningIdAsync(PedidoModel pedidoModel);
        Task CreateProdutosAsync(PedidoProdutosModel pedidoProdutosModel);
        Task CreateKitsAsync(PedidoKitModel pedidoKitModel);
        Task<IEnumerable<PedidoModel>> GetByUsuario(int id);
        Task<IEnumerable<PedidoProdutosModel>> GetProdutosByPedido(int pedido);
        Task<IEnumerable<PedidoKitModel>> GetKitsByPedido(int pedido);
        Task<PedidoModel> GetPedido(int pedido);
        Task<bool> CheckPedidoUsuario(int usuarioId, int pedido);
        Task<bool> CheckTrackingCode(string trackingCode, int pedido);
        Task<IEnumerable<PedidoModel>> FilterAsync(string termo, int status);
        Task<IEnumerable<PedidoModel>> GetApprovedAsync();
        Task<IEnumerable<PedidoModel>> GetReprovedAsync();
        Task<IEnumerable<PedidoModel>> GetWaitingAsync();
        Task<PedidoProdutosModel> GetProdutosById(int id);
        Task UpdateProdutosPedidoAsync(PedidoProdutosModel pedidoProdutosModel);
        Task<PedidoKitModel> GetKitsById(int id);
        Task UpdateKitsPedidoAsync(PedidoKitModel pedidoKitModel);
        Task DeleteProdutosAsync(PedidoProdutosModel pedidoProdutosModel);
        Task DeleteKitsAsync(PedidoKitModel pedidoKitModel);
        Task<bool> CheckIfExistOrderByUserAsync(int idUsuario);
        Task<IEnumerable<PedidoModel>> GetPedidosDespachados();
    }
}
