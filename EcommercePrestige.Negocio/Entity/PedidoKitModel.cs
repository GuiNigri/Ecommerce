namespace EcommercePrestige.Model.Entity
{
    public class PedidoKitModel:BaseModel
    {
        public int PedidoModelId { get; private set; }
        public virtual PedidoModel PedidoModel { get; private set; }
        public int KitModelId { get; private set; }
        public virtual KitModel KitModel { get; private set; }
        public int Quantidade { get; private set; }
        public double ValorUnitario { get; private set; }
        public double ValorTotal { get; private set; }

        public PedidoKitModel() {}

        public PedidoKitModel(int pedidoModelId, int kitModelId, int quantidade, double valorUnitario, double valorTotal)
        {
            PedidoModelId = pedidoModelId;
            KitModelId = kitModelId;
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

    }
}
