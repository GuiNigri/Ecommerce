using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ecommerce.Api.Authentication;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioController : ControllerBase
    {
        private readonly IUserAuthentication _userAuthentication;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenProvider _tokenProvider;

        public UsuarioController(IUserAuthentication userAuthentication, UserManager<IdentityUser> userManager, TokenProvider tokenProvider)
        {
            _userAuthentication = userAuthentication;
            _userManager = userManager;
            _tokenProvider = tokenProvider;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Login(UserAuthenticationModel user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.email);

            var result = await _userAuthentication.IdentityAuthenticate(user.email, user.password, true);

            if (!result)
            {
                return Unauthorized(new {message = "Email ou senha Inválidos"});
            }

            var (token,expiresDate) = _tokenProvider.GenerateJwtToken(identityUser);


            return new
            {
                status = true,
                token,
                expires = expiresDate

            };
        }

        [HttpGet]
        [Route("pegar")]
        [Authorize]
        public async Task<ActionResult<string>> Test()
        {
            return "funcionou";
        }
    }
}
