using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using X.PagedList;

namespace EcommercePrestige.Apresentacao.Controllers
{
    [Authorize(policy: "Admin")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioAppServices _usuarioAppServices;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmpresaAppServices _empresaAppServices;
        private readonly ISuporteAppServices _suporteAppServices;

        public UsuarioController(IUsuarioAppServices usuarioAppServices, UserManager<IdentityUser> userManager, IEmpresaAppServices empresaAppServices, ISuporteAppServices suporteAppServices)
        {
            _usuarioAppServices = usuarioAppServices;
            _userManager = userManager;
            _empresaAppServices = empresaAppServices;
            _suporteAppServices = suporteAppServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string termo,int pagina = 1)
        {
            IEnumerable<UsuarioViewModel> listaUsuarios;

            if (termo == null)
            {
                listaUsuarios = await _usuarioAppServices.GetAllAsync();
            }
            else
            {
                listaUsuarios = await _usuarioAppServices.Filter(termo,false,false);
            }

            foreach (var usuario in listaUsuarios)
            {
                var user = await _userManager.FindByIdAsync(usuario.UserId);
                usuario.EmailConfirmado = user.EmailConfirmed;
            }

            var pagedList = await listaUsuarios.OrderBy(x=>x.NomeCompleto).ToList().ToPagedListAsync(pagina, 20);

            return View(new UsuarioListAdmViewModel(pagedList, termo));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string userId)
        {
            string status = null;
            var usuarioViewModel = await _usuarioAppServices.GetByUserIdAsync(userId);

            usuarioViewModel.Administrador = await GetAdmin(userId);

            var user = await _userManager.FindByIdAsync(usuarioViewModel.UserId);

            var empresaViewModel = await _empresaAppServices.GetEmpresaByUserId(usuarioViewModel.UserId) ?? new EmpresaViewModel();

            if (TempData["Error"] != null)
            {
                status = "Error";
                ModelState.AddModelError(string.Empty, TempData["Error"].ToString());
            }
            else if (TempData["Success"] != null)
            {
                status = "Success";
                ModelState.AddModelError(string.Empty, TempData["Success"].ToString());
            }

            return View(new UsuarioEmpresaAggregateViewModel(usuarioViewModel,empresaViewModel,user.Email,user.EmailConfirmed, status));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsuarioEmpresaAggregateViewModel usuarioEmpresaAggregateViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (usuarioEmpresaAggregateViewModel.NewEmail != usuarioEmpresaAggregateViewModel.Email)
                    {
                        var user = await _userManager.FindByIdAsync(usuarioEmpresaAggregateViewModel.Usuario.UserId);

                        var code = await _userManager.GenerateChangeEmailTokenAsync(user, usuarioEmpresaAggregateViewModel.NewEmail);

                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmailChange",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, email = usuarioEmpresaAggregateViewModel.NewEmail, code = code },
                            protocol: Request.Scheme);

                        var htmlMessage =
                            $"Olá,<br>" +
                            $"Nossa equipe alterou seu email. Para confirmar seu novo e-mail <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clique neste link</a>.<br><br>" +
                            $"Equipe de suporte ao cliente <br>" +
                            $"Prestige do Brasil";

                        await _suporteAppServices.SendAutomaticSuporteEmail(htmlMessage, usuarioEmpresaAggregateViewModel.NewEmail, "Alteração de email");
                    }

                    await _usuarioAppServices.UpdateAsync(usuarioEmpresaAggregateViewModel.Usuario);

                    TempData["Success"] = "Usuário atualizado com sucesso";
                }
                catch (Exception e)
                {
                    TempData["Error"] = e.Message;
                }
            }

            return RedirectToAction("Edit", new { userId = usuarioEmpresaAggregateViewModel.Usuario.UserId});
        }

        public async Task<IActionResult> SetarAdmin(string userId, bool statusAdmin)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (statusAdmin)
            {
                await _userManager.RemoveClaimAsync(user, new Claim("AdminClaim", string.Empty));
            }
            else
            {
                await _userManager.AddClaimAsync(user, new Claim("AdminClaim", string.Empty));
            }

            return RedirectToAction("Edit", new {userId});
        }

        [HttpGet]
        public async Task<IActionResult> GetPendentes(string termo, int pagina = 1)
        {
            IEnumerable<UsuarioViewModel> listaUsuarios;

            if (termo == null)
            {
                listaUsuarios = await _usuarioAppServices.GetPendentesAsync();
            }
            else
            {
                listaUsuarios = await _usuarioAppServices.Filter(termo, true, false);
            }

            var pagedList = await listaUsuarios.ToList().ToPagedListAsync(pagina, 20);

            return View("Pendentes",new UsuarioListAdmViewModel(pagedList, null));
        }

        [HttpGet]
        public async Task<IActionResult> GetBloqueados(string termo, int pagina = 1)
        {
            IEnumerable<UsuarioViewModel> listaUsuarios;

            if (termo == null)
            {
                listaUsuarios = await _usuarioAppServices.GetBloqueadosAsync();
            }
            else
            {
                listaUsuarios = await _usuarioAppServices.Filter(termo, false, true);
            }

            var pagedList = await listaUsuarios.ToList().ToPagedListAsync(pagina, 20);

            return View("Bloqueados", new UsuarioListAdmViewModel(pagedList, null));
        }

        [HttpGet]
        public async Task<IActionResult> StatusCadastro(string status, string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                await _usuarioAppServices.SetarStatusCadastroAsync(status,userId,user.Email);
                TempData["Success"] = "Usuário atualizado com sucesso";
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }

            return RedirectToAction("Edit", new { userId });
        }

        private async Task<bool> GetAdmin(string userId)
        {
            var usuariosAdm = await _userManager.GetUsersForClaimAsync(new Claim("AdminClaim", string.Empty));

            var verificacao = usuariosAdm.Any(x => x.Id == userId);

            return verificacao;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateViewModel usuarioCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = usuarioCreateViewModel.Email, Email = usuarioCreateViewModel.Email, PhoneNumber = usuarioCreateViewModel.Celular};
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {

                    try
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code },
                            protocol: Request.Scheme);

                        await _usuarioAppServices.CreateAsync(new UsuarioViewModel(user.Id, usuarioCreateViewModel.Usuario.NomeCompleto,
                            false, usuarioCreateViewModel.Usuario.BloqueioManual, usuarioCreateViewModel.Usuario.Administrador,true, 0.0, false), user.Email,callbackUrl);

                        usuarioCreateViewModel.StatusModel = "Success";
                        ModelState.AddModelError(string.Empty,"Usuário cadastrado com sucesso");
                    }
                    catch (Exception)
                    {
                        await _userManager.DeleteAsync(user);
                        usuarioCreateViewModel.StatusModel = "Error";
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao salvar o cadastro. Tente novamente");
                    }
                }
            }

            return View(usuarioCreateViewModel);
        }

        public async Task<IActionResult> ReenviarConfirmacaoEmail(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code = code },
                    protocol: Request.Scheme);

                var htmlMessage =
                    $"Olá,<br>" +
                    $"Reenviamos o link para confirmar seu e-mail <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clique neste link</a>.<br><br>" +
                    $"Equipe de suporte ao cliente <br>" +
                    $"Prestige do Brasil";

                await _suporteAppServices.SendAutomaticSuporteEmail(htmlMessage, user.Email, "Confirmação de email");

                TempData["Success"] = "Sucesso ao enviar link.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Erro ao enviar link.";
            }

            return RedirectToAction("Edit", new { userId });

        }
    }
}
