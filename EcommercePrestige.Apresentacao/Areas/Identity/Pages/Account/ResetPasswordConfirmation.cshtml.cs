using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommercePrestige.Apresentacao.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : PageModel
    {
        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet()
        {
            StatusMessage = "Senha alterada com sucesso";
            return Page();
        }
    }
}
