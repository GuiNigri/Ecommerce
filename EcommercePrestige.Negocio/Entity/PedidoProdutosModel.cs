namespace EcommercePrestige.Model.Entity
{
    public class PedidoProdutosModel:BaseModel
    {
        public int PedidoModelId { get; private set; }
        public virtual PedidoModel PedidoModel { get; private set; }
        public int ProdutoCorModelId { get; set; }
        public virtual ProdutoCorModel ProdutoCorModel { get; private set; }
        public int Quantidade { get; private set; }
        public double ValorUnitario { get; private set; }
        public double ValorTotal { get; private set; }

        public PedidoProdutosModel()
        {
        }

        public PedidoProdutosModel(int pedidoModelId, int produtoCorModelId, int quantidade, double valorUnitario, double valorTotal)
        {
            PedidoModelId = pedidoModelId;
            ProdutoCorModelId = produtoCorModelId;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            ValorTotal = valorTotal;
        }

        public void SetQuantidade(int quantidade)
        {
            Quantidade = quantidade;
        }

        public void SetValorTotal(double valorTotal)
        {
            ValorTotal = valorTotal;
        }

        public void SetProdutoCorModel(ProdutoCorModel produtoCorModel)
        {
            ProdutoCorModel = produtoCorModel;
        }
    }
}
