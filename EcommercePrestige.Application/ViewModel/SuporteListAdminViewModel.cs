using X.PagedList;

namespace EcommercePrestige.Application.ViewModel
{
    public class SuporteListAdminViewModel:BaseViewModel
    {
        public IPagedList<SuporteViewModel> SuporteList { get; set; }
        public string Termo { get; set; }

        public SuporteListAdminViewModel()
        {
        }

        public SuporteListAdminViewModel(IPagedList<SuporteViewModel> suporteList, string termo)
        {
            SuporteList = suporteList;
            Termo = termo;
        }
    }
}
