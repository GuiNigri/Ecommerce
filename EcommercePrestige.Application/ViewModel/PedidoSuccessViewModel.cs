namespace EcommercePrestige.Application.ViewModel
{
    public class PedidoSuccessViewModel
    {
        public string NumeroPedido { get; set; }

        public PedidoSuccessViewModel(string numeroPedido)
        {
            NumeroPedido = numeroPedido;
        }
    }

}
