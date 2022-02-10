﻿using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ecommerce.Api.Authentication;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Api.Controllers.Usuario.Dto;

namespace Ecommerce.Api.Controllers.Usuario
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioController : ControllerBase
    {
        private readonly IUserAuthentication _userAuthentication;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenProvider _tokenProvider;

        private const string MensagemUsuarioSenhaInvalido = "Email ou senha Inválidos";

        public UsuarioController(IUserAuthentication userAuthentication, UserManager<IdentityUser> userManager, TokenProvider tokenProvider)
        {
            _userAuthentication = userAuthentication;
            _userManager = userManager;
            _tokenProvider = tokenProvider;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(UserAuthenticationModel user)
        {
            if (user is null)
                return BadRequest();

            var identityUser = await _userManager.FindByEmailAsync(user.email);

            var result = await _userAuthentication.IdentityAuthenticate(user.email, user.password, true);

            if (result is false)
                return Unauthorized(MensagemUsuarioSenhaInvalido);
            
            var (token,expiresDate) = _tokenProvider.GenerateJwtToken(identityUser);

            return Ok(new LoginResponse(status: true, token, expiresDate));
        }
    }
}