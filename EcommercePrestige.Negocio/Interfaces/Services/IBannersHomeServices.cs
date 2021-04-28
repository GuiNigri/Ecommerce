using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface IBannersHomeServices:IBaseServices<BannersHomeModel>
    {
        Task CreateAsync(Stream banner);
    }
}
