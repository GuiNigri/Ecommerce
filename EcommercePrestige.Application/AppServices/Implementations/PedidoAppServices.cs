using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using EcommercePrestige.Application.AutoMapper;
using EcommercePrestige.Application.ViewModel;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Services;
using EcommercePrestige.Model.Interfaces.UoW;
using Newtonsoft.Json;
using Rotativa.AspNetCore;


namespace EcommercePrestige.Application.AppServices.Implementations
{
    public class PedidoAppServices:IPedidoAppServices
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPedidoService _pedidoService;
        private readonly IProdutoCorServices _produtoCorServices;
        private readonly IProdutoFotoServices _produtoFotoServices;
        private readonly IEmpresaAppServices _empresaAppServices;
        private readonly IMapper _mapper;

        public PedidoAppServices(IUsuarioServices usuarioServices, IUnitOfWork unitOfWork, IPedidoService pedidoService, IProdutoCorServices produtoCorServices, IProdutoFotoServices produtoFotoServices, IEmpresaAppServices empresaAppServices, ICarrinhoAppServices carrinhoAppServices)
        {
            _usuarioServices = usuarioServices;
            _unitOfWork = unitOfWork;
            _pedidoService = pedidoService;
            _produtoCorServices = produtoCorServices;
            _produtoFotoServices = produtoFotoServices;
            _empresaAppServices = empresaAppServices;
            _mapper = AutoMapperConfig.Mapper;
        }

