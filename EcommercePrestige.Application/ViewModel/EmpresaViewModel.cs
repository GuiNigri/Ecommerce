using System.ComponentModel.DataAnnotations;

namespace EcommercePrestige.Application.ViewModel
{
    public class EmpresaViewModel: BaseViewModel
    {

        [Required]
        public string UserId { get; set; }
        [Required]
        public string RazaoSocial { get; set; }

        [Required]
        public string Cnpj { get; set; }

        [Required]
        public string Cnae { get; set; }
        [Required]
        public string Logradouro { get; set; }
        [Required]
        public string Numero { get; set; }
        [Required]
        public string Complemento { get; set; }
        [Required]
        public string Cep { get; set; }
        [Required]
        public string Bairro { get; set; }
        [Required]
        public string Municipio { get; set; }
        [Required]
        public string Uf { get; set; }
        [Required]
        public string Telefone { get; set; }
        [Required]
        public string NomeOtica { get; set; }

    }
}
