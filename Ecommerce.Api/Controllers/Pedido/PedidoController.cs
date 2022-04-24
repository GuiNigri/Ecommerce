using Ecommerce.Api.Controllers.Pedido.Dto;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public PedidoController(
            UserManager<IdentityUser> userManager,
            IUsuarioServices usuarioServices,
            IEmpresaServices empresaServices,
            IPedidoService pedidoService,
            IProdutoCorServices produtoCorServices,
            ILogger<PedidoController> logger)
        {
            _userManager = userManager;
            _usuarioServices = usuarioServices;
            _empresaServices = empresaServices;
            _pedidoService = pedidoService;
            _produtoCorServices = produtoCorServices;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RegistrarPedidoResponse>> RegistrarPedido(RegistrarPedidoRequest request)
        {
            try
            {
                if (request is null)
                    return BadRequest();

                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                var (usuario, identity) = await ObterDadosDoUsuario();

                var pedido = await CriarPedido(request, usuario, identity);

                await GravarPedido(request, pedido, usuario, identity);

                transaction.Complete();

                _logger.LogInformation($"Pedido via api registrado com sucesso, n°: {pedido.Id}");

                return Created("", new RegistrarPedidoResponse { numeroPedido = pedido.Id });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, exception);
            }

        }

        private async Task<(UsuarioModel, IdentityUser)> ObterDadosDoUsuario()
        {
            var claim = User.Claims.FirstOrDefault(x => x.Type == "Id");

            var userIdentity = await _userManager.FindByIdAsync(claim.Value);

            var usuario = await _usuarioServices.GetByUserIdAsync(userIdentity.Id);

            return (usuario, userIdentity);
        }

        private async Task GravarPedido(RegistrarPedidoRequest request, PedidoModel pedido, UsuarioModel usuario, IdentityUser identity)
        {
            var idPedido = await _pedidoService.CreateReturningIdAsync(pedido);

            var produtos = await CriarProdutos(request, idPedido);

            await _pedidoService.CreateProdutosAsync(produtos, new List<PedidoKitModel>());

            await _pedidoService.EnviarConfirmacaoPedidoEmail(idPedido, usuario.NomeCompleto, identity.Email);
        }

        private async Task<PedidoModel> CriarPedido(RegistrarPedidoRequest request, UsuarioModel usuario, IdentityUser identity)
        {
            const string tipoEnvioProprio = "proprio";
            const int statusConfirmado = 1;

            var endereco = await _empresaServices.GetEmpresaByUserId(identity.Id);

            var dataAtualBrasil = TimeZoneInfo.ConvertTime(DateTime.Now,
                    TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

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
                dataPedido: dataAtualBrasil,
                request.Observacoes,
                statusConfirmado
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
