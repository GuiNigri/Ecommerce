using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using EcommercePrestige.Model.Exceptions;
using Microsoft.AspNetCore.WebUtilities;

namespace EcommercePrestige.Apresentacao.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmpresaAppServices _empresaAppServices;
        private readonly ISuporteAppServices _suporteAppServices;


        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IEmpresaAppServices empresaAppServices, ISuporteAppServices suporteAppServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _empresaAppServices = empresaAppServices;
            _suporteAppServices = suporteAppServices;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public bool BloqueioAutomatico { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "E-mail obrigatório")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }


            [Required(ErrorMessage = "Senha obrigatória")]
            [StringLength(100, ErrorMessage = "A {0} deve ter no mínimo {2} caracteres", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirme a senha")]
            [Compare("Password", ErrorMessage = "As senhas não conferem")]
            public string ConfirmPassword { get; set; }
            
            [Required(ErrorMessage = "Contato obrigatório")]
            [StringLength(100, ErrorMessage = "O {0} deve ter no mínimo {2} caracteres", MinimumLength = 2)]
            [DataType(DataType.Text)]
            [Display(Name = "Contato")]
            public string NomeCompleto { get; set; }
            public string Cnpj { get; set; }

            public string CnpjConsulta { get; set; }

            [Required]
            public string RazaoSocial { get; set; }
            [Required]
            public string Cnae { get; set; }
            [Required]
            public string Cep { get; set; }
            [Required]
            public string Logradouro { get; set; }
            [Required]
            public string Numero { get; set; }

            [Required(ErrorMessage = "Complemento obrigatório")]
            [DataType(DataType.Text)]
            [Display(Name = "Complemento do endereço")]
            public string Complemento { get; set; }
            [Required]
            public string Bairro { get; set; }
            [Required]
            public string Municipio { get; set; }
            [Required]
            public string Uf { get; set; }

            [Required(ErrorMessage = "Número de telefone obrigatório")]
            [DataType(DataType.PhoneNumber)]
            [StringLength(11, ErrorMessage = "O {0} deve incluir o DDD da região, e ter entre {2} e {1} caracteres", MinimumLength = 10)]
            public string Telefone { get; set; }

            [Required(ErrorMessage = "Nome fantasia obrigatório")]
            public string NomeOtica { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                ReturnUrl = returnUrl;
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return null;
        }

        public async Task<IActionResult> OnPostAsync(string CnpjConsulta, string captcha, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (returnUrl == "Consulta")
            {
                await ConsultarDadosCnpjAsync(CnpjConsulta).ConfigureAwait(false);

                return Page();
            }

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid) return Page();

            try
            {
                var empresaViewModel = await ConvertInputModelToModelEmpresa().ConfigureAwait(false);

                var (resultStatusFirst, _) = await VerificarExistenciaEmpresa(empresaViewModel.Cnpj);

                if (!resultStatusFirst)
                {
                    var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                    var result = await _userManager.CreateAsync(user, Input.Password);

                    if (result.Succeeded)
                    {

                        var usuarioViewModel = await ConvertInputModelToModelUsuario().ConfigureAwait(false);

                        empresaViewModel.UserId = user.Id;
                        usuarioViewModel.UserId = user.Id;


                        try
                        {
                            if(empresaViewModel.Numero == "S/N")
                            {
                                empresaViewModel.Numero = "0";
                            }

                            await _empresaAppServices.Create(empresaViewModel, usuarioViewModel);

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                            var callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new { area = "Identity", userId = user.Id, code = code },
                                protocol: Request.Scheme);

                            var htmlMessage =
                                $"Olá, {Input.NomeCompleto}!<br>" +
                                $"Obrigado por se cadastrar em nosso site. Para confirmar seu e-mail <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clique neste link</a>.<br>" +
                                $"Logo retornaremos o contato informando o status da liberação.<br><br>" +
                                $"Att<br>" +
                                $"Equipe Prestige do Brasil";

                            await _suporteAppServices.SendAutomaticSuporteEmail(htmlMessage, Input.Email, "Cadastro realizado com sucesso");

                            return RedirectToPage("./RegisterConfirmation", new { email = Input.Email });
                        }
                        catch (Exception ex)
                        {
                            var (resultStatus, resultDados) = await VerificarExistenciaEmpresa(empresaViewModel.Cnpj);

                            if (resultStatus)
                            {
                                await _empresaAppServices.DeleteAsync(resultDados.Id);
                            }

                            await _userManager.DeleteAsync(user);
                            ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao finalizar o seu cadastro, por favor, tente novamente");
                            return Page();
                        }

                    }

                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "DuplicateUserName")
                        {
                            error.Description = $"{Input.Email} já está cadastrado, utilize a opção esqueci minha senha.";
                        }

                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "Empresa já cadastrada");
                    return Page();
                }

            }
            catch (ModelValidationExceptions e)
            {
                ModelState.AddModelError(e.PropertyName, e.Message);

            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }

            return Page();
        }


        private async Task ConsultarDadosCnpjAsync(string cnpj)
        {
            var (resultStatus, _) = await VerificarExistenciaEmpresa(cnpj);

            if (!resultStatus)
            {
                var (empresaModel, mensagem) = await _empresaAppServices.GetDadosEmpresaAsync(cnpj);

                if (empresaModel != null)
                {
                    var cnpjConsulta = Input.CnpjConsulta.Replace(".", "").Replace("/", "").Replace("-", "");

                    Input = new InputModel
                    {

                        RazaoSocial = empresaModel.RazaoSocial,
                        Cnae = empresaModel.Cnae,
                        Cnpj = empresaModel.Cnpj,
                        CnpjConsulta = cnpjConsulta,
                        Numero = empresaModel.Numero,
                        Complemento = empresaModel.Complemento,
                        Municipio = empresaModel.Municipio,
                        Bairro = empresaModel.Bairro,
                        Cep = empresaModel.Cep,
                        Logradouro = empresaModel.Logradouro,
                        Uf = empresaModel.Uf,
                        Telefone = empresaModel.Telefone

                    };

                    if (!empresaModel.Cnae.Contains("47.74-1-00"))
                    {
                        var mensagemCnae = "A atividade principal da empresa consultada não está relacionada ao ramo ótico," +
                                           " por esse motivo o cadastro será analisado antes de ser aprovado." + Environment.NewLine +
                                           "Fique tranquilo, fazemos esta ação por questões de segurança. Dentro de 48 horas responderemos a solicitação via e-mail";

                        ModelState.AddModelError(string.Empty, mensagemCnae);
                    }
                }

                if (mensagem != null)
                {
                    ModelState.AddModelError(string.Empty, mensagem);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty,"Empresa já cadastrada, digite outro CNPJ.");
            }

        }

        private async Task<(bool, EmpresaViewModel)> VerificarExistenciaEmpresa(string cnpj)
        {
            var resultEmpresa = await _empresaAppServices.GetEmpresaByCnpj(cnpj);

            return resultEmpresa == null ? (false, null) : (true, resultEmpresa);
        }

        private async Task<EmpresaViewModel> ConvertInputModelToModelEmpresa()
        {
            var cnpj = Input.Cnpj;
            cnpj = cnpj.Replace(".","");
            cnpj = cnpj.Replace("/", "");
            cnpj = cnpj.Replace("-", "");

            var cep = Input.Cep;
            cep = cep.Replace(".", "");
            cep = cep.Replace("-", "");

            var empresaViewModel = new EmpresaViewModel()
            {
                Cnpj = cnpj,
                Cnae = new string(Input.Cnae.Where(char.IsDigit).ToArray()),
                Logradouro = Input.Logradouro,
                Numero = Input.Numero,
                Bairro = Input.Bairro,
                Municipio = Input.Municipio,
                Uf = Input.Uf,
                RazaoSocial = Input.RazaoSocial,
                Cep = cep,
                Complemento = Input.Complemento,
                Telefone = Input.Telefone,
                NomeOtica = Input.NomeOtica
            };

            return empresaViewModel;

        }

        private async Task<UsuarioViewModel> ConvertInputModelToModelUsuario()
        {
            if (Input.Cnae.Contains("47.74-1-00"))
            {
                BloqueioAutomatico = false;
            }
            else
            {
                BloqueioAutomatico = true;
            }

            var usuarioViewModel = new UsuarioViewModel
            {
                NomeCompleto = Input.NomeCompleto,
                BloqueioManual = false,
                BloqueioAutomatico = BloqueioAutomatico,
                DescontoCliente = 0,
                Exibir = true
            };

            return usuarioViewModel;

        }
    }
}
