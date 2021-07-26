using System.Threading.Tasks;

namespace EcommercePrestige.Model.Interfaces.Authentication
{
    public interface IUserAuthentication
    {
        Task<bool> IdentityAuthenticate(string email, string password, bool rememberMe);
    }
}
