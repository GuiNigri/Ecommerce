using X.PagedList;

namespace EcommercePrestige.Application.ViewModel
{
    public class ProdutoListAdmViewModel:BaseViewModel
    {
        public IPagedList<ProdutoViewModel> ListaProdutos { get; set; }
        public string Termo { get; set; }

        public ProdutoListAdmViewModel(IPagedList<ProdutoViewModel> listaProdutos, string termo)
        {
            ListaProdutos = listaProdutos;
            Termo = termo;
        }
    }
}
