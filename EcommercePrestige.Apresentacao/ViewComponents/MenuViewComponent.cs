using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePrestige.Apresentacao.ViewComponents
{
    public class MenuViewComponent:ViewComponent
    {
        private readonly IMarcaAppServices _marcaAppServices;
        private readonly IMaterialAppServices _materialAppServices;

        public MenuViewComponent(IMarcaAppServices marcaAppServices, IMaterialAppServices materialAppServices)
        {
            _marcaAppServices = marcaAppServices;
            _materialAppServices = materialAppServices;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var marcas = await _marcaAppServices.GetAllAsync();
            return View(new MenuViewModel(marcas));
        }
    }
}
