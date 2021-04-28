﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IBannersHomeRepository:IBaseRepository<BannersHomeModel>
    {
        Task DeleteAsync(BannersHomeModel bannersHomeModel);
    }
}
