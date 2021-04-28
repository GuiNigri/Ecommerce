using System;
using System.Collections.Generic;
using System.Text;

namespace EcommercePrestige.Application.ViewModel
{
    public class TextoHomeViewModel:BaseViewModel
    {
        public string Texto { get; set; }
        
        public IEnumerable<TextoHomeViewModel> Textos { get; set; }

        public TextoHomeViewModel()
        {
        }

        public TextoHomeViewModel(IEnumerable<TextoHomeViewModel> textos, string statusModel)
        {
            Textos = textos;
            StatusModel = statusModel;
        }

    }
}
