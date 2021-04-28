using System;
using System.ComponentModel;
using EcommercePrestige.Application.Enums;

namespace EcommercePrestige.Application.ViewModel
{
    public class SuporteViewModel:BaseViewModel
    {
        public string Protocolo { get; set; }
        [DisplayName("Nome")]
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Assunto { get; set; }
        public string Mensagem { get; set; }
        public string Documento { get; set; }
        public DateTime Data { get; set; }
        public SuporteStatus Status { get; set; }

        public SuporteViewModel()
        {
        }
    }
}
