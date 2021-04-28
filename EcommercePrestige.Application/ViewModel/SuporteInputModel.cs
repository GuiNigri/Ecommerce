using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommercePrestige.Application.ViewModel
{
    public class SuporteInputModel:BaseViewModel
    {

        [DisplayName("Nome")]
        public string Nome { get; set; }
        
        public string Email { get; set; }

        [DisplayName("CPF")]
        public string Cpf { get; set; }

        [DisplayName("CNPJ")]
        public string Cnpj { get; set; }
        [DisplayName("Assuntos")]
        public SelectListItem[] AssuntosConsumidorFinal { get; set; }
        [DisplayName("Assuntos")]
        public SelectListItem[] AssuntosEmpresa { get; set; }

        public string Assuntos { get; set; }     
        [Required]
        public string Mensagem { get; set; }

        public SuporteInputModel(string statusModel)
        {
            AssuntosConsumidorFinal = PopulateAssuntosFinal();
            AssuntosEmpresa = PopulateAssuntosEmpresa();
            StatusModel = statusModel;
        }

        public SuporteInputModel()
        {
            AssuntosConsumidorFinal = PopulateAssuntosFinal();
            AssuntosEmpresa = PopulateAssuntosEmpresa();
        }

        private static SelectListItem[] PopulateAssuntosFinal()
        {
            var lista = new[]
            {
                new SelectListItem{Value = "Duvidas Gerais",Text = "Dúvidas gerais"}

            };
            return lista;
        }

       private static SelectListItem[] PopulateAssuntosEmpresa()
       {

       
           var lista = new[]
           {
               new SelectListItem{Value = "Assistencia Tecnica",Text = "Assistência técnica"},
               new SelectListItem{Value = "Duvidas Gerais",Text = "Dúvidas gerais"}
       
           };
           return lista;
       }
    }

    
}
