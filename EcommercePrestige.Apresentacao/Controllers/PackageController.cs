using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePrestige.Apresentacao.Controllers
{
    public class PackageController : Controller
    {
        private readonly IPackageAppServices _packageAppServices;

        public PackageController(IPackageAppServices packageAppServices)
        {
            _packageAppServices = packageAppServices;
        }

        [HttpGet]
        public async Task<IActionResult> Tracking()
        {
            var trackingViewModel = new TrackingHistoryViewModel
            {
                PackageTrackingList = new List<TrackingHistoryViewModel>()
            };

            return View(trackingViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PackageTracking(string trackingCode, int pedido)
        {
            var historicoRastreio = await _packageAppServices.GetTracking(trackingCode, pedido);

            var trackingHistoryViewModels = historicoRastreio.ToList();

            if (!trackingHistoryViewModels.Any())
            {
                ModelState.AddModelError(string.Empty, "Registro não encontrado");
            }

            var trackingViewModel = new TrackingHistoryViewModel {PackageTrackingList = trackingHistoryViewModels};


            return View("Tracking",trackingViewModel);
        }
    }
}
