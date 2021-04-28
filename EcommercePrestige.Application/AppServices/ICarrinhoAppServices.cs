using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;

namespace EcommercePrestige.Application.AppServices
{
    public interface ICarrinhoAppServices
    {
        Task<string> AdicionarItemAoCarrinho(ProdutoViewModel produtoModel, KitsViewModel kitsModel, CorViewModel corViewModel, int quantidade, int corId);
        Task RemoverItemDoCarrinho(int idProd, int corId);
        Task AtualizarCarrinho(ProdutoViewModel produtoModel, KitsViewModel kitsModel, int corId, int quantidade);
        Task<CarrinhoPagamentoViewModel> GetAddress(string cep);
        Task<IEnumerable<string>> GetParcelas(string formaPagamento, double valor);
        Task<IEnumerable<CorreioWebServiceViewModel>> GetFrete(string cepDestino, int quantidadePecas, double valorTotalPedido);
    }
}
