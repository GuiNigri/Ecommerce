using System.Collections.Generic;

namespace EcommercePrestige.Application.ViewModel
{
    public class ProdutoDetailsViewModel:BaseViewModel
    {
        public ProdutoViewModel Produto { get; set; }

        private IEnumerable<ProdutoFotoViewModel> imgProduto;

        public IEnumerable<ProdutoFotoViewModel> ImgProduto
        {
            get { return imgProduto; }
            set { imgProduto = value; }
        }

        private IEnumerable<ProdutoCorViewModel> corProduto;

        public IEnumerable<ProdutoCorViewModel> CorProduto
        {
            get { return corProduto; }
            set { corProduto = value; }
        }

        public bool Logado { get; set; }

        public int CorSelecionada { get; set; }

        public IEnumerable<ProdutoViewModel> ProdutosCarrossel { get; set; }

        public string ReturnUrl { get; set; }

        public ProdutoDetailsViewModel(ProdutoViewModel produtoViewModel, IEnumerable<ProdutoFotoViewModel> imgProduto, IEnumerable<ProdutoCorViewModel> corProduto, string statusModel, bool logado, IEnumerable<ProdutoViewModel> produtosCarrossel, int corSelecionada, string returnUrl)
        {
            Produto = produtoViewModel;
            ImgProduto = imgProduto;
            CorProduto = corProduto;
            StatusModel = statusModel;
            Logado = logado;
            ProdutosCarrossel = produtosCarrossel;
            CorSelecionada = corSelecionada;
            ReturnUrl = returnUrl;
        }
    }

}
