using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommercePrestige.Apresentacao.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUsuarioAppServices _usuarioAppServices;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IUsuarioAppServices usuarioAppServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usuarioAppServices = usuarioAppServices;
        }

        [Display(Name = "Email")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Celular")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Nome Completo")]
            public string NomeCompleto { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var usuario = await _usuarioAppServices.GetByUserIdAsync(user.Id);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                NomeCompleto = usuario.NomeCompleto
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user).ConfigureAwait(false);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Erro inesperado ao tentar alterar número de celular.";
                    return RedirectToPage();
                }
            }

            var usuario = await _usuarioAppServices.GetByUserIdAsync(user.Id);
            if (Input.NomeCompleto != usuario.NomeCompleto)
            {
                try
                {
                    usuario.NomeCompleto = Input.NomeCompleto;
                    await _usuarioAppServices.UpdateAsync(usuario);
                }
                catch (Exception)
                {
                    StatusMessage = "Erro inesperado ao atualizar o nome do usuário. Caso persista o erro, contate o suporte";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Informações atualizadas com sucesso";
            return RedirectToPage();
        }
    }
}
