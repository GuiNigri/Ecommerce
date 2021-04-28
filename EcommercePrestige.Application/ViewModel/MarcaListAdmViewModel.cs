using X.PagedList;

namespace EcommercePrestige.Application.ViewModel
{
    public class MarcaListAdmViewModel:BaseViewModel
    {
        public IPagedList<MarcaViewModel> ListaMarca { get; set; }
        public string Termo { get; set; }

        public MarcaListAdmViewModel(IPagedList<MarcaViewModel> listaMarca, string termo)
        {
            ListaMarca = listaMarca;
            Termo = termo;
        }
    }
}
