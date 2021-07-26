using X.PagedList;

namespace EcommercePrestige.Application.ViewModel
{
    public class EmpresaListAdminViewModel:BaseViewModel
    {
        public IPagedList<EmpresaViewModel> ListaEmpresa { get; set; }
        public string Termo { get; set; }

        public EmpresaListAdminViewModel(IPagedList<EmpresaViewModel> listaEmpresa, string termo)
        {
            ListaEmpresa = listaEmpresa;
            Termo = termo;
        }
    }
}
