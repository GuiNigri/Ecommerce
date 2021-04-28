using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Infrastructure;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace EcommercePrestige.Services
{
    public class PedidoService:BaseServices<PedidoModel>,IPedidoService
    {
        private readonly IPedidoRepository _baseRepository;
        private readonly IProdutoCorRepository _produtoCorRepository;
        private readonly IKitsRepository _kitsRepository;
        private readonly IEmailSenderServices _emailSenderServices;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICorreiosInfrastructure _correiosInfrastructure;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICieloCheckout _cieloCheckout;
        private readonly IPagarMeCheckout _pagarMeCheckout;
        private readonly IEmpresaRepository _empresaRepository;

        public PedidoService(IPedidoRepository baseRepository, IProdutoCorRepository produtoCorRepository, IKitsRepository kitsRepository,
            IEmailSenderServices emailSenderServices, IUsuarioRepository usuarioRepository, ICorreiosInfrastructure correiosInfrastructure, UserManager<IdentityUser> userManager, ICieloCheckout cieloCheckout, IPagarMeCheckout pagarMeCheckout,IEmpresaRepository empresaRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _produtoCorRepository = produtoCorRepository;
            _kitsRepository = kitsRepository;
            _emailSenderServices = emailSenderServices;
            _usuarioRepository = usuarioRepository;
            _correiosInfrastructure = correiosInfrastructure;
            _userManager = userManager;
            _cieloCheckout = cieloCheckout;
            _pagarMeCheckout = pagarMeCheckout;
            _empresaRepository = empresaRepository;
        }

        public async Task<int> CreateReturningIdAsync(PedidoModel pedidoModel)
        {
            return await _baseRepository.CreateReturningIdAsync(pedidoModel);

        }

        public async Task CriarTransactionCieloAsync(int numeroPedido, IEnumerable<PedidoProdutosModel> produtosDoPedido, PedidoModel dadosDoPedido,string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var empresa = await _empresaRepository.GetEmpresaByUserId(userId);

            try
            {
                await _cieloCheckout.CriarTransacao(numeroPedido, dadosDoPedido, produtosDoPedido, empresa.Cnpj, empresa.RazaoSocial, user.Email, empresa.Telefone);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<PagarMeReturnModel> CreatePagarMeTransaction(PagarMeModel pagarMeModel, PedidoModel orderData, IEnumerable<PedidoProdutosModel> orderProducts, string userId)
        {
            try
            {
                return await _pagarMeCheckout.CreateTransaction(pagarMeModel, orderData,orderProducts,userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task CreateProdutosAsync(IEnumerable<PedidoProdutosModel> pedidoProdutosModel, IEnumerable<PedidoKitModel> pedidoKitModel)
        {


            foreach (var produto in pedidoProdutosModel)
            {
                await _baseRepository.CreateProdutosAsync(produto);
            }

            foreach (var pedidoKit in pedidoKitModel)
            {
                await _baseRepository.CreateKitsAsync(pedidoKit);
            }
        }

        public async Task EnviarConfirmacaoPedidoEmail(int pedido, string nomeUsuario, string emailUsuario)
        {
            var mensagemEmail = $"Olá, {nomeUsuario}!<br> <br>" +
                                "Agradecemos sua visita em nosso site. <br> " +
                                "Já estamos trabalhando para rapidamente liberar seus produtos, em até 48 horas enviaremos a confirmação e/ou caso seja nescessário faremos contato para possíveis pendencias e seguimento na liberação do mesmo. <br><br>" +
                                $"Pedido N° - <b>{pedido}</b> <br>" +
                                "Acompanhe seu pedido acessando no site o campo 'MEUS PEDIDOS' localizado no menu 'LOGIN' <br> <br> " +
                                "Boas Vendas! <br>" +
                                "Equipe de relacionamento com o cliente <br>" +
                                "Prestige do Brasil";

            await _emailSenderServices.SendEmailAsync(emailUsuario, $"Pedido realizado com sucesso - N° {pedido}", mensagemEmail, null, null);

            await EnviarConfirmacaoPedidoEmailInterno(pedido);
        }

        private async Task EnviarConfirmacaoPedidoEmailInterno(int pedido)
        {
            var mensagemEmail = $"Detectamos uma nova venda no site!<br> <br>" +
                                $"Pedido N° - <b>{pedido}</b> <br> <br>" +
                                "Verifique os detalhes do pedido acessando o link abaixo <br>" +
                                $"https://prestigedobrasil.com.br/Pedido/Edit/{pedido}";


            await _emailSenderServices.SendEmailAsync("pedidos@prestigedobrasil.com.br", $"Novo Pedido realizado no site - N° {pedido}", mensagemEmail, null, null);
        }

        public async Task<IEnumerable<PedidoModel>> GetByUsuario(int id)
        {
            return await _baseRepository.GetByUsuario(id);
        }

        public async Task<PedidoModel> GetPedido(int pedido)
        {
            return await _baseRepository.GetPedido(pedido);
        }

        public async Task<IEnumerable<PedidoProdutosModel>> GetProdutosByPedido(int pedido)
        {
            return await _baseRepository.GetProdutosByPedido(pedido);
        }
        public async Task<IEnumerable<PedidoModel>> GetApprovedAsync()
        {
            return await _baseRepository.GetApprovedAsync();
        }
        public async Task<IEnumerable<PedidoModel>> GetReprovedAsync()
        {
            return await _baseRepository.GetReprovedAsync();
        }
        public async Task<IEnumerable<PedidoModel>> GetWaitingAsync()
        {
            return await _baseRepository.GetWaitingAsync();
        }

        public async Task<IEnumerable<PedidoKitModel>> GetKitsByPedido(int pedido)
        {
            return await _baseRepository.GetKitsByPedido(pedido);
        }

        public async Task<bool> CheckPedidoUsuario(int usuarioId, int pedido)
        {
            return await _baseRepository.CheckPedidoUsuario(usuarioId, pedido);
        }

        public async Task<IEnumerable<PedidoModel>> FilterAsync(string termo, int status)
        {
            return await _baseRepository.FilterAsync(termo,status);
        }

        public async Task UpdateAsync(PedidoModel pedidoModel, string rastreio, string tipoEnvio)
        {
            if (pedidoModel.Rastreio == null && rastreio != null && tipoEnvio == "correios")
            {
                pedidoModel.SetStatus(4);
                pedidoModel.SetRastreio(rastreio);
                pedidoModel.SetTipoDeEnvio(tipoEnvio);

            }
            else if (pedidoModel.TipoDeEnvio == null && tipoEnvio == "proprio")
            {
                pedidoModel.SetStatus(4);
                pedidoModel.SetTipoDeEnvio(tipoEnvio);
            }

            await _baseRepository.UpdateAsync(pedidoModel);
        }

        public async Task EnviarEmailPedidoDespachado(string email, int pedido, string rastreio,string nomeUsuario, byte[] anexo)
        {
            var nomeArquivo = $"Pedido N° {pedido}";

            string mensagemEmail;

            if (rastreio != null)
            {
                var urlRastreio = $"https://prestigedobrasil.com.br/Package/PackageTracking?trackingCode={rastreio}&pedido={pedido}";


                mensagemEmail = $"Olá, {nomeUsuario}!<br> <br>" +
                                    $"O status do seu pedido <b>{pedido}</b> foi atualizado para <b>Despachado</b> <br> <br>" +
                                    $"Transportadora: Correios <br>" +
                                    $"Código de rastreio: <b>{rastreio}</b> <br>" +
                                    $"Para rastrear seu pedido <a href='{HtmlEncoder.Default.Encode(urlRastreio)}'>clique neste link</a> <br> <br> <br>" +
                                    $"Equipe de relacionamento com o cliente <br>" +
                                    $"Prestige do Brasil";
            }
            else
            {
                mensagemEmail = $"Olá, {nomeUsuario}!<br> <br>" +
                                $"O status do seu pedido <b>{pedido}</b> foi atualizado para <b>Pronto para entrega</b> <br> <br>" +
                                $"Modo de envio: Próprio <br> <br> <br>" +
                                $"Equipe de relacionamento com o cliente <br>" +
                                $"Prestige do Brasil";
            }


            await _emailSenderServices.SendEmailAsync(email, $"Atualização de status do pedido {pedido}", mensagemEmail, anexo, nomeArquivo);
        }

        public async Task AlterarProdutoPedidoAsync(UsuarioModel usuarioModel, string email, int id, int newQuantidade)
        {
            var produtosPedido = await _baseRepository.GetProdutosById(id);

            if (produtosPedido != null)
            {
                if (produtosPedido.Quantidade != newQuantidade)
                {
                    var diferencaValores = (produtosPedido.ValorUnitario * newQuantidade) - produtosPedido.ValorTotal;

                    var diferencaQuantidade = newQuantidade - produtosPedido.Quantidade;

                    produtosPedido.SetQuantidade(newQuantidade);

                    produtosPedido.SetValorTotal(produtosPedido.ValorUnitario * newQuantidade);

                    await _baseRepository.UpdateProdutosPedidoAsync(produtosPedido);

                    await AtualizarValoresDoPedido(diferencaValores, produtosPedido.PedidoModelId);

                    await DarBaixaEstoque(null, produtosPedido.ProdutoCorModel.Id, diferencaQuantidade, "produto");
                }
            }
            else
            {
                var kitPedido = await _baseRepository.GetKitsById(id);

                if (kitPedido.Quantidade != newQuantidade)
                {
                    var produtosKit = await _produtoCorRepository.GetListByKitAsync(kitPedido.KitModel.Nome, null);

                    var diferencaValores = (kitPedido.ValorUnitario * newQuantidade) - kitPedido.ValorTotal;

                    var diferencaQuantidade = newQuantidade - kitPedido.Quantidade;

                    kitPedido.SetQuantidade(newQuantidade);

                    kitPedido.SetValorTotal(kitPedido.ValorUnitario * newQuantidade);

                    await _baseRepository.UpdateKitsPedidoAsync(kitPedido);

                    await AtualizarValoresDoPedido(diferencaValores, kitPedido.PedidoModelId);

                    await DarBaixaEstoque(produtosKit, 0, diferencaQuantidade, "kit");
                }

            }

        }


        public async Task AprovarPedidoAsync(int id, string nomeUsuario, string email)
        {
            var produtosDoPedido = await _baseRepository.GetProdutosByPedido(id);

            foreach (var produtos in produtosDoPedido)
            {
                await DarBaixaEstoque(null, produtos.ProdutoCorModelId, -produtos.Quantidade, "produto");
            }

            var KitDoPedido = await _baseRepository.GetKitsById(id);

            if (KitDoPedido != null)
            {
                var ProdutosDoKitDoPedido = await _produtoCorRepository.GetListByKitAsync(KitDoPedido.KitModel.Nome, null);

                await DarBaixaEstoque(ProdutosDoKitDoPedido, 0, -KitDoPedido.Quantidade, "kit");
            }


            var pedidoModel = await _baseRepository.GetPedido(id);

            pedidoModel.SetStatus(4);

            await _baseRepository.UpdateAsync(pedidoModel);

            var mensagemEmail = $"Olá, {nomeUsuario}!<br> <br>" +
                                $"O status do seu pedido <b>{pedidoModel.Id}</b> foi atualizado para <b>Aprovado</b> <br> <br> <br>" +
                                $"Equipe de relacionamento com o cliente <br>" +
                                $"Prestige do Brasil";

            await _emailSenderServices.SendEmailAsync(email, $"Atualização de status do pedido {pedidoModel.Id}", mensagemEmail, null, null);
        }

        public async Task ReprovarPedidoAsync(int id, string nomeUsuario, string email)
        {
            var pedidoModel = await _baseRepository.GetPedido(id);

            pedidoModel.SetStatus(4);

            await _baseRepository.UpdateAsync(pedidoModel);

            var mensagemEmail = $"Olá, {nomeUsuario}!<br> <br>" +
                                $"O status do seu pedido <b>{pedidoModel.Id}</b> foi atualizado para <b>Reprovado</b> <br> <br>" +
                                $"Para mais informações entre em contato com o suporte. <br> <br> <br>" +
                                $"Equipe de relacionamento com o cliente <br>" +
                                $"Prestige do Brasil";

            await _emailSenderServices.SendEmailAsync(email, $"Atualização de status do pedido {pedidoModel.Id}", mensagemEmail, null, null);
        }

        public async Task CancelarPedidoAsync(int id, string nomeUsuario, string email)
        {
            var produtosDoPedido = await _baseRepository.GetProdutosByPedido(id);

            foreach (var produtos in produtosDoPedido)
            {
                await DarBaixaEstoque(null, produtos.ProdutoCorModelId, +produtos.Quantidade, "produto");
            }

            var KitDoPedido = await _baseRepository.GetKitsById(id);

            if (KitDoPedido != null)
            {
                var ProdutosDoKitDoPedido = await _produtoCorRepository.GetListByKitAsync(KitDoPedido.KitModel.Nome, null);

                await DarBaixaEstoque(ProdutosDoKitDoPedido, 0, +KitDoPedido.Quantidade, "kit");
            }

            var pedidoModel = await _baseRepository.GetPedido(id);

            pedidoModel.SetStatus(4);

            await _baseRepository.UpdateAsync(pedidoModel);

            var mensagemEmail = $"Olá, {nomeUsuario}!<br> <br>" +
                                $"O status do seu pedido <b>{pedidoModel.Id}</b> foi atualizado para <b>Cancelado</b> <br> <br>" +
                                $"Para mais informações entre em contato com o suporte. <br> <br> <br>" +
                                $"Equipe de relacionamento com o cliente <br>" +
                                $"Prestige do Brasil";

            await _emailSenderServices.SendEmailAsync(email, $"Atualização de status do pedido {pedidoModel.Id}", mensagemEmail, null, null);
        }

        public async Task AdicionarItemAoPedidoAsync(int pedido,string idProduto, int cores, int quantidade)
        {
            var identificacao = idProduto.Split(",")[1].Trim();
            var id = int.Parse(idProduto.Split(",")[0].Trim());

            if (identificacao == "produto")
            {
                var produtoCorModel = await _produtoCorRepository.GetCorByProdutoAsync(cores,id,"AT");

                var valorUnitario = produtoCorModel.ProdutoModel.ValorVenda;
                var valorTotal = valorUnitario * quantidade;

                await _baseRepository.CreateProdutosAsync(new PedidoProdutosModel(pedido, produtoCorModel.Id, quantidade,
                    valorUnitario, valorTotal));

                await DarBaixaEstoque(null, produtoCorModel.Id, -quantidade, "produto");

                await AtualizarValoresDoPedido(valorTotal, pedido);
            }
            else
            {
                var kit = await _kitsRepository.GetByIdAsync(id);

                var valorUnitario = kit.ValorVenda;
                var valorTotal = valorUnitario * quantidade;

                await _baseRepository.CreateKitsAsync(new PedidoKitModel(pedido, id, quantidade, valorUnitario,
                    valorTotal));

                await DarBaixaEstoque(await _produtoCorRepository.GetListByKitAsync(kit.Nome, null),
                    0, -quantidade, "kit");

                await AtualizarValoresDoPedido(valorTotal, pedido);

            }
        }

        public async Task RemoverItemDoPedido(string tipoOp, int id)
        {
            if (tipoOp == "produto")
            {
                var produtosPedido = await _baseRepository.GetProdutosById(id);

                await AtualizarValoresDoPedido(-produtosPedido.ValorTotal, produtosPedido.PedidoModelId);

                await DarBaixaEstoque(null, produtosPedido.ProdutoCorModel.Id, produtosPedido.Quantidade, "produto");

                await _baseRepository.DeleteProdutosAsync(produtosPedido);
            }
            else
            {
                var kitPedido = await _baseRepository.GetKitsById(id);

                await AtualizarValoresDoPedido(-kitPedido.ValorTotal, kitPedido.PedidoModelId);

                await DarBaixaEstoque(await _produtoCorRepository.GetListByKitAsync(kitPedido.KitModel.Nome, null),
                    0, kitPedido.Quantidade, "kit");

                await _baseRepository.DeleteKitsAsync(kitPedido);
            }
        }

        public async Task<bool> CheckIfExistOrderByUserAsync(string userId)
        {
            var usuario = await _usuarioRepository.GetByUserIdAsync(userId);

            return await _baseRepository.CheckIfExistOrderByUserAsync(usuario.Id);
        }

        public async Task VerificarSePedidoEntregueAsync()
        {
            var pedidos = await _baseRepository.GetPedidosDespachados();

            foreach (var pedido in pedidos)
            {
                if (!await _correiosInfrastructure.VerificarSeEntregue(pedido.Rastreio)) continue;

                pedido.SetStatus(4);
                await _baseRepository.UpdateAsync(pedido);

                var userIdentity = await _userManager.FindByIdAsync(pedido.UsuarioModel.UserId);

                var mensagemEmail = $"Olá, {pedido.UsuarioModel.NomeCompleto}!<br> <br>" +
                                    $"O seu pedido n° <b>{pedido.Id}</b> foi atualizado para <b>Concluido/Entregue</b> <br> <br>" +
                                    $"Se tiver qualquer problema conte com a nossa equipe de pós venda. <br> <br> <br>" +
                                    $"Obrigado pela confiança e boas venda <br>" +
                                    $"Prestige do Brasil";

                await _emailSenderServices.SendEmailAsync(userIdentity.Email, $"Pedido N° {pedido.Id} Entregue!", mensagemEmail, null, null);
            }
        }

        private async Task DarBaixaEstoque(IEnumerable<ProdutoCorModel> Produtos, int produtoId, int quantidade, string tipoOp)
        {
            if (tipoOp == "produto")
            {
                await _produtoCorRepository.BaixaEstoque(produtoId, quantidade);
            }
            else
            {
                foreach (var produtos in Produtos)
                {
                    await _produtoCorRepository.BaixaEstoque(produtos.Id, quantidade);
                }
            }
        }

        private async Task AtualizarValoresDoPedido(double valorTotal, int pedido)
        {
            var pedidoModel = await _baseRepository.GetPedido(pedido);

            var subTotal = pedidoModel.Subtotal + valorTotal;

            pedidoModel.SetSubTotal(subTotal);

            var total = pedidoModel.ValorTotal + valorTotal - pedidoModel.Desconto;

            pedidoModel.SetValorTotal(total);

            await _baseRepository.UpdateAsync(pedidoModel);
        }

        public async Task EnviarEmailAlteracaoPedido(string nome, int numeroPedido, string email)
        {
            var mensagemEmail = $"Olá, {nome}!<br> <br>" +
                                $"Seu pedido foi alterado pela nossa equipe. Entre na página 'Pedidos' para conferir a alteração ou então entre em contato com o nosso suporte.<br> <br>" +
                                $"Pedido: <b> {numeroPedido}</b> <br>" +
                                $"Data da alteração: <b>{TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))}</b> <br> <br> <br>" +
                                $"Equipe de relacionamento com o cliente <br>" +
                                $"Prestige do Brasil";

            await _emailSenderServices.SendEmailAsync(email,
                $"Pedido {numeroPedido} - Alterado pela nossa equipe", mensagemEmail, null, null);
        }

        public async Task ConcluirPedido(int pedido, string userEmail)
        {
            var pedidoModel = await _baseRepository.GetPedido(pedido);

            pedidoModel.SetStatus(4);

            await _baseRepository.UpdateAsync(pedidoModel);

            var mensagemEmail = $"Olá, {pedidoModel.UsuarioModel.NomeCompleto}!<br> <br>" +
                                $"O seu pedido n° <b>{pedidoModel.Id}</b> foi atualizado para <b>Concluido/Entregue</b> <br> <br>" +
                                $"Se tiver qualquer problema conte com a nossa equipe de pós venda. <br> <br> <br>" +
                                $"Obrigado pela confiança e boas venda <br>" +
                                $"Prestige do Brasil";
            
            await _emailSenderServices.SendEmailAsync(userEmail, $"Pedido N° {pedidoModel.Id} Entregue!", mensagemEmail, null, null);


        }
    }
}
