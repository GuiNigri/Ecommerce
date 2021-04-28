using System.Threading.Tasks;

namespace EcommercePrestige.Application.ViewModel
{
    public class UsuarioEmpresaAggregateViewModel:BaseViewModel
    {
        public UsuarioViewModel Usuario { get; set; }
        public EmpresaViewModel Empresa { get; set; }
        public bool EmailConfirmado { get; set; }
        public string Email { get; set; }
        public string NewEmail { get; set; }

        
        public UsuarioEmpresaAggregateViewModel()
        {
        }
        public UsuarioEmpresaAggregateViewModel(UsuarioViewModel usuario, EmpresaViewModel empresa, string email,bool emailConfirmado,string status)
        {
            Usuario = usuario;
            Empresa = empresa;
            StatusModel = status;
            Email = email;
            NewEmail = email;
            EmailConfirmado = emailConfirmado;
        }
    }
}
