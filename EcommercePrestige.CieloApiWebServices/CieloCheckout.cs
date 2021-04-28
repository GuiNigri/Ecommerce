using Cielo;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EcommercePrestige.CieloApiWebServices
{
    public class CieloCheckout:ICieloCheckout
    {
        private readonly IHttpClientFactory _clientFactory;

        public CieloCheckout(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task CriarTransacao(int numeroPedido, PedidoModel dadosDoPedido, IEnumerable<PedidoProdutosModel> produtosDoPedido, 
            string identity, string fullName, string email, string phone)
        {
            var listaProdutosCielo = new List<CieloProdutoCarrinhoModel>();

            foreach (var produto in produtosDoPedido)
            {
                var cieloProdutoCarrinhoModel = new CieloProdutoCarrinhoModel
                {
                    Name = produto.ProdutoCorModel.ProdutoModel.Referencia,
                    Description = null,
                    Quantity = produto.Quantidade,
                    UnitPrice = int.Parse(produto.ProdutoCorModel.ProdutoModel.ValorVenda.ToString().Replace(",", "").Replace(".","")),
                    Type = "Asset",
                    Weight = 300
                };

                listaProdutosCielo.Add(cieloProdutoCarrinhoModel);
            }

            var address = new CieloConsumidorEnderecoModel
            {
                Street = dadosDoPedido.Rua,
                Number = dadosDoPedido.Numero,
                Complement = dadosDoPedido.Complemento,
                District = dadosDoPedido.Bairro,
                City = dadosDoPedido.Cidade,
                State = dadosDoPedido.Estado
            };

            var discount = new CieloDescontoCarrinhoModel
            {
                Type = "Amount",
                Value = int.Parse(dadosDoPedido.Desconto.ToString("F").Replace(",", "").Replace(".", ""))
            };

            var cart = new CieloCarrinhoModel
            {
                Discount = discount,
                Items = listaProdutosCielo
            };

            var listaTipoDeFreteCielo = new List<CieloTiposFreteModel>();

            var service = new CieloTiposFreteModel
            {
                Name = "Correios",
                Price = 28,
                Deadline = 5,
                Carrier = null
            };

            listaTipoDeFreteCielo.Add(service);

            var shipping = new CieloFreteModel
            {
                SourceZipCode = "22240004",
                TargetZipCode = dadosDoPedido.Cep,
                Type = "FixedAmount",
                Services = listaTipoDeFreteCielo,
                Address = address
            };

            var payment = new CieloPagamentoModel
            {
                MaxNumberOfInstallments = int.Parse(dadosDoPedido.Parcelas)
            };

            var customer = new CieloConsumidorModel
            {
                Identity = identity,
                FullName = fullName,
                Email = email,
                Phone = phone
            };

            var options = new CieloOptionsModel
            {
                AntifraudEnabled = true,
                ReturnUrl = "https://prestigedobrasil.com.br"
            };

            var transaction = new CieloModel();

            transaction.OrderNumber = numeroPedido.ToString();
            transaction.SoftDescriptor = "prestigebra";

            transaction.Cart = cart;

            transaction.Shipping = shipping;

            transaction.Payment = payment;

            transaction.Customer = customer;

            transaction.Options = options;

            transaction.Settings = null;

            try
            {

                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, "https://cieloecommerce.cielo.com.br/api/public/v1/orders"))
                {
                
                    var json = JsonConvert.SerializeObject(transaction);
                    using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        request.Headers.Add("MerchantId", "");
                        request.Content = stringContent;
                        //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                
                        using (var response = await client
                            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                            .ConfigureAwait(false))
                        {
                            response.EnsureSuccessStatusCode();
                            var respostaRetornoMensagemCielo = response.Content.ReadAsStringAsync();

                            var cieloModelRetornado = JsonConvert.DeserializeObject<CieloModel>(respostaRetornoMensagemCielo.Result);

                            var urlCheckoutTransacional = cieloModelRetornado.Settings.CheckoutUrl;
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
    }
}
