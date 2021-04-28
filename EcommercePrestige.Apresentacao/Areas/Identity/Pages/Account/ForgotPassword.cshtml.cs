using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace EcommercePrestige.Apresentacao.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ISuporteAppServices _suporteAppServices;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, ISuporteAppServices suporteAppServices)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _suporteAppServices = suporteAppServices;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);


                var htmlMessage =
                    $"Olá,<br>" +
                    $"Recebemos uma solicitação de alteração de senha da sua conta. Para alterar sua senha <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clique neste link</a>.<br><br>" +
                    $"Caso não tenha solicitado a alteração de senha, desconsidere esse e-mail.<br><br><br>" +
                    $"Equipe de suporte ao cliente <br>" +
                    $"Prestige do Brasil";

                await _suporteAppServices.SendAutomaticSuporteEmail(htmlMessage, Input.Email,"Solicitação de recuperação de senha");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
