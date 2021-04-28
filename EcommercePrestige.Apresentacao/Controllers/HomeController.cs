using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AspNetCore.SEOHelper.Sitemap;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EcommercePrestige.Apresentacao.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProdutoAppServices _produtoAppServices;
        private readonly IHomeAppServices _homeAppServices;
        private readonly IMarcaAppServices _marcaAppServices;
        private readonly IWebHostEnvironment _env;

        public HomeController(IProdutoAppServices produtoAppServices, IHomeAppServices homeAppServices, IMarcaAppServices marcaAppServices, IWebHostEnvironment env)
        {
            _produtoAppServices = produtoAppServices;
            _homeAppServices = homeAppServices;
            _marcaAppServices = marcaAppServices;
            _env = env;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            CreateSitemapInRootDirectory();

            var marcas = await _marcaAppServices.GetAllAsync();

            var marcaPeople = 0;
            var marcaPrestige = 0;
            var marcaAzzaro = 0;

            foreach (var item in marcas)
            {
                if (item.Nome.ToLower() == "people")
                {
                    marcaPeople = item.Id;
                }
                else if (item.Nome.ToLower() == "prestige")
                {
                    marcaPrestige = item.Id;
                }
                else if (item.Nome.ToLower() == "azzaro")
                {
                    marcaAzzaro = item.Id;
                }
            }

            var carrosselPrestige = await _produtoAppServices.GetCategoryForHomeAsync("bestSeller", marcaPrestige);
            var carrosselPeople = await _produtoAppServices.GetCategoryForHomeAsync("bestSeller", marcaPeople);
            var carrosselAzzaro = await _produtoAppServices.GetCategoryForHomeAsync("bestSeller", marcaAzzaro);

            return View(new HomeViewModel(carrosselPeople, carrosselPrestige, carrosselAzzaro));
        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> EditTextos()
        {
            var listaTextos = await _homeAppServices.GetAllTextosAsync();

            var statusModel = "";

            if (TempData["Success"] != null)
            {
                statusModel = "Success";
                ModelState.AddModelError(string.Empty, TempData["Success"].ToString());
            }
            else if (TempData["Error"] != null)
            {
                statusModel = "Error";
                ModelState.AddModelError(string.Empty, TempData["Error"].ToString());
            }
            
            return View(new TextoHomeViewModel(listaTextos, statusModel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> AdicionarTextoTopHome(TextoHomeViewModel textoHomeViewModel)
        {
            try
            {
                await _homeAppServices.CreateTextoAsync(textoHomeViewModel);
                TempData["Success"] = "Adicionado com sucesso";
            }
            catch (Exception)
            {
                TempData["Error"] = "Erro ao adicionar texto";
            }


            return RedirectToAction("EditTextos", "Home");
        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> RemoverTextoTopHome(int id)
        {
            try
            {
                await _homeAppServices.DeleteTextoAsync(id);
                TempData["Success"] = "Excluido com sucesso";
            }
            catch (Exception)
            {
                TempData["Error"] = "Erro ao excluir texto";
            }

            
            return RedirectToAction("EditTextos", "Home");
        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> EditBanners()
        {
            var listaBanners = await _homeAppServices.GetAllBannersAsync();

            var statusModel = "";

            if (TempData["Success"] != null)
            {
                statusModel = "Success";
                ModelState.AddModelError(string.Empty, TempData["Success"].ToString());
            }
            else if (TempData["Error"] != null)
            {
                statusModel = "Error";
                ModelState.AddModelError(string.Empty, TempData["Error"].ToString());
            }

            return View(new BannersHomeViewModel(listaBanners, statusModel));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> AdicionarBanner(IFormFile banner)
        {
            try
            {
                await _homeAppServices.CreateBannerAsync(banner.OpenReadStream());
                TempData["Success"] = "Adicionado com sucesso";
            }
            catch (Exception)
            {
                TempData["Error"] = "Erro ao adicionar banner";
            }


            return RedirectToAction("EditBanners", "Home");
        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> RemoverBanner(int id)
        {
            try
            {
                await _homeAppServices.DeleteBannerAsync(id);
                TempData["Success"] = "Excluido com sucesso";
            }
            catch (Exception)
            {
                TempData["Error"] = "Erro ao excluir banner";
            }


            return RedirectToAction("EditBanners", "Home");
        }

        public string CreateSitemapInRootDirectory()
        {
            var list = new List<SitemapNode>();
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://prestigedobrasil.com.br/", Frequency = SitemapFrequency.Daily });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://prestigedobrasil.com.br/Produtos/GetCategory?marca=Azzaro", Frequency = SitemapFrequency.Daily });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://prestigedobrasil.com.br/Produtos/GetCategory?marca=People", Frequency = SitemapFrequency.Daily });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://prestigedobrasil.com.br/Produtos/GetCategory?marca=Prestige", Frequency = SitemapFrequency.Daily });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://prestigedobrasil.com.br/Identity/Account/Register", Frequency = SitemapFrequency.Daily });

            new SitemapDocument().CreateSitemapXML(list, _env.ContentRootPath);
            return "sitemap.xml file should be create in root directory";
        }

    }
}