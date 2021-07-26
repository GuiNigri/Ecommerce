using System.Collections.Generic;

namespace EcommercePrestige.Model.Entity
{
    public class CieloModel
    {
        public string OrderNumber { get; set; }
        public string SoftDescriptor { get; set; }
        public CieloCarrinhoModel Cart { get; set; }
        public CieloFreteModel Shipping { get; set; }
        public CieloPagamentoModel Payment { get; set; }
        public CieloConsumidorModel Customer { get; set; }
        public CieloOptionsModel Options { get; set; }
        public CieloSettingsModel Settings { get; set; }
    }

    public class CieloPagamentoModel
    {
        public int BoletoDiscount { get; set; }
        public int DebitDiscount { get; set; }
        public int MaxNumberOfInstallments { get; set; }
    }

    public class CieloConsumidorModel
    {
        public string Identity { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class CieloConsumidorEnderecoModel
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }

    public class CieloCarrinhoModel
    {
        public IEnumerable<CieloProdutoCarrinhoModel> Items { get; set; }
        public CieloDescontoCarrinhoModel Discount { get; set; }
    }

    public class CieloProdutoCarrinhoModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Sku { get; set; }
        public int Weight { get; set; }
    }

    public class CieloDescontoCarrinhoModel
    {
        public string Type { get; set; }
        public int Value { get; set; }
    }

    public class CieloFreteModel
    {
        public string SourceZipCode { get; set; }
        public string TargetZipCode { get; set; }
        public string Type { get; set; }
        public IEnumerable<CieloTiposFreteModel> Services { get; set; }
        public CieloConsumidorEnderecoModel Address { get; set; }
    }

    public class CieloTiposFreteModel
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Deadline { get; set; }
        public string Carrier { get; set; }
    }

    public class CieloOptionsModel
    {
        public bool AntifraudEnabled { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class CieloSettingsModel
    {
        public string CheckoutUrl { get; set; }
        public string Profile { get; set; }
        public int Version { get; set; }
    }
}
