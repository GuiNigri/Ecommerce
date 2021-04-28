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
    public class EmpresaController:Controller
    {
        private readonly IEmpresaAppServices _empresaAppServices;

        public EmpresaController(IEmpresaAppServices empresaAppServices)
        {
            _empresaAppServices = empresaAppServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string termo, int pagina = 1)
        {
            IEnumerable<EmpresaViewModel> listaEmpresa;

            if (termo == null)
            {
                listaEmpresa = await _empresaAppServices.GetAllAsync();
            }
            else
            {
                listaEmpresa = await _empresaAppServices.FilterAsync(termo);
            }

            var pagedList = await listaEmpresa.ToList().ToPagedListAsync(pagina, 20);

            return View(new EmpresaListAdminViewModel(pagedList,termo));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var empresaViewModel = await _empresaAppServices.GetByIdAsync(id);
            
            return View(empresaViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmpresaViewModel empresaViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _empresaAppServices.UpdateAsync(empresaViewModel);

                    empresaViewModel.StatusModel = "Success";
                    ModelState.AddModelError(string.Empty,"Empresa alterada com sucesso");
                }
                catch (Exception e)
                {
                    empresaViewModel.StatusModel = "Error";
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            return View(empresaViewModel);
        }
    }

}
