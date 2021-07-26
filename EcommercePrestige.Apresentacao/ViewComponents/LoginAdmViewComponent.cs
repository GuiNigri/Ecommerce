using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePrestige.Apresentacao.ViewComponents
{
    public class LoginAdmViewComponent : ViewComponent
    {
        private readonly IUsuarioAppServices _usuarioAppServices;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginAdmViewComponent(IUsuarioAppServices usuarioAppServices, UserManager<IdentityUser> userManager)
        {
            _usuarioAppServices = usuarioAppServices;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(new ClaimsPrincipal(User));
            var usuario = await _usuarioAppServices.GetByUserIdAsync(user.Id);

            var loginViewModel = new LoginViewModel();

            var verificacaoNome = usuario.NomeCompleto.Contains(" ");

            if (verificacaoNome)
            {
                try
                {
                    loginViewModel.NomeUsuario = usuario.NomeCompleto.Substring(0, usuario.NomeCompleto.IndexOf(" ", StringComparison.Ordinal));
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else
            {
                loginViewModel.NomeUsuario = usuario.NomeCompleto;
            }


            return View(loginViewModel);
        }
    }
}
