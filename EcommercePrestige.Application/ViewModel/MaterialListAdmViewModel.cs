using X.PagedList;

namespace EcommercePrestige.Application.ViewModel
{
    public class MaterialListAdmViewModel:BaseViewModel
    {
        public IPagedList<MaterialViewModel> ListaMaterial { get; set; }
        public string Termo { get; set; }

        public MaterialListAdmViewModel(IPagedList<MaterialViewModel> listaMaterial,string termo)
        {
            ListaMaterial = listaMaterial;
            Termo = termo;
        }
    }
}
