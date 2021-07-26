using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.Helpers;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using X.PagedList;

namespace EcommercePrestige.Apresentacao.Controllers
{
    public class PedidoController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPedidoAppServices _pedidoAppServices;
        private readonly IUsuarioAppServices _usuarioAppServices;
        private readonly IProdutoCorAppServices _produtoCorAppServices;
        private readonly IProdutoAppServices _produtoAppServices;
        private readonly IKitAppServices _kitAppServices;
        private readonly ICorAppServices _corAppServices;
        private readonly IEmpresaAppServices _empresaAppServices;

        public PedidoController(UserManager<IdentityUser> userManager, IPedidoAppServices pedidoAppServices,
            IUsuarioAppServices usuarioAppServices,IProdutoCorAppServices produtoCorAppServices, IProdutoAppServices produtoAppServices,
            IKitAppServices kitAppServices, ICorAppServices corAppServices, IEmpresaAppServices empresaAppServices)
        {
            _userManager = userManager;
            _pedidoAppServices = pedidoAppServices;
            _usuarioAppServices = usuarioAppServices;
            _produtoCorAppServices = produtoCorAppServices;
            _produtoAppServices = produtoAppServices;
            _kitAppServices = kitAppServices;
            _corAppServices = corAppServices;
            _empresaAppServices = empresaAppServices;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Success()
        {
            if (TempData["Pedido"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new PedidoSuccessViewModel(TempData["pedido"].ToString()));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalizar([Bind("FormaPagamento,Parcelas,ObsPagamento,Cep,Rua,Numero,Complemento,Bairro,Cidade,Estado")] CarrinhoPagamentoViewModel carrinhoPagamentoViewModel, string transactionData, string servicoFrete)
        {
            bool statusPedido;
            string nomeCompleto;
            string erro;
            int idPedido;

            var carrinho = SessionHelper.GetObjectFromJson<List<CarrinhoViewModel>>(HttpContext.Session, "cart");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Todos os campos são obrigatórios";

                return RedirectToAction("Pagamento", "Carrinho");
            }

            if (carrinho == null)
            {
                TempData["Error"] = "O carrinho está vazio.";

                return RedirectToAction("Pagamento", "Carrinho");
            }

            if (TempData["frete"].ToString() != "gratis" && servicoFrete == "proprio")
            {
                TempData["Error"] = "Selecione o frete";

                return RedirectToAction("Pagamento", "Carrinho");
            }


            var user = await _userManager.GetUserAsync(User);

            try
            {
                var freteJson =
                    JsonConvert.DeserializeObject<IEnumerable<CorreioWebServiceViewModel>>(TempData["frete"]
                        .ToString());

                var dadosEnvio = freteJson.First(x => x.Servico == servicoFrete);

                var (status, nome, mensagemDoErro, pedido) = await _pedidoAppServices.FinalizarPedido(carrinhoPagamentoViewModel, carrinho, user.Id, transactionData, dadosEnvio);

                statusPedido = status;
                nomeCompleto = nome;
                erro = mensagemDoErro;
                idPedido = pedido;

            }
            catch (Exception)
            {
                TempData["Error"] = $"Ocorreu um erro ao finalizar o pedido. Caso o erro persista, entre em contato pelo 'Fale Conosco'";
                return RedirectToAction("Pagamento","Carrinho");
            }

            if (statusPedido)
            {
                await _pedidoAppServices.EnviarConfirmacaoPedidoEmail(idPedido, nomeCompleto, user.Email);
                carrinho.Clear();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", carrinho);
                TempData["Pedido"] = idPedido.ToString();
                return RedirectToAction("Success");
            }

            TempData["Error"] = erro switch
            {
                "estoque" => "Um ou mais itens do pedido estão com estoque insuficiente",
                "pagamento" => "01 - Ocorreu um erro ao processar o pagamento.",
                "parcela" => "02 - Ocorreu um erro ao processar o pagamento.",
                _ => TempData["Error"]
            };

            return RedirectToAction("Pagamento", "Carrinho");
        }

        //administração

        [HttpGet]
        [Authorize(policy:"Admin")]
        public async Task<IActionResult> List(string termo, int pagina =1)
        {
            IEnumerable<PedidoAdmViewModel> listaPedidos;

            if (termo==null)
            {
                listaPedidos = await _pedidoAppServices.GetAllAdmAsync();
            }
            else
            {
                listaPedidos = await _pedidoAppServices.FilterAsync(termo, -1);
            }

            var pagedList = await listaPedidos.OrderByDescending(x=>x.Id).ToList().ToPagedListAsync(pagina, 20);

            return View("List",new PedidoListAdminViewModel(pagedList,termo));
        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> ListAprovados(string termo, int pagina = 1)
        {
            IEnumerable<PedidoAdmViewModel> listaPedidos;

            if (termo == null)
            {
                listaPedidos = await _pedidoAppServices.GetApprovedAdmAsync();
            }
            else
            {
                listaPedidos = await _pedidoAppServices.FilterAsync(termo, 1);
            }

            var pagedList = await listaPedidos.OrderByDescending(x => x.Id).ToList().ToPagedListAsync(pagina, 20);

            return View("List",new PedidoListAdminViewModel(pagedList, termo));
        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> ListReprovados(string termo, int pagina = 1)
        {
            IEnumerable<PedidoAdmViewModel> listaPedidos;

            if (termo == null)
            {
                listaPedidos = await _pedidoAppServices.GetReprovedAdmAsync();
            }
            else
            {
                listaPedidos = await _pedidoAppServices.FilterAsync(termo, 2);
            }

            var pagedList = await listaPedidos.OrderByDescending(x => x.Id).ToList().ToPagedListAsync(pagina, 20);

            return View("List", new PedidoListAdminViewModel(pagedList, termo));
        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> ListAguardando(string termo, int pagina = 1)
        {
            IEnumerable<PedidoAdmViewModel> listaPedidos;

            if (termo == null)
            {
                listaPedidos = await _pedidoAppServices.GetWaitingAdmAsync();
            }
            else
            {
                listaPedidos = await _pedidoAppServices.FilterAsync(termo, 0);
            }

            var pagedList = await listaPedidos.OrderByDescending(x => x.Id).ToList().ToPagedListAsync(pagina, 20);

            return View("List", new PedidoListAdminViewModel(pagedList, termo));
        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> Edit(int id, int pagina = 1)
        {
            var pedidoAdmViewModel = await _pedidoAppServices.GetPedidoAdm(id);

            var usuarioViewModel = await _usuarioAppServices.GetByIdAsync(pedidoAdmViewModel.UsuarioId);

            var empresaViewModel = await _empresaAppServices.GetEmpresaByUserId(usuarioViewModel.UserId);

            pedidoAdmViewModel.CNPJ = empresaViewModel.Cnpj;
            pedidoAdmViewModel.RazaoSocial = empresaViewModel.RazaoSocial;
            pedidoAdmViewModel.NomeOtica = empresaViewModel.NomeOtica;

            var produtos = await _pedidoAppServices.GetProdutosByPedido(id);

            pedidoAdmViewModel.ProdutosPedido = await produtos.OrderBy(x=>x.Produto.Referencia).ToList().ToPagedListAsync(pagina, 20);

            var referencias = await _produtoAppServices.GetAllAsync(null);
            var kits = await _kitAppServices.GetAllAsync();

            var referenciaSelect = referencias.Select(item => new SelectListItem {Value = $"{item.Id},produto", Text = item.Referencia}).ToList();

            referenciaSelect.AddRange(kits.Select(item => new SelectListItem {Value = $"{item.Id},kit", Text = $"Kit {item.Nome}"}));

            pedidoAdmViewModel.Referencias = referenciaSelect;

            var cores = await _corAppServices.GetAllAsync();

            pedidoAdmViewModel.CoresSelect = await cores
                .Select(item => new SelectListItem {Value = $"{item.Id}", Text = item.Descricao}).ToListAsync();

            if (TempData["ErrorModel"] != null)
            {
                pedidoAdmViewModel.StatusModel = "Error";
                ModelState.AddModelError(string.Empty, TempData["ErrorModel"].ToString());
            }
            if (TempData["Success"] != null)
            {
                pedidoAdmViewModel.StatusModel = "Success";
                ModelState.AddModelError(string.Empty, TempData["Success"].ToString());
            }

            return View("Edit",pedidoAdmViewModel);
        }

        [HttpPost]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> Edit(PedidoAdmViewModel pedidoAdmViewModel, string newRastreio, string newTipoEnvio)
        {
            try
            {
                var usuario = await _usuarioAppServices.GetByIdAsync(pedidoAdmViewModel.UsuarioId);
                var user = await _userManager.FindByIdAsync(usuario.UserId);

                await _pedidoAppServices.UpdateAsync(pedidoAdmViewModel, newRastreio, newTipoEnvio);

                if ((pedidoAdmViewModel.Rastreio == null && newRastreio != null)  || (newTipoEnvio == "proprio" && pedidoAdmViewModel.TipoDeEnvio == null))
                {
                    var anexoBytes = await GerarAnexoPDF(pedidoAdmViewModel.Id);

                    if (anexoBytes == null)
                    {
                        TempData["ErrorModel"] = "Erro ao gerar o PDF do pedido, o email não foi enviado ao cliente.";
                        return RedirectToAction("Edit", new { id = pedidoAdmViewModel.Id });
                    }

                    await _pedidoAppServices.EnviarEmailPedidoDespachado(user.Email, pedidoAdmViewModel.Id, newRastreio,
                        usuario.NomeCompleto, anexoBytes);
                }


                
                TempData["Success"] = "Atualizado com sucesso";
            }
            catch (Exception)
            {
                TempData["ErrorModel"] = "Erro ao atualizar pedido";
            }

            return RedirectToAction("Edit", new {id = pedidoAdmViewModel.Id});

        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> AprovarPedido(int idPedido, int idUsuario)
        {
            try
            {
                var usuario = await _usuarioAppServices.GetByIdAsync(idUsuario);
                var user = await _userManager.FindByIdAsync(usuario.UserId);

                await _pedidoAppServices.AprovarPedidoAsync(idPedido,usuario.NomeCompleto,user.Email);
                TempData["Success"] = "Aprovado com sucesso";
            }
            catch (Exception)
            {
                TempData["ErrorModel"] = "Erro ao aprovar pedido";
            }

            return RedirectToAction("Edit", new { id = idPedido });
        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> ReprovarPedido(int idPedido, int idUsuario)
        {
            try
            {
                var usuario = await _usuarioAppServices.GetByIdAsync(idUsuario);
                var user = await _userManager.FindByIdAsync(usuario.UserId);

                await _pedidoAppServices.ReprovarPedidoAsync(idPedido,usuario.NomeCompleto,user.Email);
                TempData["Success"] = "Reprovado com sucesso";
            }
            catch (Exception)
            {
                TempData["ErrorModel"] = "Erro ao reprovar pedido";
            }

            return RedirectToAction("Edit", new { id = idPedido });
        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> CancelarPedido(int idPedido, int idUsuario)
        {
            try
            {
                var usuario = await _usuarioAppServices.GetByIdAsync(idUsuario);
                var user = await _userManager.FindByIdAsync(usuario.UserId);

                await _pedidoAppServices.CancelarPedidoAsync(idPedido, usuario.NomeCompleto, user.Email);
                TempData["Success"] = "Cancelado com sucesso";
            }
            catch (Exception)
            {
                TempData["ErrorModel"] = "Erro ao cancelar pedido";
            }

            return RedirectToAction("Edit", new { id = idPedido });
        }

        [HttpPost]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> AtualizarQuantidade(int idProduto,int idEdit,int idUsuario, int quantNumber)
        {
            try
            {
                var usuario = await _usuarioAppServices.GetByIdAsync(idUsuario);
                var user = await _userManager.FindByIdAsync(usuario.UserId);

                await _pedidoAppServices.AlterarProdutosPedidoAsync(usuario, user.Email, idProduto, quantNumber);
                TempData["Success"] = "Atualizado com sucesso";
            }
            catch (Exception)
            {
                TempData["ErrorModel"] = "Erro ao atualizar pedido";
            }

            return RedirectToAction("Edit", new { id = idEdit });
        }

        [HttpPost]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> AdicionarItemAoPedido(int pedido, string idProduto, int corId,int quantidade)
        {
            try
            {
                await _pedidoAppServices.AdicionarItemAoPedidoAsync(pedido, idProduto, corId, quantidade);
                TempData["Success"] = "Item adicionado ao pedido com sucesso";
            }
            catch (Exception)
            {
                TempData["Error"] = "Erro ao adicionar item ao pedido";
            }

            return RedirectToAction("Edit", new { id = pedido });
        }

        [HttpGet]
        [Authorize(policy: "Admin")]
        public async Task<IActionResult> RemoverItemDoPedido(string tipoOp, int id, int pedido)
        {
            try
            {
                await _pedidoAppServices.RemoverItemDoPedido(tipoOp, id);
                TempData["Success"] = "Item removido do pedido com sucesso";
            }
            catch (Exception)
            {
                TempData["Error"] = "Erro ao remover item do pedido";
            }

            return RedirectToAction("Edit", new { id = pedido });
        }



        [HttpPost]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult> GetCorProduto(string idProduto)
        {
            var identificacao = idProduto.Split(',')[1].Trim();

            var id = idProduto.Split(',')[0].Trim();

            if (identificacao != "produto") return Content(string.Join("", ""));

            try
            {
                var produtoCor = await _produtoCorAppServices.GetByProdutoAsync(int.Parse(id), null);

                var result = (from t in produtoCor
                    select new
                    {
                        Cores = $"<option value= {t.Id}> {t.Descricao} </option>"

                    }).ToList();

                return Content(string.Join("", result));
            }
            catch (Exception)
            {
                TempData["ErrorModel"] = "Erro ao atualizar pedido";
            }

            return Content(string.Join("", ""));
        }

        public async Task<IActionResult> VerificarPedidoConcluido()
        {
            await _pedidoAppServices.VerificarSePedidoEntregue();

            return await List(null);
        }

        [Authorize]
        public async Task<IActionResult> Impressao(int pedido)
        {
            try
            {
                var pedidoViewModel = await _pedidoAppServices.GetPedido(pedido);

                var userIdPedido = pedidoViewModel.userId;

                var user = await _userManager.GetUserAsync(User);

                if (userIdPedido == user.Id)
                {

                    return await _pedidoAppServices.GerarPedidoPdf(pedidoViewModel,user.Email);
                }
                
                var impressaoViewModel = new ImpressaoPedidoViewModel
                {
                    StatusModel = "Error",
                    PedidoViewModel = new PedidoDetailsViewModel(),
                    ProdutosPedidoViewModel = new List<ProdutosPedidoViewModel>()
                };

                ModelState.AddModelError(string.Empty, "Pedido não pertence ao usuário logado.");


                return View(impressaoViewModel);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Authorize(policy: "Admin")]
        public async Task<IActionResult> ImpressaoADM(int pedido)
        {
            try
            {
                var pedidoViewModel = await _pedidoAppServices.GetPedido(pedido);

                var user = await _userManager.FindByIdAsync(pedidoViewModel.userId);

                return await _pedidoAppServices.GerarPedidoPdf(pedidoViewModel, user.Email);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Authorize(policy: "Admin")]
        public async Task<IActionResult> EnviarAlteracaoEmail(int pedido)
        {
            try
            {
                var pedidoViewModel = await _pedidoAppServices.GetPedido(pedido);

                var user = await _userManager.FindByIdAsync(pedidoViewModel.userId);

                 await _pedidoAppServices.EnviarEmailAlteracaoPedido(pedidoViewModel,user.Email);

                 TempData["Success"] = "Email enviado com sucesso.";

            }
            catch (Exception)
            {
                TempData["Error"] = "Errro ao enviar email.";
            }

            return RedirectToAction("Edit", new { id = pedido });
        }

        [Authorize(policy: "Admin")]
        public async Task<IActionResult> ConcluirPedido(int pedido, string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                await _pedidoAppServices.ConcluirPedido(pedido, user.Email);

                TempData["Success"] = "Pedido concluido com sucesso";
            }
            catch (Exception)
            {
                TempData["Error"] = "Errro ao concluir pedido.";
            }

            return RedirectToAction("Edit", new { id = pedido });
        }

        [Authorize(policy: "Admin")]
        private async Task<byte[]> GerarAnexoPDF(int numeroPedido)
        {
            try
            {
                var pedido = await _pedidoAppServices.GetPedido(numeroPedido);

                var user = await _userManager.FindByIdAsync(pedido.userId);

                var empresa = await _empresaAppServices.GetEmpresaByUserId(pedido.userId);

                var usuario = await _usuarioAppServices.GetByUserIdAsync(pedido.userId);

                var produtos = await _pedidoAppServices.GetProdutosByPedido(pedido.Id);

                var quantidadeArmacoes = produtos.Sum(x => x.Quantidade);

                var pdfImpressaoModel = new ImpressaoPedidoViewModel(pedido, produtos.OrderBy(x=>x.Produto.Referencia), user.Email, empresa, usuario.NomeCompleto, quantidadeArmacoes);

                var pdf = new ViewAsPdf
                {
                    Model = pdfImpressaoModel,
                    ViewName = "Impressao",
                    FileName = "teste"
                };

                return await pdf.BuildFile(ControllerContext);
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
