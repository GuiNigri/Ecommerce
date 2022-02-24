using Ecommerce.Api.Controllers.Pedido.Dto;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Ecommerce.Api.Controllers.Pedido
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "AdminClaim")]
    public class PedidoController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmpresaServices _empresaServices;
        private readonly IPedidoService _pedidoService;
        private readonly IProdutoCorServices _produtoCorServices;

        public PedidoController(
            UserManager<IdentityUser> userManager,
            IUsuarioServices usuarioServices,
            IEmpresaServices empresaServices,
            IPedidoService pedidoService,
            IProdutoCorServices produtoCorServices)
        {
            _userManager = userManager;
            _usuarioServices = usuarioServices;
            _empresaServices = empresaServices;
            _pedidoService = pedidoService;
            _produtoCorServices = produtoCorServices;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegistrarPedido(RegistrarPedidoRequest request)
        {
            if (request is null)
                return BadRequest();

            var pedido = await CriarPedido(request);

            var result = await GravarPedido(request, pedido);
            
            if (result is false)
                return UnprocessableEntity();

            return Created("", new RegistrarPedidoResponse { numeroPedido = pedido.Id });
        }

        private async Task<bool> GravarPedido(RegistrarPedidoRequest request, PedidoModel pedido)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                var idPedido = await _pedidoService.CreateReturningIdAsync(pedido);

                var produtos = await CriarProdutos(request, idPedido);

                await _pedidoService.CreateProdutosAsync(produtos, new List<PedidoKitModel>());

                transaction.Complete();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private async Task<PedidoModel> CriarPedido(RegistrarPedidoRequest request)
        {
            const string tipoEnvioProprio = "proprio";
            const int statusAguardando = 0;

            var claim = User.Claims.FirstOrDefault(x => x.Type == "Id");

            var userIdentity = await _userManager.FindByIdAsync(claim.Value);

            var usuario = await _usuarioServices.GetByUserIdAsync(userIdentity.Id);

            var endereco = await _empresaServices.GetEmpresaByUserId(userIdentity.Id);

            var pedido = new PedidoModel(
                usuario.Id,
                request.Pagamento.FormaPagamento,
                request.Pagamento.Parcelas,
                request.Pagamento.Subtotal,
                request.Pagamento.Desconto,
                CalcularValorTotal(request),
                request.Pagamento.Frete,
                servicoEnvio: null,
                endereco.Cep,
                endereco.Logradouro,
                endereco.Numero.ToString(),
                endereco.Complemento,
                endereco.Bairro,
                endereco.Municipio,
                endereco.Uf,
                dataPedido: DateTime.Now,
                request.Observacoes,
                statusAguardando
                );

            pedido.SetTipoDeEnvio(tipoEnvioProprio);

            return pedido;
        }

        private async Task<IEnumerable<PedidoProdutosModel>> CriarProdutos(RegistrarPedidoRequest request, int pedido)
        {
            var produtos = new List<PedidoProdutosModel>();

            foreach (var produtoDto in request.Produtos)
            {
                var produtoCor = await _produtoCorServices.GetByIdAsync(produtoDto.Produto);

                var valorUnitario = produtoCor.ProdutoModel.ValorVenda;

                var valorTotal = valorUnitario * produtoDto.Quantidade;

                var produto = new PedidoProdutosModel(
                    pedido,
                    produtoDto.Produto,
                    produtoDto.Quantidade,
                    valorUnitario,
                    valorTotal
                    );

                produtos.Add(produto);
            }

            return produtos;
        }

        private static double CalcularValorTotal(RegistrarPedidoRequest request)
        {
            return (request.Pagamento.Subtotal - request.Pagamento.Desconto) + request.Pagamento.Frete;
        }
    }
}
