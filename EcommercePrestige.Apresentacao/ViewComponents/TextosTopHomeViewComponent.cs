using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePrestige.Apresentacao.ViewComponents
{
    public class TextosTopHomeViewComponent:ViewComponent
    {
        private readonly IHomeAppServices _homeAppServices;

        public TextosTopHomeViewComponent(IHomeAppServices homeAppServices)
        {
            _homeAppServices = homeAppServices;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var textos = await _homeAppServices.GetAllTextosAsync();

            return View(new TextoHomeViewModel(textos,null));
        }
    }
}
