using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePrestige.Apresentacao.ViewComponents
{
    public class BannersHomeViewComponent : ViewComponent
    {
        private readonly IHomeAppServices _homeAppServices;

        public BannersHomeViewComponent(IHomeAppServices homeAppServices)
        {
            _homeAppServices = homeAppServices;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banners = await _homeAppServices.GetAllBannersAsync();

            return View(new BannersHomeViewModel(banners, null));
        }
    }
}
