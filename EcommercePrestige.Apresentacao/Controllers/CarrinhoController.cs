using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.Helpers;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EcommercePrestige.Apresentacao.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly IProdutoAppServices _produtoAppServices;
        private readonly IProdutoCorAppServices _produtoCorAppServices;
        private readonly ICarrinhoAppServices _carrinhoAppServices;
        private readonly IPedidoAppServices _pedidoAppServices;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmpresaAppServices _empresaAppServices;
        private readonly IKitAppServices _kitAppServices;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICorAppServices _corAppServices;
        private readonly IUsuarioAppServices _usuarioAppServices;

        public CarrinhoController(IProdutoAppServices produtoAppServices, IProdutoCorAppServices produtoCorAppServices,
            ICarrinhoAppServices carrinhoAppServices, IPedidoAppServices pedidoAppServices,
            UserManager<IdentityUser> userManager, IEmpresaAppServices empresaAppServices, IKitAppServices kitAppServices,
            SignInManager<IdentityUser> signInManager,ICorAppServices corAppServices, IUsuarioAppServices usuarioAppServices)
        {
            _produtoAppServices = produtoAppServices;
            _produtoCorAppServices = produtoCorAppServices;
            _carrinhoAppServices = carrinhoAppServices;
            _pedidoAppServices = pedidoAppServices;
            _userManager = userManager;
            _empresaAppServices = empresaAppServices;
            _kitAppServices = kitAppServices;
            _signInManager = signInManager;
            _corAppServices = corAppServices;
            _usuarioAppServices = usuarioAppServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var carrinho = SessionHelper.GetObjectFromJson<List<CarrinhoViewModel>>(HttpContext.Session, "cart");

            var carrinhoViewModel = new CarrinhoViewModel();

            if (carrinho != null)
            {

                carrinhoViewModel.CarrinhoViewModels = carrinho;

                var produtoCarrinho = carrinho.Any(x => x.Produto != null);

                double subTotal = 0;
                double descontoTotal = 0;

                if (produtoCarrinho)
                {
                    subTotal += carrinho.FindAll(x => x.Produto != null).Sum(x => double.Parse(x.Produto.ValorVenda) * x.QuantidadeIndividual);

                    descontoTotal = carrinho.Sum(x => x.DescontoUnitarioProduto);
                    
                }

                var kitsCarrinho = carrinho.Any(x => x.Kits != null);

                if (kitsCarrinho)
                {
                    subTotal += carrinho.FindAll(x => x.Kits != null).Sum(x => double.Parse(x.Kits.ValorVenda) * x.QuantidadeIndividual);
                }

                var userLogado = await _userManager.GetUserAsync(User);

                var usuario = await _usuarioAppServices.GetByUserIdAsync(userLogado.Id);

                if (usuario.DescontoCliente > 0)
                {
                    var teste = subTotal * (usuario.DescontoCliente / 100);
                    descontoTotal += teste;
                }

                var valorTotal = subTotal - descontoTotal;

                carrinhoViewModel.SubTotalPedido = subTotal.ToString("C");
                carrinhoViewModel.ValorTotalPedido = valorTotal.ToString("C");
                carrinhoViewModel.QuantidadeTotalItens = carrinho.Count();

                carrinhoViewModel.DescontoPedido = descontoTotal.ToString("C");

            }
            else
            {
                carrinhoViewModel.CarrinhoViewModels = new List<CarrinhoViewModel>();
            }

            carrinhoViewModel.StatusModel = "success";

            if (TempData["Error"] != null)
            {
                carrinhoViewModel.StatusModel = "Error";
                ModelState.AddModelError(string.Empty, TempData["Error"].ToString());
            }

            return View("Index", carrinhoViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Pagamento()
        {
            var carrinho = SessionHelper.GetObjectFromJson<List<CarrinhoViewModel>>(HttpContext.Session, "cart");

            if (carrinho == null) return RedirectToAction("Index", "Home");

            var user = await _userManager.GetUserAsync(User);

            var empresa = await _empresaAppServices.GetEmpresaByUserId(user.Id);
            var usuario = await _usuarioAppServices.GetByUserIdAsync(user.Id);

            var valorTotalPedido = ToValorTotal(carrinho, usuario.DescontoCliente);
            var fretes = await _carrinhoAppServices.GetFrete(empresa.Cep,carrinho.Sum(x=>x.QuantidadeIndividual), valorTotalPedido);

            TempData["frete"] = JsonConvert.SerializeObject(fretes);

            var statusModel = "success";

            if (TempData["Error"] != null)
            {
                statusModel = "Error";
                ModelState.AddModelError(string.Empty, TempData["Error"].ToString());
            }

            return View(new CarrinhoPagamentoViewModel(empresa, carrinho, valorTotalPedido, statusModel, fretes));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar(string corId, int prodId, int kitId, int quantNumber,string returnUrl = null)
        {
            string msg;

            var actionRetorno = "Details";
            var controllerRetorno = "Produtos";
            var idRetorno = prodId;

            if (corId == null)
            {
                TempData["Error"] = "Selecione uma cor.";
                return RedirectToAction(actionRetorno, controllerRetorno, new { id = idRetorno, idCor = 0, returnUrl });
            }

            var produtoCorId = int.Parse(corId.Split(",")[1].Trim());
            var cor = int.Parse(corId.Split(",")[0].Trim());

            if (prodId > 0)
            {
                if (!_signInManager.IsSignedIn(User))
                {
                    TempData["Error"] = "Necessário login para adicionar produtos ao carrinho";
                    return RedirectToAction(actionRetorno, controllerRetorno, new { id = idRetorno, idCor = produtoCorId, returnUrl });
                }

                if (cor <= 0)
                {
                    TempData["Error"] = "Selecione a cor da armação";

                    return RedirectToAction("Details", "Produtos", new { id = prodId, idCor = produtoCorId, returnUrl });
                }

                msg = await AddProduto(prodId, cor, quantNumber,produtoCorId);
            }
            else
            {
                actionRetorno = "Index";
                controllerRetorno = "Kits";
                idRetorno = kitId;

                if (!_signInManager.IsSignedIn(User))
                {
                    TempData["Error"] = "Necessário login para adicionar produtos ao carrinho";
                    return RedirectToAction(actionRetorno, controllerRetorno, new { id = idRetorno });
                }

                msg = await AddKit(kitId,quantNumber);


            }


            if (msg != null)
            {
                if (msg.StartsWith("Estoque"))
                {
                    TempData["Error"] = msg;
                    return RedirectToAction(actionRetorno, controllerRetorno, new { id = idRetorno, idCor = produtoCorId, returnUrl });
                }

                TempData["Success"] = msg;
                return RedirectToAction(actionRetorno, controllerRetorno, new { id = idRetorno, idCor = produtoCorId, returnUrl });
                
            }

            TempData["Error"] = "Erro ao adicionar produto ao carrinho";

            return RedirectToAction(actionRetorno, controllerRetorno, new {id = idRetorno, idCor = produtoCorId, returnUrl });

        }

        private async Task<string> AddKit(int kitId, int quantNumber)
        {
            var kitsModel = await _kitAppServices.GetByIdAsync(kitId);

            if (kitsModel != null && quantNumber > 0)
            {
                return await _carrinhoAppServices.AdicionarItemAoCarrinho(new ProdutoViewModel(), kitsModel, null,
                    quantNumber, 0);
            }

            return null;
        }

        private async Task<string> AddProduto(int prodId, int corId, int quantidade, int produtoCorId)
        {
            var produtoModel = await _produtoAppServices.GetProductByIdAndCor(prodId, corId, "AT");
            var corviewModel = await _corAppServices.GetByIdAsync(corId);

            if (produtoModel != null && quantidade > 0)
            {
                return await _carrinhoAppServices.AdicionarItemAoCarrinho(produtoModel, new KitsViewModel(), corviewModel, quantidade, produtoCorId);
            }

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> AtualizarKit(int kitId, int quantNumber)
        {
            var kitsModel = await _kitAppServices.GetByIdAsync(kitId);

            if (kitsModel != null && quantNumber > 0)
            {
                await _carrinhoAppServices.AtualizarCarrinho(new ProdutoViewModel(), kitsModel, 0, quantNumber);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Atualizar(int corId, int prodId, int quantNumber)
        {
            var produtoModel = await _produtoAppServices.GetProductByIdAndCor(prodId, corId, "AT");

            if (produtoModel != null && quantNumber > 0)
            {
                await _carrinhoAppServices.AtualizarCarrinho(produtoModel, new KitsViewModel(), corId, quantNumber);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Remover(int corId, int prodId)
        {
            await _carrinhoAppServices.RemoverItemDoCarrinho(prodId, corId);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<JsonResult> GetAddress(string cep)
        {
            return Json(await _carrinhoAppServices.GetAddress(cep));
        }

        [HttpGet]
        public async Task<JsonResult> GetParcelas(string formaPagamento, string valor)
        {
            var valorDouble = double.Parse(valor);
            return Json(await _carrinhoAppServices.GetParcelas(formaPagamento, valorDouble));
        }

        [HttpGet]
        public async Task<PartialViewResult> GetFrete(string cepDestino)
        {
            var carrinho = SessionHelper.GetObjectFromJson<List<CarrinhoViewModel>>(HttpContext.Session, "cart");


            var user = await _userManager.GetUserAsync(User);
            var usuario = await _usuarioAppServices.GetByUserIdAsync(user.Id);
            var valorTotalPedido = ToValorTotal(carrinho, usuario.DescontoCliente);

            var fretes = await _carrinhoAppServices.GetFrete(cepDestino, carrinho.Sum(x => x.QuantidadeIndividual), valorTotalPedido);

            TempData["frete"] = JsonConvert.SerializeObject(fretes);

            return PartialView("PagamentoFreteViewPartial",fretes);
        }

        private static double ToValorTotal(List<CarrinhoViewModel> carrinho, double descontoUsuario)
        {
            var produtoCarrinho = carrinho.Any(x => x.Produto != null);

            double valorTotal = 0;
            double descontoTotal = 0;

            if (produtoCarrinho)
            {
                var listaProdutos = carrinho.FindAll(x => x.Produto != null).ToList();
                valorTotal += listaProdutos.Sum(x => double.Parse(x.Produto.ValorVenda) * x.QuantidadeIndividual);
                descontoTotal = listaProdutos.Sum(x => x.DescontoUnitarioProduto);
            }

            var kitsCarrinho = carrinho.Any(x => x.Kits != null);

            if (kitsCarrinho)
            {
                valorTotal += carrinho.FindAll(x => x.Kits != null).Sum(x => double.Parse(x.Kits.ValorVenda) * x.QuantidadeIndividual);
            }

            if (descontoUsuario > 0)
            {
                var valorDescontoUsuario = valorTotal * (descontoUsuario / 100);

                descontoTotal += valorDescontoUsuario;
            }

            valorTotal -= descontoTotal;

            return valorTotal;
        }

    }
}
