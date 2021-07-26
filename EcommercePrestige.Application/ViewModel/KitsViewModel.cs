using System.Collections.Generic;

namespace EcommercePrestige.Application.ViewModel
{
    public class KitsViewModel:BaseViewModel
    {
        public string Nome { get; set; }
        public string ValorVenda { get; set; }
        public string Descricao { get; set; }
        public string fotoPrincipal { get; set; }
        public IEnumerable<KitsProdutoViewModel> Produtos { get; set; }
        public string StatusVenda { get; set; }
        public bool Logado { get; set; }

        public KitsViewModel(KitsViewModel kitsViewModel,string statusModel, bool logado)
        {
            Id = kitsViewModel.Id;
            Nome = kitsViewModel.Nome;
            ValorVenda = kitsViewModel.ValorVenda;
            Descricao = kitsViewModel.Descricao;
            fotoPrincipal = kitsViewModel.fotoPrincipal;
            Produtos = kitsViewModel.Produtos;
            StatusVenda = kitsViewModel.StatusVenda;
            StatusModel = statusModel;
            Logado = logado;
        }

        public KitsViewModel()
        {
        }
    }
}
