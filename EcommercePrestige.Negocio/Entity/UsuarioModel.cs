namespace EcommercePrestige.Model.Entity
{
    public class UsuarioModel:BaseModel
    {
        public string UserId { get; private set; }
        public string NomeCompleto { get; private set; }
        public bool BloqueioAutomatico { get; private set; }
        public bool BloqueioManual { get; private set; }
        public bool Verificado { get; private set; }
        public double DescontoCliente { get; private set; }
        public bool Exibir { get; private set; }

        public void SetBloqueioAutomatico(bool bloqueioAutomatico)
        {
            BloqueioAutomatico = bloqueioAutomatico;
        }

        public void SetVerificado(bool verificado)
        {
            Verificado = verificado;
        }
    }
}
