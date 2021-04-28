using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace EcommercePrestige.Application.ViewModel
{
    public class ImpressaoPedidoViewModel:BaseViewModel
    {
        public PedidoDetailsViewModel PedidoViewModel { get; set; }
        public IEnumerable<ProdutosPedidoViewModel> ProdutosPedidoViewModel { get; set; }
        public string Email { get; set; }
        public EmpresaViewModel Empresa { get; set; }
        public string Contato { get; set; }
        public int QuantidadeArmacoes { get; set; }

        public ImpressaoPedidoViewModel(PedidoDetailsViewModel pedidoViewModel, IEnumerable<ProdutosPedidoViewModel> produtosPedidoViewModel, string email, EmpresaViewModel empresa, string contato, int quantidadeArmacoes)
        {
            PedidoViewModel = pedidoViewModel;
            ProdutosPedidoViewModel = produtosPedidoViewModel;
            Email = email;
            Empresa = empresa;
            Contato = contato;
            QuantidadeArmacoes = quantidadeArmacoes;
        }

        public ImpressaoPedidoViewModel()
        {
        }
    }
}
