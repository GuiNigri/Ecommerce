using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePrestige.Apresentacao.ViewComponents
{
    public class ProdutoViewComponent:ViewComponent
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IProdutoCorAppServices _produtoCorAppServices;

        public ProdutoViewComponent(SignInManager<IdentityUser> signInManager, IProdutoCorAppServices produtoCorAppServices)
        {
            _signInManager = signInManager;
            _produtoCorAppServices = produtoCorAppServices;
        }
        public async Task<IViewComponentResult> InvokeAsync(ProdutoViewModel produtoViewModel, bool exibirPreco)
        {
            var logado = _signInManager.IsSignedIn(new ClaimsPrincipal(User));

            var produtoCorViewModel = await _produtoCorAppServices.GetByProdutoAsync(produtoViewModel.Id, "AT");

            var quantidadeCores = produtoCorViewModel.Count();

            var quantidadeCoresIndisponiveis = produtoCorViewModel.Count(item => item.Estoque == 0);

            if (quantidadeCores == quantidadeCoresIndisponiveis)
            {
                produtoViewModel.StatusProduto = "indisponivel";
            }

            return View(new ProdutoComponentViewModel(produtoViewModel,produtoCorViewModel,logado, exibirPreco));
        }
    }
}
