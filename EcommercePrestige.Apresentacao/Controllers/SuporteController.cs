using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePrestige.Apresentacao.Controllers
{
    public class SuporteController : Controller
    {
        private readonly ISuporteAppServices _suporteAppServices;

        public SuporteController(ISuporteAppServices suporteAppServices)
        {
            _suporteAppServices = suporteAppServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(new SuporteInputModel());
        }

        [HttpGet]
        public async Task<IActionResult> Politicas()
        {
            return View();
        }

    }
}
