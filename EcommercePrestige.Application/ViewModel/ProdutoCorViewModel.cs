namespace EcommercePrestige.Application.ViewModel
{
    public class ProdutoCorViewModel:BaseViewModel
    {
        public string Descricao { get; set; }
        public int CorId { get; set; }
        public string ImgCor { get; set; }
        public string DescricaoCor { get; set; }
        public string CodigoInternoCor { get; set; }
        public int Estoque { get; set; }
        public string CodigoBarras { get; set; }
        public int ProdutosModelId { get; set; }
        public string ProdutosModel { get; set; }
        public bool PedidosGold { get; set; }
        public bool PedidosSilver { get; set; }
        public bool PedidosBasic { get; set; }
    }
}
