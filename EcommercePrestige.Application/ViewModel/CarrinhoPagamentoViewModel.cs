using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EcommercePrestige.Model.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommercePrestige.Application.ViewModel
{
    public class CarrinhoPagamentoViewModel:BaseViewModel
    {
        public SelectListItem[] FormasPagamento { get; set; }
        [Required]
        public string FormaPagamento { get; set; }
        [Required]
        public string Parcelas { get; set; }
        public string Frete { get; set; }
        public int QuantidadeArmacoes { get; set; }
        public double ValorPedido { get; set; }
        [Required]
        public string Cep { get; set; }
        [Required]
        public string Rua { get; set; }
        [Required]
        public string Numero { get; set; }
        [Required]
        public string Complemento { get; set; }
        [Required]
        public string Bairro { get; set; }
        [Required]
        public string Cidade { get; set; }
        [Required]
        public string Estado { get; set; }
        public string ObsPagamento { get; set; }
        public IEnumerable<CorreioWebServiceViewModel> ModalidadeFretes { get; set; }

        public CarrinhoPagamentoViewModel()
        {
            FormasPagamento = PopulateFormasPagamento();
        }

        public CarrinhoPagamentoViewModel(CorreioAddressModel correioAddressModel)
        {
            Cep = correioAddressModel.Cep;
            Rua = correioAddressModel.Rua;
            Bairro = correioAddressModel.Bairro;
            Cidade = correioAddressModel.Cidade;
            Estado = correioAddressModel.Estado;
        }

        public CarrinhoPagamentoViewModel(EmpresaViewModel empresaViewModel, IEnumerable<CarrinhoViewModel> carrinho, double valorTotal,string statusModel, IEnumerable<CorreioWebServiceViewModel> modalidadeFretes)
        {
            var cart = carrinho.ToList();

            var frete = "A combinar";

            if (valorTotal > 1500)
            {
                frete = "Frete grátis";
            }

            Cep = empresaViewModel.Cep;
            Rua = empresaViewModel.Logradouro;
            Bairro = empresaViewModel.Bairro;
            Cidade = empresaViewModel.Municipio;
            Estado = empresaViewModel.Uf;
            Numero = empresaViewModel.Numero;
            Complemento = empresaViewModel.Complemento;
            FormasPagamento = PopulateFormasPagamento(valorTotal);
            Frete = frete;
            QuantidadeArmacoes = cart.Sum(x => x.QuantidadeIndividual);
            ValorPedido = valorTotal;
            StatusModel = statusModel;
            ModalidadeFretes = modalidadeFretes;

        }

        private static SelectListItem[] PopulateFormasPagamento(double valorTotal = 0)
        {

            SelectListItem[] lista;

            if (valorTotal >= 350)
            {
                lista = new[]
                {
                    new SelectListItem {Value = "", Text = "Selecione"},
                    new SelectListItem {Value = "boleto", Text = "Boleto bancário"},
                    new SelectListItem {Value = "cheque", Text = "Cheque"},
                    new SelectListItem{Value = "deposito",Text = "Depósito em conta"},
                    new SelectListItem{Value = "cartao",Text = "Cartão de crédito"}
                };
            }
            else
            {
                lista = new[]
                {
                    new SelectListItem {Value = "", Text = "Selecione..."},
                    new SelectListItem {Value = "boleto", Text = "Boleto bancário"},
                    new SelectListItem{Value = "deposito",Text = "Depósito em conta"},
                    new SelectListItem{Value = "cartao",Text = "Cartão de crédito"}

                };
            }

            return lista;
        }


    }


}
