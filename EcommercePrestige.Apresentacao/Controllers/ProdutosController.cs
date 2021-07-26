using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using EcommercePrestige.Model.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;



namespace EcommercePrestige.Apresentacao.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly IProdutoAppServices _produtoAppServices;
        private readonly IProdutoFotoAppServices _produtoFotoAppServices;
        private readonly IProdutoCorAppServices _produtoCorAppServices;
        private readonly IMarcaAppServices _marcaAppServices;
        private readonly IMaterialAppServices _materialAppServices;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICorAppServices _corAppServices;

        public ProdutosController(IProdutoAppServices produtoAppServices,
            IProdutoFotoAppServices produtoFotoAppServices, IProdutoCorAppServices produtoCorAppServices,
            IMarcaAppServices marcaAppServices, IMaterialAppServices materialAppServices,
            SignInManager<IdentityUser> signInManager, ICorAppServices corAppServices)
        {
            _produtoAppServices = produtoAppServices;
            _produtoFotoAppServices = produtoFotoAppServices;
            _produtoCorAppServices = produtoCorAppServices;
            _marcaAppServices = marcaAppServices;
            _materialAppServices = materialAppServices;
            _signInManager = signInManager;
            _corAppServices = corAppServices;
        }

        [HttpGet]
        public async Task<ActionResult> Index(List<ProdutoViewModel> produtoViewModel, FiltroProdutoViewModel filtro,
            int pagina = 1)
        {
            IEnumerable<ProdutoViewModel> produtos;

            if (produtoViewModel.Any())
            {
                produtos = produtoViewModel;
            }
            else
            {
                produtos = await _produtoAppServices.GetAllAsync("AT");
            }

            var statusModel = "Success";
            if (TempData["Error"] != null)
            {
                statusModel = "Error";
                ModelState.AddModelError(string.Empty, TempData["Error"].ToString());
            }

            var produtoViewModels = await produtos.ToList().ToPagedListAsync(pagina, 24);


            var produtoCorViewModel = await _produtoCorAppServices.GetAllGroupedAsync("AT");
            var marcaViewModel = await _marcaAppServices.GetAllAsync();
            var materialViewModel = await _materialAppServices.GetAllAsync();

            return View("Index",
                new ProdutoIndexViewModel(produtoViewModels, produtoCorViewModel, marcaViewModel, materialViewModel, filtro,statusModel));
        }
        [HttpGet]
        public async Task<IActionResult> Return(string returnUrl)
        {
            return Redirect(returnUrl);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id, int idCor, string returnUrl = null)
        {
            var produtoViewModel = await _produtoAppServices.GetByIdAsync(id);
            var produtoFotoViewModel = await _produtoFotoAppServices.GetFotosByCorAsync(idCor);
            var produtoCorViewModel = await _produtoCorAppServices.GetByProdutoAsync(id,"AT");

            var lancamento = await _produtoAppServices.GetCategoryForHomeAsync("lancamento", 0);

            var logado = _signInManager.IsSignedIn(User);

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

            returnUrl ??= Request.Headers["Referer"].ToString();

            return View(new ProdutoDetailsViewModel(produtoViewModel, produtoFotoViewModel, produtoCorViewModel, statusModel,logado, lancamento,idCor, returnUrl));
        }

        [HttpGet]
        public async Task<PartialViewResult> GetFotoByCor(int produtoId)
        {
            var fotos = await _produtoFotoAppServices.GetFotosByCorAsync(produtoId);

            return PartialView("ProdutoFotoViewPartial",fotos);
        }

        [HttpGet]
        public async Task<PartialViewResult> GetFotoByCorIndex(int produtoId)
        {
            var foto = await _produtoFotoAppServices.GetFotosByCorIndexAsync(produtoId);

            return PartialView("ProdutoFotoIndexViewPartial", foto.UriBlob);
        }

        [HttpGet]
        public async Task<ActionResult> FilterIndex(string generoOption, string marcaOption, int corOption, string materialOption,
            string orderType, string category, string termoOption)
        {
            var imgCor = "";
            var corViewModel = await _corAppServices.GetByIdAsync(corOption);

            if (corViewModel != null)
            {
                imgCor = corViewModel.ImgUrl;
            }
            var filtroProdutosViewModel =
                new FiltroProdutoViewModel(corOption, marcaOption, generoOption, orderType, materialOption ,category, imgCor, termoOption);

            var produtoViewModels = new List<ProdutoViewModel>();

            if (!CheckModelNull(filtroProdutosViewModel))
            {
                try
                {
                    IEnumerable<ProdutoViewModel> filtroList;

                    if (filtroProdutosViewModel.Category != null && filtroProdutosViewModel.Termo == null)
                    {
                        filtroList = await _produtoAppServices.GetCategoryAsync(filtroProdutosViewModel, "AT");
                    }
                    else if (filtroProdutosViewModel.Termo != null)
                    {
                        filtroList = await _produtoAppServices.GetFilterTermoAsync(filtroProdutosViewModel, "AT");
                    }
                    else
                    {
                        filtroList = await _produtoAppServices.FilterAndOrderAsync(filtroProdutosViewModel, "AT");
                    }


                    produtoViewModels = filtroList.ToList();

                    if (!produtoViewModels.Any())
                    {
                        TempData["Error"] = "Nenhum produto encontrado de acordo com o filtro";
                        produtoViewModels = new List<ProdutoViewModel>();
                        filtroProdutosViewModel = new FiltroProdutoViewModel();
                    }
                }
                catch (ModelValidationExceptions e)
                {
                    TempData["Error"] = e.Message;

                    produtoViewModels = new List<ProdutoViewModel>();
                    filtroProdutosViewModel = new FiltroProdutoViewModel();
                }
            }
            return await Index(produtoViewModels, filtroProdutosViewModel).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFilter(string category)
        {
            var filtros = new FiltroProdutoViewModel();

            IEnumerable<ProdutoViewModel> categoryList = new List<ProdutoViewModel>();

            if (category != null)
            {
                filtros = new FiltroProdutoViewModel(category);
                categoryList = await _produtoAppServices.GetCategoryAsync(filtros, "AT");
            }

            return await Index(await categoryList.ToListAsync(), filtros).ConfigureAwait(false);

        }

        [HttpGet]
        public async Task<IActionResult> Page(int corOrder, string generoOrder, string marcaOrder, string orderType, string materialOrder,
            string category, int pagina, string termoOrder)
        {
            var imgCor = "";
            var corViewModel = await _corAppServices.GetByIdAsync(corOrder);

            if (corViewModel != null)
            {
                imgCor = corViewModel.ImgUrl;
            }

            var filtros = new FiltroProdutoViewModel(corOrder, marcaOrder, generoOrder, orderType, materialOrder, category, imgCor, termoOrder);

            var listaOrdenada = await _produtoAppServices.FilterAndOrderAsync(filtros,"AT");

            return await Index(listaOrdenada.ToList(), filtros, pagina).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategory(string category, string marca, string gender, string material)
        {
            var filtros = new FiltroProdutoViewModel(marca,gender,category,material);

            var categoryList = await _produtoAppServices.GetCategoryAsync(filtros, "AT");

            var list = await categoryList.ToListAsync();

            if (list.Count > 0) return await Index(list, filtros).ConfigureAwait(false);

            TempData["Error"] = "Nenhum produto encontrado na categoria selecionada";
            filtros = new FiltroProdutoViewModel();

            return await Index(list, filtros).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<bool> CadastrarAviseMe(string corId, string email)
        {
            try
            {
                if (email == null) return false;

                await _produtoAppServices.CadastrarAviseMeAsync(int.Parse(corId), email);
                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }

        private static bool CheckModelNull(FiltroProdutoViewModel filtroProdutosViewModel)
        {
            if (filtroProdutosViewModel.CorOption <= 0 &&
                filtroProdutosViewModel.MarcaOption == null &&
                filtroProdutosViewModel.GeneroOption == null &&
                filtroProdutosViewModel.OrderType == null &&
                filtroProdutosViewModel.Category == null &&
                filtroProdutosViewModel.MaterialOption == null &&
                filtroProdutosViewModel.Termo == null)
            {
                return true;
            }

            return false;
        }
    }
}