        public async Task<IEnumerable<PedidoViewModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<PedidoViewModel>>(await _pedidoService.GetAllAsync());
        }

        public async Task<IEnumerable<PedidoAdmViewModel>> GetAllAdmAsync()
        {
            return await ToPedidoAdmViewModel(await _pedidoService.GetAllAsync());
        }

        private async Task<IEnumerable<PedidoAdmViewModel>> ToPedidoAdmViewModel(IEnumerable<PedidoModel> listaPedidosModel)
        {
            var listaPedidos = new List<PedidoAdmViewModel>();

            foreach (var pedidoModel in listaPedidosModel)
            {
                var empresa = await _empresaAppServices.GetEmpresaByUserId(pedidoModel.UsuarioModel.UserId);

                var pedidoAdmViewModel = _mapper.Map<PedidoAdmViewModel>(pedidoModel);

                pedidoAdmViewModel.RazaoSocial = empresa.RazaoSocial;

                listaPedidos.Add(pedidoAdmViewModel);
            }

            return listaPedidos;
        }

        public async Task CancelarPedidoAsync(int id, string nomeUsuario, string email)
        {
            _unitOfWork.BeginTransaction();
            await _pedidoService.CancelarPedidoAsync(id, nomeUsuario, email);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<PedidoAdmViewModel>> GetApprovedAdmAsync()
        {
            return await ToPedidoAdmViewModel(await _pedidoService.GetApprovedAsync());
        }
        public async Task<IEnumerable<PedidoAdmViewModel>> GetReprovedAdmAsync()
        {
            return await ToPedidoAdmViewModel(await _pedidoService.GetReprovedAsync());
        }
        public async Task<IEnumerable<PedidoAdmViewModel>> GetWaitingAdmAsync()
        {
            return await ToPedidoAdmViewModel(await _pedidoService.GetWaitingAsync());
        }

        public async Task<(bool,string,string,int)> FinalizarPedido(CarrinhoPagamentoViewModel carrinhoPagamentoViewModel, IEnumerable<CarrinhoViewModel> carrinho, string userId, string transactionData, CorreioWebServiceViewModel dadosEnvio)
        {
            try
            {
                var carrinhoViewModels = carrinho.ToList();

                if (!await VerificarEstoqueFinalizacao(carrinhoViewModels))
                {
                    return (false, null,"estoque",0);
                }

                var usuario = await _usuarioServices.GetByUserIdAsync(userId);

                var produtoCarrinho = carrinhoViewModels.Any(x => x.Produto != null);

                double valorTotal = 0;
                double descontoTotal = 0;

                var listaProdutos = new List<CarrinhoViewModel>();
                var listaKits = new List<CarrinhoViewModel>();

                if (produtoCarrinho)
                {
                    listaProdutos = carrinhoViewModels.FindAll(x => x.Produto != null);
                    
                    valorTotal += listaProdutos.Sum(x => double.Parse(x.Produto.ValorVenda) * x.QuantidadeIndividual);
                    descontoTotal += listaProdutos.Sum(x => x.DescontoUnitarioProduto);
                }

                var kitsCarrinho = carrinhoViewModels.Any(x => x.Kits != null);

                if (kitsCarrinho)
                {
                    listaKits = carrinhoViewModels.FindAll(x => x.Kits != null);
                    valorTotal += listaKits.Sum(x => double.Parse(x.Kits.ValorVenda) * x.QuantidadeIndividual);
                }

                if (usuario.DescontoCliente > 0)
                {
                    var descontoCliente = valorTotal * (usuario.DescontoCliente / 100);
                    descontoTotal += descontoCliente;
                }


                var pedidoModel = ToPedidoModel(carrinhoPagamentoViewModel, usuario.Id, valorTotal, descontoTotal, dadosEnvio);

                if (transactionData != null)
                {
                    var pagarMeModel = JsonConvert.DeserializeObject<PagarMeModel>(transactionData);

                    if(pagarMeModel.Installments == null)
                    {
                        pagarMeModel.SetInstallments("1");
                    }

                    if (!await CheckInstallments(pedidoModel.FormaDePagamento, pedidoModel.ValorTotal,int.Parse(pagarMeModel.Installments)))
                    {
                        return (false, null, "parcela", 0);
                    }

                    var orderProducts = await ToPedidoProdutoModel(listaProdutos, 0);

                    var pagarMeReturnModel = await _pedidoService.CreatePagarMeTransaction(pagarMeModel, pedidoModel, orderProducts, userId);

                    if(pagarMeReturnModel.TransactionStatus != "paid" && pagarMeReturnModel.TransactionStatus != "authorized")
                    {
                        return (false, null, "pagamento",0);
                    }

                    pedidoModel.SetTransactionStatus(pagarMeReturnModel.TransactionStatus);
                    pedidoModel.SetTid(pagarMeReturnModel.Tid);
                    pedidoModel.SetAuthorizationCode(pagarMeReturnModel.AuthorizationCode);
                    pedidoModel.SetGateway("Stone");
                }

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                var idPedido = await _pedidoService.CreateReturningIdAsync(pedidoModel);

                var produtosDoPedido = new List<PedidoProdutosModel>();

                if (listaProdutos.Any())
                {
                    produtosDoPedido = await ToPedidoProdutoModel(listaProdutos, idPedido);
                    await _pedidoService.CreateProdutosAsync(produtosDoPedido,new List<PedidoKitModel>());
                }

                if (listaKits.Any())
                {
                    var kitsDoPedido = await ToPedidoKitModel(listaKits, idPedido);
                    await _pedidoService.CreateProdutosAsync(new List<PedidoProdutosModel>(), kitsDoPedido);
                }

                scope.Complete();

                return (true, usuario.NomeCompleto,null,idPedido);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private async Task<bool> CheckInstallments(string payment, double amount, int installment)
        {
            var maxInstallment = 1;

            if (payment == "boleto" || payment == "cartao" || payment == "cheque")
            {
                if (amount >= 700)
                {
                    for (var i = 1; i < 7; i++)
                    {
                        if (amount / i >= 350)
                        {
                            maxInstallment = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

            }

            if (installment <= maxInstallment)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task EnviarConfirmacaoPedidoEmail(int idPedido, string nomeCompleto, string userEmail)
        {
            await _pedidoService.EnviarConfirmacaoPedidoEmail(idPedido, nomeCompleto, userEmail);
        }

        public async Task<IEnumerable<PedidoViewModel>> GetByUsuario(int id)
        {
            return _mapper.Map<IEnumerable<PedidoViewModel>>(await _pedidoService.GetByUsuario(id));
        }

        public async Task<IEnumerable<ProdutosPedidoViewModel>> GetProdutosByPedido(int pedido)
        {
            var produtos =  await _pedidoService.GetProdutosByPedido(pedido);
            var kits = await _pedidoService.GetKitsByPedido(pedido);

            var lista = new List<ProdutosPedidoViewModel>();

            foreach (var produto in produtos)
            {
                var produtoViewModel = _mapper.Map<ProdutoViewModel>(produto.ProdutoCorModel.ProdutoModel);
                var produtoFoto = await _produtoFotoServices.GetFotosbyCorAsync(produto.ProdutoCorModel.Id);
                produtoViewModel.UriFoto = produtoFoto.FirstOrDefault()?.UriBlob;

                var carrinho = new ProdutosPedidoViewModel
                {
                    Id = produto.Id,
                    Quantidade = produto.Quantidade,
                    Kits = null,
                    Cor = produto.ProdutoCorModel.CorModel.ImgUrl,
                    CI = produto.ProdutoCorModel.CodigoInterno,
                    DescricaoCor = produto.ProdutoCorModel.CorModel.Descricao,
                    Produto = produtoViewModel,
                    ValorUnitario = produto.ValorUnitario.ToString("C"),
                    ValorTotal = produto.ValorTotal.ToString("C")
                };

                lista.Add(carrinho);
            }

            foreach (var kit in kits)
            {
                var kitsViewModel = _mapper.Map<KitsViewModel>(kit.KitModel);

                var carrinho = new ProdutosPedidoViewModel
                {
                    Id = kit.Id,
                    Quantidade = kit.Quantidade,
                    Kits = kitsViewModel,
                    Cor = "N/A",
                    Produto = null,
                    ValorUnitario = kit.ValorUnitario.ToString("C"),
                    ValorTotal = kit.ValorTotal.ToString("C")
                };

                lista.Add(carrinho);
            }

            return lista;
        }

        public async Task<PedidoDetailsViewModel> GetPedido(int pedido)
        {
            return _mapper.Map<PedidoDetailsViewModel>(await _pedidoService.GetPedido(pedido));
        }

        public async Task<PedidoAdmViewModel> GetPedidoAdm(int pedido)
        {
            return _mapper.Map<PedidoAdmViewModel>(await _pedidoService.GetPedido(pedido));
        }

        public async Task<bool> CheckPedidoUsuario(int usuarioId, int pedido)
        {
            return await _pedidoService.CheckPedidoUsuario(usuarioId, pedido);
        }

        public async Task<bool> CheckIfExistOrderByUserAsync(string userId)
        {
            return await _pedidoService.CheckIfExistOrderByUserAsync(userId);
        }
        private async Task<List<PedidoProdutosModel>> ToPedidoProdutoModel(IEnumerable<CarrinhoViewModel> carrinhoViewModels, int idPedido)
        {
            var produtosDoPedido = new List<PedidoProdutosModel>();

            foreach (var item in carrinhoViewModels)
            {
                var cor = await _produtoCorServices.GetCorByProdutoAsync(item.Cor.Id, item.Produto.Id, "AT");

                var pedidoProdutoModel = new PedidoProdutosModel(idPedido,
                    cor.Id,
                    item.QuantidadeIndividual,
                    double.Parse(item.Produto.ValorVenda),
                    item.QuantidadeIndividual * double.Parse(item.Produto.ValorVenda));

                produtosDoPedido.Add(pedidoProdutoModel);
            }

            return produtosDoPedido;
        }

        private async Task<IEnumerable<PedidoKitModel>> ToPedidoKitModel(IEnumerable<CarrinhoViewModel> carrinhoViewModels, int idPedido)
        {
            return carrinhoViewModels.Select(item =>
                new PedidoKitModel(idPedido,
                item.Kits.Id,
                item.QuantidadeIndividual,
                double.Parse(item.Kits.ValorVenda),
                item.QuantidadeIndividual * double.Parse(item.Kits.ValorVenda))).ToList();
        }

        private static PedidoModel ToPedidoModel(CarrinhoPagamentoViewModel carrinhoPagamentoViewModel, int usuarioId, double subTotal, double descontoTotal, CorreioWebServiceViewModel dadosEnvio)
        {
            var pedidoModel = new PedidoModel(
                usuarioId,
                carrinhoPagamentoViewModel.FormaPagamento,
                carrinhoPagamentoViewModel.Parcelas,
                subTotal,
                descontoTotal,
                (subTotal + double.Parse(dadosEnvio.Valor)) - descontoTotal,
                double.Parse(dadosEnvio.Valor),
                dadosEnvio.Servico,
                carrinhoPagamentoViewModel.Cep,
                carrinhoPagamentoViewModel.Rua,
                carrinhoPagamentoViewModel.Numero,
                carrinhoPagamentoViewModel.Complemento,
                carrinhoPagamentoViewModel.Bairro,
                carrinhoPagamentoViewModel.Cidade,
                carrinhoPagamentoViewModel.Estado,
                TimeZoneInfo.ConvertTime(DateTime.Now,
                    TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")),
                carrinhoPagamentoViewModel.ObsPagamento,
                0
            );

            return pedidoModel;
        }

        private async Task<bool> VerificarEstoqueFinalizacao(IEnumerable<CarrinhoViewModel> carrinho)
        {
            var cart = carrinho.ToList();

            var kit = cart.FindAll(x => x.Kits != null);

            var lista = new List<ArmacoesViewModel>();

            foreach (var k in kit)
            {
                var (armacoesDoKit, status) = await _produtoCorServices.GetListByKitAsync(k.Kits.Nome,"AT");

                foreach (var armacao in armacoesDoKit)
                {
                    if (lista.Any())
                    {
                        var index = IsExist(armacao.ProdutoModelId, armacao.CorModelId, lista);

                        if (index != -1)
                        {
                            lista[index].Quantidade += k.QuantidadeIndividual;
                        }
                        else
                        {
                            lista.Add(new ArmacoesViewModel(armacao.ProdutoModelId, armacao.CorModelId, k.QuantidadeIndividual));
                        }
                    }
                    else
                    {
                        lista.Add(new ArmacoesViewModel(armacao.ProdutoModelId, armacao.CorModelId, k.QuantidadeIndividual));
                    }

                }
            }

            var produtos = cart.FindAll(x => x.Produto != null);

            foreach (var p in produtos)
            {
                if (lista.Any())
                {
                    var index = IsExist(p.Produto.Id, p.Cor.Id,lista);

                    if (index != -1)
                    {
                        lista[index].Quantidade += p.QuantidadeIndividual;
                    }
                    else
                    {
                        lista.Add(new ArmacoesViewModel(p.Produto.Id, p.Cor.Id, p.QuantidadeIndividual));
                    }
                }
                else
                {
                    lista.Add(new ArmacoesViewModel(p.Produto.Id, p.Cor.Id, p.QuantidadeIndividual));
                }
            }

            foreach (var item in lista)
            {
                if (!await _produtoCorServices.VerificarEstoque(item.Armacao, item.Cor, item.Quantidade, "AT")) return false;
            }

            return true;
        }

        private static int IsExist(int id, int corId, IReadOnlyList<ArmacoesViewModel> cart)
        {

            for (var i = 0; i < cart.Count; i++)
            {

                if (cart[i].Armacao.Equals(id) && cart[i].Cor.Equals(corId))
                {
                    return i;
                }

            }

            return -1;
        }

        public async Task<IEnumerable<PedidoAdmViewModel>> FilterAsync(string termo, int status)
        {
            return _mapper.Map<IEnumerable<PedidoAdmViewModel>>(await _pedidoService.FilterAsync(termo, status));
        }

        public async Task UpdateAsync(PedidoAdmViewModel pedidoAdmViewModel, string rastreio, string newTipoEnvio)
        {
            _unitOfWork.BeginTransaction();
            await _pedidoService.UpdateAsync(_mapper.Map<PedidoModel>(pedidoAdmViewModel), rastreio, newTipoEnvio);
            await _unitOfWork.CommitAsync();
        }

        public async Task AlterarProdutosPedidoAsync(UsuarioViewModel usuarioViewModel, string email, int id, int newQuantidade)
        {
            _unitOfWork.BeginTransaction();
            await _pedidoService.AlterarProdutoPedidoAsync(_mapper.Map<UsuarioModel>(usuarioViewModel), email, id, newQuantidade);
            await _unitOfWork.CommitAsync();
        }

        public async Task AprovarPedidoAsync(int id, string nomeUsuario, string email)
        {
            _unitOfWork.BeginTransaction();
            await _pedidoService.AprovarPedidoAsync(id,nomeUsuario,email);
            await _unitOfWork.CommitAsync();
        }

        public async Task ReprovarPedidoAsync(int id, string nomeUsuario, string email)
        {
            _unitOfWork.BeginTransaction();
            await _pedidoService.ReprovarPedidoAsync(id,nomeUsuario,email);
            await _unitOfWork.CommitAsync();
        }

        public async Task AdicionarItemAoPedidoAsync(int pedido, string idProduto, int cores, int quantidade)
        {
            _unitOfWork.BeginTransaction();
            await _pedidoService.AdicionarItemAoPedidoAsync(pedido,idProduto,cores,quantidade);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoverItemDoPedido(string tipoOp, int id)
        {
            _unitOfWork.BeginTransaction();
            await _pedidoService.RemoverItemDoPedido(tipoOp, id);
            await _unitOfWork.CommitAsync();
        }

        public async Task VerificarSePedidoEntregue()
        {
            _unitOfWork.BeginTransaction();
            await _pedidoService.VerificarSePedidoEntregueAsync();
            await _unitOfWork.CommitAsync();
        }

        public async Task<ViewAsPdf> GerarPedidoPdf(PedidoDetailsViewModel pedido, string email)
        {

            var empresa = await _empresaAppServices.GetEmpresaByUserId(pedido.userId);

            var usuario = await _usuarioServices.GetByUserIdAsync(pedido.userId);

            var produtos = await GetProdutosByPedido(pedido.Id);

            var quantidadeArmacoes = produtos.Sum(x => x.Quantidade);

            var pdfImpressaoModel = new ImpressaoPedidoViewModel(pedido, produtos.OrderBy(x=>x.Produto.Referencia), email,empresa,usuario.NomeCompleto,quantidadeArmacoes);

            var pdf = new ViewAsPdf
            {
                Model = pdfImpressaoModel,
                ViewName = "Impressao"
            };

            return pdf;
        }


        public async Task EnviarEmailAlteracaoPedido(PedidoDetailsViewModel pedido, string email)
        {
            var usuario = await _usuarioServices.GetByUserIdAsync(pedido.userId);
            await _pedidoService.EnviarEmailAlteracaoPedido(usuario.NomeCompleto, pedido.Id, email);
        }

        public async Task EnviarEmailPedidoDespachado(string email, int pedido, string rastreio, string nomeUsuario, byte[] anexo)
        {
            await _pedidoService.EnviarEmailPedidoDespachado(email,pedido,rastreio,nomeUsuario,anexo);
        }

        public async Task ConcluirPedido(int pedido, string userEmail)
        {
            _unitOfWork.BeginTransaction();
            await _pedidoService.ConcluirPedido(pedido, userEmail);
            await _unitOfWork.CommitAsync();
        }
    }
}
