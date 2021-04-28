using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;

namespace EcommercePrestige.Data.Repository
{
    public class BannersHomeRepository:BaseRepository<BannersHomeModel>, IBannersHomeRepository
    {
        private readonly EcommerceContext _context;

        public BannersHomeRepository(EcommerceContext context) : base(context)
        {
            _context = context;
        }

        public async Task DeleteAsync(BannersHomeModel bannersHomeModel)
        {
            _context.BannersHomeModel.Remove(bannersHomeModel);
        }
    }
}
