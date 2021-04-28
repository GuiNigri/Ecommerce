using System.Collections.Generic;

namespace EcommercePrestige.Application.ViewModel
{
    public class CarrinhoViewModel:BaseViewModel
    {
        public ProdutoViewModel Produto { get; set; }
        public KitsViewModel Kits { get; set; }
        public CorViewModel Cor { get; set; }
        public int CorId { get; set; }
        public int QuantidadeIndividual { get; set; }
        public string ValorUnitarioTotal { get; set; }
        public string SubTotalPedido { get; set; }
        public double DescontoUnitarioProduto { get; set; }
        public string DescontoPedido { get; set; }
        public string ValorTotalPedido { get; set; }
        public int QuantidadeTotalItens { get; set; }
        public IEnumerable<CarrinhoViewModel> CarrinhoViewModels { get; set; }
    }

    public class ProdutosPedidoViewModel:BaseViewModel
    {
        public ProdutoViewModel Produto { get; set; }
        public KitsViewModel Kits { get; set; }
        public string Cor { get; set; }
        public string CI { get; set; }
        public string DescricaoCor { get; set; }
        public int Quantidade { get; set; }
        public string ValorUnitario { get; set; }
        public string ValorTotal { get; set; }
    }
}
