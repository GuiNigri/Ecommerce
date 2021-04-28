namespace EcommercePrestige.Application.ViewModel
{
    public class UsuarioViewModel:BaseViewModel
    {
        public string UserId { get; set; }
        public string NomeCompleto { get; set; }
        public bool BloqueioAutomatico { get; set; }
        public bool BloqueioManual { get; set; }
        public bool Administrador { get; set; }
        public bool Verificado { get; set; }
        public double DescontoCliente { get; set; }
        public bool EmailConfirmado { get; set; }
        public bool Exibir { get; set; }

        public UsuarioViewModel()
        {
        }

        public UsuarioViewModel(string userId, string nomeCompleto, bool bloqueioAutomatico, bool bloqueioManual, bool administrador, bool verificado, double descontoCliente, bool exibir)
        {
            UserId = userId;
            NomeCompleto = nomeCompleto;
            BloqueioAutomatico = bloqueioAutomatico;
            BloqueioManual = bloqueioManual;
            Administrador = administrador;
            Verificado = verificado;
            DescontoCliente = descontoCliente;
            Exibir = exibir;
        }
    }
}
