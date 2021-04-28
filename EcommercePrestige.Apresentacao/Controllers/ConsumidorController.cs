using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePrestige.Apresentacao.Controllers
{
    public class ConsumidorController : Controller
    {
        private readonly IEmpresaAppServices _empresaAppServices;

        public ConsumidorController(IEmpresaAppServices empresaAppServices)
        {
            _empresaAppServices = empresaAppServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index(IEnumerable<EmpresaViewModel> listaConsumidorViewModels)
        {
            var listCidades = await _empresaAppServices.GetCidadesEmpresasAsync();

            return View("Index", new ConsumidorViewModel(listCidades, listaConsumidorViewModels));
        }

        [HttpPost]
        public async Task<ActionResult> GetBairrosByCidade(string city)
        {
            var cidadesViewModel = new CidadesViewModel(city);

            var bairros = await _empresaAppServices.GetBairrosByCidadesAsync(cidadesViewModel);

            return Content(string.Join("", bairros));
        }

        [HttpGet]
        public async Task<IActionResult> GetOticas(string cidade, string bairro)
        {
            if (cidade != null && bairro != null)
            {
                var listaEmpresas = await _empresaAppServices.GetListaDeEmpresasByCidadesEBairro(cidade, bairro);

                return await Index(listaEmpresas.ToList()).ConfigureAwait(false);
            }

            return RedirectToAction("Index", new { listaConsumidorViewModels = new List<EmpresaViewModel>()});
        }
    }
}
