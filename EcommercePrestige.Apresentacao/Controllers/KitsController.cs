using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace EcommercePrestige.Apresentacao.Controllers
{
    public class KitsController : Controller
    {
        private readonly IKitAppServices _kitsAppServices;
        private readonly IProdutoFotoAppServices _produtoFotoAppServices;
        private readonly SignInManager<IdentityUser> _signInManager;

        public KitsController(IKitAppServices kitsAppServices, IProdutoFotoAppServices produtoFotoAppServices, SignInManager<IdentityUser> signInManager)
        {
            _kitsAppServices = kitsAppServices;
            _produtoFotoAppServices = produtoFotoAppServices;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var statusModel = "success";

            if (TempData["Error"] != null)
            {
                statusModel = "Error";
                ModelState.AddModelError(string.Empty, TempData["Error"].ToString());
            }
            else if (TempData["Success"] != null)
            {
                statusModel = "Success";
                ModelState.AddModelError(string.Empty, TempData["Success"].ToString());
            }

            var kitsViewModel = await _kitsAppServices.GetKitsAsync(id,"AT");

            var logado = _signInManager.IsSignedIn(User);

            return View(new KitsViewModel(kitsViewModel,statusModel, logado));
        }

        [HttpGet]
        public async Task<ActionResult> GetImgByProduct(int id)
        {
            var foto = await _produtoFotoAppServices.GetFotosToKitAsync(id);

            return Content(string.Join("", foto.UriBlob));
        }

        //adm-----------------------------------------------------------------
        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> List(string termo, int pagina = 1)
        {
            IEnumerable<KitsViewModel> kitsList;

            if (termo == null)
            {
                kitsList = await _kitsAppServices.GetAllAsync();
            }
            else
            {
                kitsList = await _kitsAppServices.FilterAsync(termo);
            }

            var pagedList = await kitsList.ToList().ToPagedListAsync(pagina, 21);

            return View(new KitListAdmViewModel(pagedList, termo));
        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var kitViewModel = await _kitsAppServices.GetByIdAsync(id);


            return View(kitViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> Edit(KitsViewModel kitsViewModel)
        {
            try
            {
                await _kitsAppServices.UpdateAsync(kitsViewModel);
                kitsViewModel.StatusModel = "Success";
                ModelState.AddModelError(string.Empty, "Alterado com sucesso");
            }
            catch (Exception e)
            {
                kitsViewModel.StatusModel = "Error";
                ModelState.AddModelError(string.Empty, e.Message);
            }
            

            return View(kitsViewModel);
        }

    }
}
