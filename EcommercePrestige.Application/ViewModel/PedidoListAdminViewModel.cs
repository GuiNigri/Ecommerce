using X.PagedList;

namespace EcommercePrestige.Application.ViewModel
{
    public class PedidoListAdminViewModel:BaseViewModel
    {
        public IPagedList<PedidoAdmViewModel> ListPedidos { get; set; }
        public string Termo { get; set; }

        public PedidoListAdminViewModel(IPagedList<PedidoAdmViewModel> listPedidos, string termo)
        {
            ListPedidos = listPedidos;
            Termo = termo;
        }
    }
}
