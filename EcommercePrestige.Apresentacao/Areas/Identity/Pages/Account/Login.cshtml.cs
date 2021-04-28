using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace EcommercePrestige.Apresentacao.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUsuarioAppServices _usuarioAppServices;
        private readonly ISuporteAppServices _suporteAppServices;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager, IUsuarioAppServices usuarioAppServices, ISuporteAppServices suporteAppServices)
        {
            _userManager = userManager;
            _usuarioAppServices = usuarioAppServices;
            _suporteAppServices = suporteAppServices;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "E-mail obrigatório")]
            [Display(Name = "Email")]
            [EmailAddress]

            public string Email { get; set; }

            [Required(ErrorMessage = "Senha obrigatória")]
            [Display(Name = "Senha")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Mantenha-me conectado")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("/Index");
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var mensagem = "";
                
                var identityUser = await _userManager.FindByEmailAsync(Input.Email);

                

                if (identityUser != null)
                {
                    var usuario = await _usuarioAppServices.GetByUserIdAsync(identityUser.Id);

                    var result = SignInResult.Success;

                    if (usuario.BloqueioAutomatico)
                    {
                        if (usuario.Verificado)
                        {
                            result = SignInResult.LockedOut;
                            mensagem = "Usuário bloqueado. Entre em contato com nosso suporte para mais informações";
                        }
                        else
                        {
                            result = SignInResult.LockedOut;
                            mensagem = "Usuário bloqueado. Seu cadastro está em análise";
                        }


                    }
                    else if (usuario.BloqueioManual)
                    {
                        result = SignInResult.LockedOut;
                        mensagem = "Usuário bloqueado. Entre em contato com nosso suporte para mais informações";
                    }

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            return LocalRedirect(returnUrl);
                        }
                        else if(result.IsNotAllowed)
                        {
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                            var callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new { area = "Identity", userId = identityUser.Id, code = code },
                                protocol: Request.Scheme);

                            var htmlMessage =
                                $"Olá, {usuario.NomeCompleto}!<br>" +
                                $"Para finalizar seu cadastro em nosso site, favor confirmar seu e-mail clicando <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>neste link</a>.<br><br><br>" +
                                $"Equipe de suporte ao cliente <br>" +
                                $"Prestige do Brasil";

                            await _suporteAppServices.SendAutomaticSuporteEmail(htmlMessage, Input.Email, "Confirmação de conta");

                            ModelState.AddModelError(string.Empty, "Confirme seu e-mail para acessar o sistema. Reenviamos o link de confirmação para o e-mail cadastrado");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Usuário ou senha incorretos");
                        }
                        
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {

                        ModelState.AddModelError(string.Empty, mensagem);
                        return Page();
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario ou senha incorretos.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
