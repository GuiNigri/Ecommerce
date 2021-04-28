using System;
using System.Collections.Generic;
using System.Text;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Services
{
    public class TextoHomeServices:BaseServices<TextoHomeModel>, ITextoHomeServices
    {
        public TextoHomeServices(ITextoHomeRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
