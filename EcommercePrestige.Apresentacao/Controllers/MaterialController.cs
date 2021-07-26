using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace EcommercePrestige.Apresentacao.Controllers
{
    [Authorize(policy: "Admin")]
    public class MaterialController : Controller
    {
        private readonly IMaterialAppServices _materialAppServices;

        public MaterialController(IMaterialAppServices materialAppServices)
        {
            _materialAppServices = materialAppServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string termo, int pagina = 1)
        {
            IEnumerable<MaterialViewModel> listaMaterial;
            if (termo == null)
            {
               listaMaterial = await _materialAppServices.GetAllAsync();
            }
            else
            {
                listaMaterial = await _materialAppServices.FilterAsync(termo);
            }
            
            var pagedList = await listaMaterial.ToList().ToPagedListAsync(pagina, 20);

            return View(new MaterialListAdmViewModel(pagedList,termo));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {


            return View(new MaterialViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialViewModel materialViewModel)
        {
            try
            {
                await _materialAppServices.CreateAsync(materialViewModel);

                materialViewModel.StatusModel = "Success";
                ModelState.AddModelError(string.Empty, "Criado com sucesso");
            }
            catch (Exception e)
            {
                materialViewModel.StatusModel = "Error";
                ModelState.AddModelError(string.Empty, e.Message);
            }

            return View(materialViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var materialViewModel = await _materialAppServices.GetByIdAsync(id);

            return View(materialViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MaterialViewModel materialViewModel)
        {
            try
            {
                await _materialAppServices.UpdateAsync(materialViewModel);

                materialViewModel.StatusModel = "Success";
                ModelState.AddModelError(string.Empty, "Alterado com sucesso");
            }
            catch (Exception e)
            {
                materialViewModel.StatusModel = "Error";
                ModelState.AddModelError(string.Empty, e.Message);
            }

            return View(materialViewModel);
        }
    }
}
