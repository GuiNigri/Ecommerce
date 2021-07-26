using System;
using System.Collections.Generic;
using System.Text;

namespace EcommercePrestige.Model.Entity
{
    public class UserAuthenticationModel
    {
        public string id { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public UserAuthenticationModel(string id, string email, string password)
        {
            this.id = id;
            this.email = email;
            this.password = password;
        }
    }
}
