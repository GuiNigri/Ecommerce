using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace EcommercePrestige.Apresentacao.Areas.Identity.Pages.Account.Manage
{
    public class PedidosModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUsuarioAppServices _usuarioAppServices;
        private readonly IPedidoAppServices _pedidoAppServices;

        public PedidosModel(UserManager<IdentityUser> userManager, IUsuarioAppServices usuarioAppServices, IPedidoAppServices pedidoAppServices)
        {
            _userManager = userManager;
            _usuarioAppServices = usuarioAppServices;
            _pedidoAppServices = pedidoAppServices;
        }

        [BindProperty]
        public string StatusMessage { get; set; }

        public IPagedList<PedidoViewModel> listaPedidosUsuario;


        private async Task LoadAsync(int pagina)
        {
            var user = await _userManager.GetUserAsync(User);
            var usuario = await _usuarioAppServices.GetByUserIdAsync(user.Id);

            var lista = await _pedidoAppServices.GetByUsuario(usuario.Id);

            var listaPedidos = await lista.ToList().ToPagedListAsync(pagina, 10);

            listaPedidosUsuario = listaPedidos;
        }

        public async Task OnGet(int pagina = 1)
        {
            if (TempData["ErroPedido"] != null)
            {
                StatusMessage = TempData["ErroPedido"].ToString();
            }

            await LoadAsync(pagina);
        }
    }
}
