using EcommercePrestige.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommercePrestige.Model.Interfaces.Infrastructure
{
    public interface IPagarMeCheckout
    {
        Task<PagarMeReturnModel> CreateTransaction(PagarMeModel pagarMeModel, PedidoModel orderData, IEnumerable<PedidoProdutosModel> orderProducts, string userId);
    }
}
