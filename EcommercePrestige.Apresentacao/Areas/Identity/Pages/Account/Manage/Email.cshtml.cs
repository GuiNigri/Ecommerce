using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace EcommercePrestige.Apresentacao.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISuporteAppServices _suporteAppServices;

        public EmailModel(
            UserManager<IdentityUser> userManager,
            ISuporteAppServices suporteAppServices)
        {
            _userManager = userManager;
            _suporteAppServices = suporteAppServices;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Novo email")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Não foi possível carregar os dados do usuário");
            }

            await LoadAsync(user).ConfigureAwait(false);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Não foi possível carregar os dados do usuário");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user).ConfigureAwait(false);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = Input.NewEmail, code = code },
                    protocol: Request.Scheme);

                var htmlMessage =
                    $"Olá,<br>" +
                    $"Recebemos uma solicitação de alteração de e-mail. Para confirmar seu novo e-mail <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clique neste link</a>.<br><br>" +
                    $"Equipe de suporte ao cliente <br>" +
                    $"Prestige do Brasil";

                await _suporteAppServices.SendAutomaticSuporteEmail(htmlMessage, Input.NewEmail, "Alteração de email");

                StatusMessage = "Link de confirmação encaminhado para o e-mail informado";
                return RedirectToPage();
            }

            StatusMessage = "Não houve alteração no e-mail.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user).ConfigureAwait(false);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);

            var htmlMessage =
                $"Olá,<br>" +
                $"Recebemos uma solicitação de alteração de e-mail, para confirmar seu novo e-mail <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clique neste link</a>.<br><br>" +
                $"Equipe de suporte ao cliente <br>" +
                $"Prestige do Brasil";

            await _suporteAppServices.SendAutomaticSuporteEmail(htmlMessage, Input.NewEmail, "Alteração de email");

            StatusMessage = "Link de confirmação encaminhado para o e-mail informado";
            return RedirectToPage();
        }
    }
}
