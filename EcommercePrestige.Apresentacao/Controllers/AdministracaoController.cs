using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EcommercePrestige.Apresentacao.Controllers
{
    [Authorize(policy: "Admin")]
    public class AdministracaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
