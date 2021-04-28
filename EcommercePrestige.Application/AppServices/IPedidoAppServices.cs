using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;
using Rotativa.AspNetCore;

namespace EcommercePrestige.Application.AppServices
{
    public interface IPedidoAppServices
    {
        Task<(bool, string, string, int)> FinalizarPedido(CarrinhoPagamentoViewModel carrinhoPagamentoViewModel,
            IEnumerable<CarrinhoViewModel> carrinho, string userId, string transactionData, CorreioWebServiceViewModel dadosEnvio);
        Task<IEnumerable<PedidoViewModel>> GetByUsuario(int id);
        Task<IEnumerable<ProdutosPedidoViewModel>> GetProdutosByPedido(int pedido);
        Task<PedidoDetailsViewModel> GetPedido(int pedido);
        Task<bool> CheckPedidoUsuario(int usuarioId, int pedido);
        Task<IEnumerable<PedidoViewModel>> GetAllAsync();
        Task<IEnumerable<PedidoAdmViewModel>> FilterAsync(string termo, int status);
        Task<IEnumerable<PedidoAdmViewModel>> GetAllAdmAsync();
        Task<PedidoAdmViewModel> GetPedidoAdm(int pedido);
        Task UpdateAsync(PedidoAdmViewModel pedidoAdmViewModel, string rastreio, string newTipoEnvio);
        Task AprovarPedidoAsync(int id, string nomeUsuario, string email);
        Task ReprovarPedidoAsync(int id, string nomeUsuario, string email);
        Task CancelarPedidoAsync(int id, string nomeUsuario, string email);
        Task<IEnumerable<PedidoAdmViewModel>> GetApprovedAdmAsync();
        Task<IEnumerable<PedidoAdmViewModel>> GetReprovedAdmAsync();
        Task<IEnumerable<PedidoAdmViewModel>> GetWaitingAdmAsync();
        Task AlterarProdutosPedidoAsync(UsuarioViewModel usuarioViewModel, string email, int id, int newQuantidade);
        Task AdicionarItemAoPedidoAsync(int pedido, string idProduto, int cores, int quantidade);
        Task RemoverItemDoPedido(string tipoOp, int id);
        Task<bool> CheckIfExistOrderByUserAsync(string userId);
        Task VerificarSePedidoEntregue();
        Task<ViewAsPdf> GerarPedidoPdf(PedidoDetailsViewModel pedido, string email);
        Task EnviarEmailAlteracaoPedido(PedidoDetailsViewModel pedido, string email);
        Task EnviarEmailPedidoDespachado(string email, int pedido, string rastreio, string nomeUsuario, byte[] anexo);
        Task EnviarConfirmacaoPedidoEmail(int idPedido, string nomeCompleto, string userEmail);
        Task ConcluirPedido(int pedido, string userEmail);
    }
}
