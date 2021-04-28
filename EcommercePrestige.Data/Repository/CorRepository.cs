using System;
using System.Collections.Generic;
using System.Text;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;

namespace EcommercePrestige.Data.Repository
{
    public class CorRepository:BaseRepository<CorModel>, ICorRepository
    {
        public CorRepository(EcommerceContext context) : base(context)
        {
        }
    }
}
