using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface IPedidoService:IBaseServices<PedidoModel>
    {
        Task<int> CreateReturningIdAsync(PedidoModel pedidoModel);
        Task CreateProdutosAsync(IEnumerable<PedidoProdutosModel> pedidoProdutosModel,
            IEnumerable<PedidoKitModel> pedidoKitModel);

        Task<IEnumerable<PedidoModel>> GetByUsuario(int id);
        Task<IEnumerable<PedidoProdutosModel>> GetProdutosByPedido(int pedido);
        Task<IEnumerable<PedidoKitModel>> GetKitsByPedido(int pedido);
        Task<PedidoModel> GetPedido(int pedido);
        Task<bool> CheckPedidoUsuario(int usuarioId, int pedido);
        Task<IEnumerable<PedidoModel>> FilterAsync(string termo, int status);
        Task UpdateAsync(PedidoModel pedidoModel, string rastreio, string tipoEnvio);
        Task AprovarPedidoAsync(int id, string nomeUsuario, string email);
        Task ReprovarPedidoAsync(int id, string nomeUsuario, string email);
        Task CancelarPedidoAsync(int id, string nomeUsuario, string email);
        Task EnviarConfirmacaoPedidoEmail(int pedido, string nomeUsuario, string emailUsuario);
        Task<IEnumerable<PedidoModel>> GetApprovedAsync();
        Task<IEnumerable<PedidoModel>> GetReprovedAsync();
        Task<IEnumerable<PedidoModel>> GetWaitingAsync();
        Task AlterarProdutoPedidoAsync(UsuarioModel usuarioModel, string email, int id, int newQuantidade);
        Task AdicionarItemAoPedidoAsync(int pedido, string idProduto, int cores, int quantidade);
        Task RemoverItemDoPedido(string tipoOp, int id);
        Task<bool> CheckIfExistOrderByUserAsync(string userId);
        Task VerificarSePedidoEntregueAsync();
        Task EnviarEmailAlteracaoPedido(string nome, int numeroPedido, string email);
        Task EnviarEmailPedidoDespachado(string email, int pedido, string rastreio, string nomeUsuario, byte[] anexo);
        Task ConcluirPedido(int pedido, string userEmail);
        Task CriarTransactionCieloAsync(int numeroPedido, IEnumerable<PedidoProdutosModel> produtosDoPedido, PedidoModel dadosDoPedido,
            string userId);
        Task<PagarMeReturnModel> CreatePagarMeTransaction(PagarMeModel pagarMeModel, PedidoModel orderData, IEnumerable<PedidoProdutosModel> orderProducts, string userId);
    }
}
