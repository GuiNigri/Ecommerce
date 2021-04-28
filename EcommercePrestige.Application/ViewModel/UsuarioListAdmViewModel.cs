using X.PagedList;

namespace EcommercePrestige.Application.ViewModel
{
    public class UsuarioListAdmViewModel:BaseViewModel
    {
        public IPagedList<UsuarioViewModel> ListaUsuarios { get; set; }
        public string Termo { get; set; }

        public UsuarioListAdmViewModel(IPagedList<UsuarioViewModel> listaUsuarios, string termo)
        {
            ListaUsuarios = listaUsuarios;
            Termo = termo;
        }
    }
}
