using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePrestige.Apresentacao.ViewComponents
{
    public class LoginViewComponent:ViewComponent
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUsuarioAppServices _usuarioAppServices;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginViewComponent(UserManager<IdentityUser> userManager,IUsuarioAppServices usuarioAppServices, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _usuarioAppServices = usuarioAppServices;
            _signInManager = signInManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(new ClaimsPrincipal(User));

            var loginViewModel = new LoginViewModel();

            if (user != null)
            {
                //await _userManager.AddClaimAsync(user, new Claim("AdminClaim", string.Empty));
                loginViewModel.Admin = false;
                var usuariosAdm = await _userManager.GetUsersForClaimAsync(new Claim("AdminClaim", string.Empty));
                //await _signInManager.RefreshSignInAsync(user);
                var verificacao = usuariosAdm.Any(x => x.Id == user.Id);

                if (verificacao)
                {
                    loginViewModel.Admin = true;
                }

                var usuario = await _usuarioAppServices.GetByUserIdAsync(user.Id);

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
                
            }

            return View(loginViewModel);
        }
    }
}
