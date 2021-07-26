using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommercePrestige.Apresentacao.Areas.Identity.Pages.Account.Manage
{
    public class EmpresaModel : PageModel
    {
        private readonly IEmpresaAppServices _empresaAppServices;
        private readonly UserManager<IdentityUser> _userManager;

        public EmpresaModel(IEmpresaAppServices empresaAppServices, UserManager<IdentityUser> userManager)
        {
            _empresaAppServices = empresaAppServices;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Display(Name = "CNPJ")]
            public string Cnpj { get; set; }
            
            [Display(Name = "Razão Social")]
            public string RazaoSocial { get; set; }
            
            [Display(Name = "CNAE")]
            public string Cnae { get; set; }
            
            [Display(Name = "CEP")]
            public string Cep { get; set; }
            
            [Display(Name = "Rua")]
            public string Logradouro { get; set; }
            
            [Display(Name = "Número")]
            public string Numero { get; set; }
            
            public string Complemento { get; set; }
            
            public string Bairro { get; set; }
            
            public string Municipio { get; set; }
            
            [Display(Name = "UF")]
            public string Uf { get; set; }
            
            public string Telefone { get; set; }
            
            [Display(Name = "Nome da ótica")]
            public string NomeOtica { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var empresa = await _empresaAppServices.GetEmpresaByUserId(user.Id);

            var cnpj = $@"{Convert.ToInt64(empresa.Cnpj):00\.000\.000\/0000\-00}";
            var cnae = $@"{Convert.ToInt32(empresa.Cnae):00\.00\-0\-00}";
            var cep = $@"{Convert.ToInt32(empresa.Cep):00\.000\-000}";
            var tel = $@"{Convert.ToInt64(empresa.Telefone):(00\) 0000\-0000}";

            Input = new InputModel
            {
                Bairro = empresa.Bairro,
                Numero = empresa.Numero,
                Municipio = empresa.Municipio,
                Complemento = empresa.Complemento,
                Uf = empresa.Uf,
                Cep = cep,
                RazaoSocial = empresa.RazaoSocial,
                Cnae = cnae,
                Logradouro = empresa.Logradouro,
                Telefone = tel,
                NomeOtica = empresa.NomeOtica,
                Cnpj = cnpj
            };
        }
        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            await LoadAsync(user);
        }
    }
}
