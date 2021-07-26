using System.Threading.Tasks;
using EcommercePrestige.Model.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Api.Authentication
{
    public class UserAuthentication:IUserAuthentication
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserAuthentication(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<bool> IdentityAuthenticate(string email, string password, bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

            return result.Succeeded;
        }
    }
}
