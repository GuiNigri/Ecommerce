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
    public class PedidoDetailsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUsuarioAppServices _usuarioAppServices;
        private readonly IPedidoAppServices _pedidoAppServices;

        public PedidoDetailsModel(UserManager<IdentityUser> userManager, IUsuarioAppServices usuarioAppServices, IPedidoAppServices pedidoAppServices)
        {
            _userManager = userManager;
            _usuarioAppServices = usuarioAppServices;
            _pedidoAppServices = pedidoAppServices;
        }

        [BindProperty]
        public PedidoDetailsViewModel PedidoViewModel { get; set; }
        public IPagedList<ProdutosPedidoViewModel> ProdutosPedidoViewModel { get; set; }
        public string StatusMessage { get; set; }


        private async Task<bool> LoadAsync(int numeroPedido, int idUsuario, int pagina)
        {
            var validacao = await _pedidoAppServices.CheckPedidoUsuario(idUsuario, numeroPedido);

            if (!validacao) return false;

            PedidoViewModel = await _pedidoAppServices.GetPedido(numeroPedido);

            if (PedidoViewModel == null)
            {
                return false;
            }

            var produtos = await _pedidoAppServices.GetProdutosByPedido(numeroPedido);

            ProdutosPedidoViewModel = await produtos.ToList().ToPagedListAsync(pagina, 3);

            return ProdutosPedidoViewModel.Any();
        }

        public async Task<IActionResult> OnGet(int pedido, int pagina = 1)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Não foi possível carregar os dados do usuário");
            }

            var usuario = await _usuarioAppServices.GetByUserIdAsync(user.Id);

            var validacao = await LoadAsync(pedido, usuario.Id, pagina);

            if(validacao)
            {
                return Page();
            }

            TempData["ErroPedido"] = "Erro ao carregar os dados do pedido";
            return RedirectToPage("./Pedidos");
        }
    }
}
