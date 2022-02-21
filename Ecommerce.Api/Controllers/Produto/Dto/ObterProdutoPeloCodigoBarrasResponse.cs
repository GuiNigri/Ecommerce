namespace Ecommerce.Api.Controllers.Produto.Dto
{
    public class ObterProdutoPeloCodigoBarrasResponse
    {
        public int Id { get; set; }
        public string Referencia { get; set; }
        public string Cor { get; set; }
        public double ValorUnitario { get; set; }
    }
}
