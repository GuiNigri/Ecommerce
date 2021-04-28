using System.Collections.Generic;

namespace EcommercePrestige.Application.ViewModel
{
    public class HomeViewModel:BaseViewModel
    {
        public IEnumerable<ProdutoViewModel> ListPeople { get; set; }
        public IEnumerable<ProdutoViewModel> ListPrestige { get; set; }
        public IEnumerable<ProdutoViewModel> ListAzzaro { get; set; }

        public HomeViewModel(IEnumerable<ProdutoViewModel> listPeople, IEnumerable<ProdutoViewModel> listPrestige, IEnumerable<ProdutoViewModel> listAzzaro)
        {
            ListPeople = listPeople;
            ListPrestige = listPrestige;
            ListAzzaro = listAzzaro;
        }
    }

}
