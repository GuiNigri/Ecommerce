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
    public class SuporteController : Controller
    {
        private readonly ISuporteAppServices _suporteAppServices;

        public SuporteController(ISuporteAppServices suporteAppServices)
        {
            _suporteAppServices = suporteAppServices;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(new SuporteInputModel());
        }

        [HttpGet]
        public async Task<IActionResult> Politicas()
        {
            return View();
        }

    }
}
