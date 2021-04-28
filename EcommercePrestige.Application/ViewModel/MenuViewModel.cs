using System.Collections.Generic;

namespace EcommercePrestige.Application.ViewModel
{
    public class MenuViewModel
    {
        public IEnumerable<MarcaViewModel> Marcas { get; set; }

        public MenuViewModel(IEnumerable<MarcaViewModel> marcas)
        {
            Marcas = marcas;
        }
    }
}
