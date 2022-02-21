using System;

namespace Ecommerce.Api.Controllers.Usuario.Dto
{
    public class LoginResponse
    {
        public bool Status { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }

        public LoginResponse(bool status, string token, DateTime expires)
        {
            Status = status;
            Token = token;
            Expires = expires;
        }
    }
}
