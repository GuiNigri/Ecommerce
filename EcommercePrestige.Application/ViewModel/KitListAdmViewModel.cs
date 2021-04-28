using X.PagedList;

namespace EcommercePrestige.Application.ViewModel
{
    public class KitListAdmViewModel:BaseViewModel
    {
        public IPagedList<KitsViewModel> Kit { get; set; }
        public string Termo { get; set; }

        public KitListAdmViewModel(IPagedList<KitsViewModel> kit, string termo)
        {
            Kit = kit;
            Termo = termo;
        }
    }
}
