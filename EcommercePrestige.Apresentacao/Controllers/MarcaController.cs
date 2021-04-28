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
    public class MarcaController : Controller
    {
        private readonly IMarcaAppServices _marcaAppServices;

        public MarcaController(IMarcaAppServices marcaAppServices)
        {
            _marcaAppServices = marcaAppServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string termo, int pagina = 1)
        {
            IEnumerable<MarcaViewModel> listaMarcas;

            if (termo == null)
            {
                listaMarcas = await _marcaAppServices.GetAllAsync();
            }
            else
            {
                listaMarcas = await _marcaAppServices.Filter(termo);
            }
            
            var listaPaged = await listaMarcas.ToList().ToPagedListAsync(pagina, 20);

            return View(new MarcaListAdmViewModel(listaPaged, termo));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new MarcaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")]MarcaViewModel marcaViewModel)
        {
            try
            {
                await _marcaAppServices.CreateAsync(marcaViewModel);
                marcaViewModel.StatusModel = "Success";
                ModelState.AddModelError(string.Empty, "Marca criada com sucesso");
            }
            catch (Exception e)
            {
                marcaViewModel.StatusModel = "Error";
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View(marcaViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var marcaModel = await _marcaAppServices.GetByIdAsync(id);

            return View(marcaModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Nome")] MarcaViewModel marcaViewModel)
        {
            try
            {
                await _marcaAppServices.UpdateAsync(marcaViewModel);
                marcaViewModel.StatusModel = "Success";
                ModelState.AddModelError(string.Empty, "Marca alterada com sucesso");
            }
            catch (Exception e)
            {
                marcaViewModel.StatusModel = "Error";
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View(marcaViewModel);
        }


    }
}
