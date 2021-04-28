using System.Collections.Generic;

namespace EcommercePrestige.Application.ViewModel
{
    public class ProdutoComponentViewModel
    {
        public ProdutoViewModel Produto { get; set; }

        public IEnumerable<ProdutoCorViewModel> CorProduto { get; set; }

        public bool Logado { get; set; }

        public bool ExibirPreco { get; set; }

        public ProdutoComponentViewModel(ProdutoViewModel produto, IEnumerable<ProdutoCorViewModel> corProduto,bool logado, bool exibirPreco)
        {
            Produto = produto;
            Logado = logado;
            CorProduto = corProduto;
            ExibirPreco = exibirPreco;
        }
    }
}
