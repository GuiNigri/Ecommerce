using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Infrastructure
{
    public interface ICieloCheckout
    {
        Task CriarTransacao(int numeroPedido, PedidoModel dadosDoPedido, IEnumerable<PedidoProdutosModel> produtosDoPedido,
            string identity, string fullName, string email, string phone);
    }
}
