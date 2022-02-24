using System.Collections.Generic;

namespace Ecommerce.Api.Controllers.Pedido.Dto
{
    public class RegistrarPedidoRequest
    {
        public int Empresa { get; set; }
        public IEnumerable<ProdutoDto> Produtos { get; set; }
        public PagamentoDto Pagamento { get; set; }
        public string Observacoes { get; set; }
    }
    
    public class ProdutoDto
    {
        public int Produto { get; set; }
        public int Quantidade { get; set; }
    }

    public class PagamentoDto
    {
        public string FormaPagamento { get; set; }
        public string Parcelas { get; set; }
        public double Frete { get; set; }
        public double Desconto { get; set; }
        public double Subtotal { get; set; }
    }
}
