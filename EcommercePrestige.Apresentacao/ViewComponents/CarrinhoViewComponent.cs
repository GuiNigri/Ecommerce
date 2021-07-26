using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Application.Helpers;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePrestige.Apresentacao.ViewComponents
{
    public class CarrinhoViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var carrinho = SessionHelper.GetObjectFromJson<List<CarrinhoViewModel>>(HttpContext.Session, "cart");

            var carrinhoViewModel = new CarrinhoViewModel();

            if (carrinho != null)
            {
                carrinhoViewModel.QuantidadeTotalItens = carrinho.Count();
            }
            else
            {
                carrinhoViewModel.CarrinhoViewModels = new List<CarrinhoViewModel>();
            }

            return View(carrinhoViewModel);
        }
    }
}
