using System;
using System.Collections.Generic;
using EcommercePrestige.Application.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace EcommercePrestige.Application.ViewModel
{
    public class PedidoViewModel:BaseViewModel
    {
        public DateTime DataPedido { get; set; }
        public string ValorTotal { get; set; }
        public PedidosStatus Status { get; set; }
    }

    public class PedidoDetailsViewModel : PedidoViewModel
    {
        public string FormaDePagamento { get; set; }
        public string Parcelas { get; set; }
        public string Rastreio { get; set; }
        public string userId { get; set; }
        public string SubTotal { get; set; }
        public string Desconto { get; set; }
        public string Frete { get; set; }
        public string TipoDeEnvio { get; set; }
    }

    public class PedidoAdmViewModel : BaseViewModel
    {
        public string Usuario { get; set; }
        public string UserId { get; set; }
        public int UsuarioId { get; set; }
        public string RazaoSocial  { get; set; }
        public string CNPJ { get; set; }
        public string NomeOtica { get; set; }
        public string Rastreio { get; set; }
        public SelectListItem[] FormaDePagamentoSelect { get; set; }
        public string FormaDePagamento { get; set; }
        public SelectListItem[] TipoDeEnvioSelect { get; set; }
        public string TipoDeEnvio { get; set; }
        public string Parcelas { get; set; }
        public string Subtotal { get; set; }
        public string Desconto { get; set; }
        public string Frete { get; set; }
        public string ValorTotal { get; set; }
        public string TransactionStatus { get; set; }
        public string Tid { get; set; }
        public string AuthorizationCode { get; set; }
        public string Gateway { get; set; }
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public DateTime DataPedido { get; set; }
        public PedidosStatus Status { get; set; }
        public string Obs { get; set; }
        public IPagedList<ProdutosPedidoViewModel> ProdutosPedido { get; set; }
        public List<SelectListItem> Referencias { get; set; }
        public List<SelectListItem> CoresSelect { get; set; }

        public PedidoAdmViewModel()
        {
            FormaDePagamentoSelect = PopulateFormasPagamento();
            TipoDeEnvioSelect = PopulateTipoEnvio();
        }


        private static SelectListItem[] PopulateFormasPagamento()
        {
            var lista = new[]
            {
                new SelectListItem {Value = "boleto", Text = "Boleto bancário"},
                new SelectListItem {Value = "cheque", Text = "Cheque"},
                new SelectListItem{Value = "deposito",Text = "Depósito em conta"},
                new SelectListItem{Value = "cartao",Text = "Cartão de crédito"}

            };
            return lista;
        }

        private static SelectListItem[] PopulateTipoEnvio()
        {
            var lista = new[]
            {
                new SelectListItem {Value = "", Text = "Selecione ..."},
                new SelectListItem {Value = "correios", Text = "Correios"},
                new SelectListItem{Value = "proprio",Text = "Próprio"}
            };
            return lista;
        }
    }
}
