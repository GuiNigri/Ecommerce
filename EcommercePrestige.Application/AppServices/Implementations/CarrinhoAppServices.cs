using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EcommercePrestige.Application.AutoMapper;
using EcommercePrestige.Application.Helpers;
using EcommercePrestige.Application.ViewModel;
using EcommercePrestige.Model.Interfaces.Infrastructure;
using EcommercePrestige.Model.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EcommercePrestige.Application.AppServices.Implementations
{
    public class CarrinhoAppServices:ICarrinhoAppServices
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ICorreiosServices _correiosServices;
        private readonly IProdutoCorServices _produtoCorServices;
        private readonly IKitsServices _kitsServices;
        private readonly bool statusPromo;
        private readonly IMapper _mapper;

        public CarrinhoAppServices(IHttpContextAccessor httpContext, ICorreiosServices correiosServices, IProdutoCorServices produtoCorServices, IKitsServices kitsServices)
        {
            _httpContext = httpContext;
            _correiosServices = correiosServices;
            _produtoCorServices = produtoCorServices;
            _kitsServices = kitsServices;
            _mapper = AutoMapperConfig.Mapper;

            statusPromo = false;
        }

        public async Task<string> AdicionarItemAoCarrinho(ProdutoViewModel produtoModel, KitsViewModel kitsModel, CorViewModel corViewModel, int quantidade, int corId)
        {

            var valorItem = produtoModel.Id > 0 ? (double.Parse(produtoModel.ValorVenda) * quantidade) :
                (double.Parse(kitsModel.ValorVenda) * quantidade);
            

            if (SessionHelper.GetObjectFromJson<List<CarrinhoViewModel>>(_httpContext.HttpContext.Session, "cart") == null)
            {
                List<CarrinhoViewModel> carrinho;

                if (produtoModel.Id > 0)
                {
                    if (!await HasEstoque(produtoModel.Id, corViewModel.Id, quantidade)) return "Estoque insuficiente";


                    carrinho = new List<CarrinhoViewModel>
                    {
                        new CarrinhoViewModel {Produto = produtoModel, QuantidadeIndividual = quantidade, Cor = corViewModel, ValorUnitarioTotal = valorItem.ToString("C"),
                            CorId = corId}
                    };
                }
                else
                {
                    if (!await HasEstoque(kitsModel.Id, quantidade, "AT")) return "Estoque insuficiente";

                    carrinho = new List<CarrinhoViewModel>
                    {
                        new CarrinhoViewModel {Kits = kitsModel, QuantidadeIndividual = quantidade, ValorUnitarioTotal = valorItem.ToString("C")}
                    };
                }

                var carrinhoInicialAtualizadoPromo = await VerificarPromoCarrinho(carrinho);

                SessionHelper.SetObjectAsJson(_httpContext.HttpContext.Session, "cart", carrinhoInicialAtualizadoPromo);

                return "Produto adicionado ao carrinho";

            }

            var cart = SessionHelper.GetObjectFromJson<List<CarrinhoViewModel>>(_httpContext.HttpContext.Session,
                "cart");

            int index;
            var quantReal = quantidade;

            if (produtoModel.Id > 0)
            {
                index = IsExist(produtoModel.Id, corViewModel.Id);

                

                if (index != -1)
                {
                    cart[index].QuantidadeIndividual += quantidade;
                    cart[index].ValorUnitarioTotal =
                        (double.Parse(produtoModel.ValorVenda) * cart[index].QuantidadeIndividual).ToString("C");

                    quantReal = cart[index].QuantidadeIndividual;
                }
                else
                {
                    cart.Add(new CarrinhoViewModel
                        { Produto = produtoModel, QuantidadeIndividual = quantidade, Cor = corViewModel, ValorUnitarioTotal = valorItem.ToString("C"), CorId = corId });
                }

                if (!await HasEstoque(produtoModel.Id, corViewModel.Id, quantReal)) return "Estoque insuficiente";
            }
            else
            {
                index = IsExist(kitsModel.Id, 0);

                if (index != -1)
                {
                    cart[index].QuantidadeIndividual += quantidade;
                    cart[index].ValorUnitarioTotal =
                        (double.Parse(kitsModel.ValorVenda) * cart[index].QuantidadeIndividual).ToString("C");

                    quantReal = cart[index].QuantidadeIndividual;
                }
                else
                {
                    cart.Add(new CarrinhoViewModel
                        { Kits = kitsModel, QuantidadeIndividual = quantidade, ValorUnitarioTotal = valorItem.ToString("C") });
                }

                if (!await HasEstoque(kitsModel.Id, quantReal, "AT")) return "Estoque insuficiente";
            }

            //verificar promoção no carrinho todoo
            var carrinhoAtualizadoPromo = await VerificarPromoCarrinho(cart);

            SessionHelper.SetObjectAsJson(_httpContext.HttpContext.Session, "cart", carrinhoAtualizadoPromo);

            return "Produto adicionado ao carrinho";

        }

        public async Task AtualizarCarrinho(ProdutoViewModel produtoModel, KitsViewModel kitsModel, int corId, int quantidade)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CarrinhoViewModel>>(_httpContext.HttpContext.Session, "cart");

            if (produtoModel.Id > 0)
            {
                if (await HasEstoque(produtoModel.Id, corId, quantidade))
                {
                    var index = IsExist(produtoModel.Id, corId);
                    if (index != -1)
                    {
                        cart[index].QuantidadeIndividual = quantidade;
                        cart[index].ValorUnitarioTotal =
                            (double.Parse(produtoModel.ValorVenda) * cart[index].QuantidadeIndividual).ToString("C");
                    }
                }
            }
            else
            {
                if (await HasEstoque(kitsModel.Id, quantidade,"AT"))
                {
                    var index = IsExist(kitsModel.Id, 0);

                    if (index != -1)
                    {
                        cart[index].QuantidadeIndividual = quantidade;
                        cart[index].ValorUnitarioTotal = (double.Parse(kitsModel.ValorVenda) * cart[index].QuantidadeIndividual).ToString("C");
                    }
                }
            }

            var carrinhoAtualizadoPromo = await VerificarPromoCarrinho(cart);

            SessionHelper.SetObjectAsJson(_httpContext.HttpContext.Session, "cart", carrinhoAtualizadoPromo);

        }

        public async Task RemoverItemDoCarrinho(int idProd, int idCor)
        {

            var cart = SessionHelper.GetObjectFromJson<List<CarrinhoViewModel>>(_httpContext.HttpContext.Session, "cart");

            var index = IsExist(idProd, idCor);

            cart.RemoveAt(index);

            var carrinhoAtualizadoPromo = await VerificarPromoCarrinho(cart);

            SessionHelper.SetObjectAsJson(_httpContext.HttpContext.Session, "cart", carrinhoAtualizadoPromo);
        }


        private async Task<IEnumerable<CarrinhoViewModel>> VerificarPromoCarrinho(List<CarrinhoViewModel> cart)
        {
            if (!statusPromo) return cart;

            var quantPrestige = 0;
            var quantPeople = 0;
            var quantAzzaro = 0;


            foreach (var item in cart)
            {
                switch (item.Produto.MarcaModel.ToLower())
                {
                    case "people":
                        quantPeople += item.QuantidadeIndividual;
                        break;
                    case "prestige":
                        quantPrestige += item.QuantidadeIndividual;
                        break;
                    case "azzaro":
                        quantAzzaro += item.QuantidadeIndividual;
                        break;
                }
            }

            int percDesconto;

            if (quantPeople >= 25)
            {
                percDesconto = quantPeople >= 50 ? 15 : 10;

                foreach (var item in cart)
                {
                    if (item.Produto.MarcaModel.ToLower() != "people") continue;
                    var valorTotalUnitario = double.Parse(item.Produto.ValorVenda) * item.QuantidadeIndividual;
                    item.DescontoUnitarioProduto = valorTotalUnitario * percDesconto / 100;
                }
            }
            else
            {
                foreach (var item in cart.Where(item => item.Produto.MarcaModel.ToLower() == "people"))
                {
                    item.DescontoUnitarioProduto = 0;
                }
            }

            if (quantPrestige >= 15)
            {
                percDesconto = quantPrestige >= 30 ? 15 : 10;

                foreach (var item in cart)
                {
                    if (item.Produto.MarcaModel.ToLower() != "prestige") continue;
                    var valorTotalUnitario = double.Parse(item.Produto.ValorVenda) * item.QuantidadeIndividual;
                    item.DescontoUnitarioProduto = valorTotalUnitario * percDesconto / 100;
                }
            }
            else
            {
                foreach (var item in cart.Where(item => item.Produto.MarcaModel.ToLower() == "prestige"))
                {
                    item.DescontoUnitarioProduto = 0;
                }
            }

            if (quantAzzaro >= 20)
            {
                percDesconto = quantAzzaro >= 40 ? 15 : 10;

                foreach (var item in cart)
                {
                    if (item.Produto.MarcaModel.ToLower() != "azzaro") continue;

                    var valorTotalUnitario = double.Parse(item.Produto.ValorVenda) * item.QuantidadeIndividual;
                    item.DescontoUnitarioProduto = valorTotalUnitario * percDesconto / 100;
                }
            }
            else
            {
                foreach (var item in cart.Where(item => item.Produto.MarcaModel.ToLower() == "azzaro"))
                {
                    item.DescontoUnitarioProduto = 0;
                }
            }

            return cart;

        }
        private int IsExist(int id, int corId)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CarrinhoViewModel>>(_httpContext.HttpContext.Session, "cart");

            if (cart == null) return -1;

            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].Produto != null)
                {
                    if (cart[i].Produto.Id.Equals(id) && cart[i].Cor.Id.Equals(corId))
                    {
                        return i;
                    }
                }
                else if (cart[i].Kits != null)
                {
                    if (cart[i].Kits.Id.Equals(id) && corId <= 0)
                    {
                        return i;
                    }
                }

            }
            return -1;
        }

        private async Task<bool> HasEstoque(int prodId, int corId, int quantidade)
        {
            var carrinho = SessionHelper.GetObjectFromJson<List<CarrinhoViewModel>>(_httpContext.HttpContext.Session, "cart");

            if (carrinho == null) return await _produtoCorServices.VerificarEstoque(prodId, corId, quantidade, "AT");

            var kits = carrinho.FindAll(x => x.Kits != null);

            if (!kits.Any()) return await _produtoCorServices.VerificarEstoque(prodId, corId, quantidade,"AT");

            foreach (var item in kits)
            {
                var (produtosDoKit,status) = await _produtoCorServices.GetListByKitAsync(item.Kits.Nome, "AT");

                quantidade += produtosDoKit.Where(prod => prod.ProdutoModelId == prodId && prod.CorModelId == corId).Sum(prod => item.QuantidadeIndividual);
            }

            return await _produtoCorServices.VerificarEstoque(prodId, corId, quantidade, "AT");
        }

        private async Task<bool> HasEstoque(int kitId, int quantidade, string statusAtivacao)
        {
            var kit = await _kitsServices.GetByIdAsync(kitId);

            var (armacoesDoKit,status) = await _produtoCorServices.GetListByKitAsync(kit.Nome, "AT");

            var carrinho = SessionHelper.GetObjectFromJson<List<CarrinhoViewModel>>(_httpContext.HttpContext.Session, "cart");

            foreach (var item in armacoesDoKit)
            {
                var quant = quantidade;

                if (carrinho != null)
                {
                    foreach (var produtos in carrinho)
                    {
                        if (produtos.Produto != null)
                        {
                            if (produtos.Produto.Id == item.ProdutoModelId && produtos.Cor.Id == item.CorModelId)
                            {
                                quant += produtos.QuantidadeIndividual;
                            }
                        }
                        else
                        {
                            if (produtos.Kits.Id == kit.Id) continue;

                            var kits = await _kitsServices.GetByIdAsync(produtos.Kits.Id);

                            var (armacoes, st) = await _produtoCorServices.GetListByKitAsync(kits.Nome, "AT");

                            quant += armacoes.Where(x => x.ProdutoModelId == item.ProdutoModelId && x.CorModelId == item.CorModelId).Sum(x => produtos.QuantidadeIndividual);
                        }

                    }
                }

                if (!await _produtoCorServices.VerificarEstoque(item.ProdutoModelId, item.CorModelId, quant, "AT"))
                {
                    return false;
                }
            }

            
            return true;

        }

        public async Task<CarrinhoPagamentoViewModel> GetAddress(string cep)
        {
            var addressModel = await _correiosServices.AddressByZipCode(cep);

            return new CarrinhoPagamentoViewModel(addressModel);
        }

        public async Task<IEnumerable<string>> GetParcelas(string formaPagamento, double valor)
        {

            var listaParcelas = new List<string>();

            if (formaPagamento == "boleto" || formaPagamento == "cartao" || formaPagamento == "cheque")
            {
                if (valor >= 700)
                {
                    for (var i = 1; i < 7; i++)
                    {
                        if (valor / i >= 350)
                        {
                            listaParcelas.Add($"<option value= {i}> {i}x sem juros </option>");
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    listaParcelas.Add($"<option value= 1> 1x sem juros </option>");
                }

            }
            else
            {
                listaParcelas.Add($"<option value=1> À vista </option>");
            }

            return listaParcelas;
        }

        public async Task<IEnumerable<CorreioWebServiceViewModel>> GetFrete(string cepDestino, int quantidadePecas, double valorTotalPedido)
        {
            
            return _mapper.Map<IEnumerable<CorreioWebServiceViewModel>>(await _correiosServices.GetFrete(cepDestino, quantidadePecas, valorTotalPedido));
        }
    }
}
