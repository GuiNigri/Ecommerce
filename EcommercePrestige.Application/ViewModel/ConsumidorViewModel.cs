using System.Collections.Generic;

namespace EcommercePrestige.Application.ViewModel
{
    public class ConsumidorViewModel:BaseViewModel
    {
        public IEnumerable<CidadesViewModel> CidadesList { get; set; }
        public IEnumerable<EmpresaViewModel> EmpresasList { get; set; }

        public ConsumidorViewModel(IEnumerable<CidadesViewModel> cidadesList, IEnumerable<EmpresaViewModel> empresasList)
        {
            CidadesList = cidadesList;
            EmpresasList = empresasList;
        }
    }

    public class CidadesViewModel : BaseViewModel
    {
        public string Cidade { get; set; }

        public CidadesViewModel(){}
        public CidadesViewModel(string cidade)
        {
            Cidade = cidade;
        }
    }
}
