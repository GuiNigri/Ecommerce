using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
namespace EcommercePrestige.Apresentacao.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly ISuporteAppServices _suporteAppServices;


        public ChangePasswordModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<ChangePasswordModel> logger, ISuporteAppServices suporteAppServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _suporteAppServices = suporteAppServices;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Senha atual")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "A senha tem que ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nova senha")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirme a nova senha")]
            [Compare("NewPassword", ErrorMessage = "As senhas não conferem")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar os dados do usuário");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Não foi possível carregar os dados do usuario.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            var htmlMessage =
                $"Olá,<br>" +
                $"Sua senha acaba de ser alterada.<br>" +
                $"Caso esta alteração não tenha sido realizada por você, entre em contato com o suporte ou redefina sua senha em nosso site.<br><br>" +
                $"Equipe de suporte ao cliente <br>" +
                $"Prestige do Brasil";

            await _suporteAppServices.SendAutomaticSuporteEmail(htmlMessage, user.Email, "Alteração de senha");

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Senha alterada com sucesso";

            return RedirectToPage();
        }
    }
}
