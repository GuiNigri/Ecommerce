namespace EcommercePrestige.Application.ViewModel
{
    public class UsuarioCreateViewModel:BaseViewModel
    {
        public UsuarioViewModel Usuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Celular { get; set; }
    }
}
