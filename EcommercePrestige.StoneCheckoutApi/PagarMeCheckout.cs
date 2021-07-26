using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Infrastructure;
using EcommercePrestige.Model.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using PagarMe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommercePrestige.StoneCheckoutApi
{
    public class PagarMeCheckout : IPagarMeCheckout
    {
        private readonly IProdutoCorRepository _produtoCorRepository;
        private readonly PagarMeSettingsModel _pagarMeSettings;

        public PagarMeCheckout(IProdutoCorRepository produtoCorRepository, IOptions<PagarMeSettingsModel> pagarMeSettings)
        {
            _produtoCorRepository = produtoCorRepository;
            _pagarMeSettings = pagarMeSettings.Value;
        }

        public async Task<PagarMeReturnModel> CreateTransaction(PagarMeModel pagarMeModel, PedidoModel orderData, IEnumerable<PedidoProdutosModel> orderProducts, string userId)
        {

            PagarMeService.DefaultApiKey = _pagarMeSettings.DefaultApiKey;
            PagarMeService.DefaultEncryptionKey = _pagarMeSettings.DefaultEncryptionKey;

            var transaction = new Transaction();

            transaction.Amount = pagarMeModel.Amount;

            transaction.CardHash = pagarMeModel.Card_hash;

            var documentType = pagarMeModel.Customer.Document_number.Length == 11 ? DocumentType.Cpf : DocumentType.Cnpj;
            var customerType = pagarMeModel.Customer.Document_number.Length == 11 ? CustomerType.Individual : CustomerType.Corporation;

            transaction.Customer = new Customer
            {
                ExternalId = $"{userId}",
                Name = pagarMeModel.Customer.Name,
                Type = customerType,
                Country = "br",
                Email = pagarMeModel.Customer.Email,
                Documents = new[]
                {
                    new Document{
                        Type = documentType,
                        Number = pagarMeModel.Customer.Document_number
                    }
                },
                PhoneNumbers = new string[]
                {
                    $"+55{pagarMeModel.Customer.Phone.Ddd}{pagarMeModel.Customer.Phone.Number}"
                }
            };

            transaction.Billing = new Billing
            {
                Name = pagarMeModel.Customer.Name,
                Address = new Address()
                {
                    Country = "br",
                    State = pagarMeModel.Customer.Address.State,
                    City = pagarMeModel.Customer.Address.City,
                    Neighborhood = pagarMeModel.Customer.Address.Neighborhood,
                    Street = pagarMeModel.Customer.Address.Street,
                    StreetNumber = pagarMeModel.Customer.Address.Street_number,
                    Zipcode = pagarMeModel.Customer.Address.Zipcode
                }
            };


            transaction.Shipping = new Shipping
            {
                Name = "Correios",
                Fee = int.Parse(orderData.Frete.ToString("N2").Replace(",",".").Replace(".","")),
                Address = new Address()
                {
                    Country = "br",
                    State = orderData.Estado,
                    City = orderData.Cidade,
                    Neighborhood = orderData.Bairro,
                    Street = orderData.Rua,
                    StreetNumber = orderData.Numero,
                    Zipcode = orderData.Cep
                }
            };


            var productsPagarMe = new List<Item>();

            foreach (var product in orderProducts)
            {
                product.SetProdutoCorModel(await _produtoCorRepository.GetByIdAsync(product.ProdutoCorModelId));

                var itemProduct = new Item()
                {
                    Id = product.ProdutoCorModel.ProdutoModel.Id.ToString(),
                    Title = $"{product.ProdutoCorModel.ProdutoModel.Referencia} | {product.ProdutoCorModel.CodigoInterno}",
                    UnitPrice = int.Parse(product.ValorUnitario.ToString().Replace(",", "").Replace(".", "")),
                    Quantity = product.Quantidade,
                    Tangible = true
                };

                productsPagarMe.Add(itemProduct);
            }

            transaction.Item = productsPagarMe.ToArray();

            transaction.SoftDescriptor = "prestigedobra";

            transaction.Save();

            var pagarMeReturnModel = new PagarMeReturnModel(transaction.Status.ToString().ToLower(), transaction.Tid,
                transaction.AuthorizationCode);

            return pagarMeReturnModel;
            
        }
    }
}
