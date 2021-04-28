using System;
using System.Collections.Generic;
using System.Text;

namespace EcommercePrestige.Application.ViewModel
{
    class CieloViewModel:BaseViewModel
    {
        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string BandeiraCartao { get; set; }
        public DateTime ValidadeCartao { get; set; }
        public string CvvCartao { get; set; }
        public decimal ValorCompraCartao { get; set; }
        public int Parcelas { get; set; }




        public string CpfCartao { get; set; }
        public int NascimentoDiaCartao { get; set; }
        public int NascimentoMesCartao { get; set; }
        public int NascimentoAnoCartao { get; set; }
        public string EmailCartao { get; set; }



        public string CepCartao { get; set; }
        public string CidadeCartao { get; set; }
        public string Ufcartao { get; set; }
        public string ComplementoCartao { get; set; }
        public string BairroCartao { get; set; }
        public string RuaCartao { get; set; }
        public string NumeroEndCartao { get; set; }
        public string PaisCartao { get; set; }
    }
}
