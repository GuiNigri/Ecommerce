using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommercePrestige.Data.Repository
{
    public class MaterialRepository:BaseRepository<MaterialModel>, IMaterialRepository
    {
        private readonly EcommerceContext _context;

        public MaterialRepository(EcommerceContext context) : base(context)
        {
            _context = context;
        }

        public async Task<MaterialModel> GetByNameAsync(string name)
        {
            return await _context.MaterialModel.FirstOrDefaultAsync(x => x.Material == name);
        }

        public async Task<IEnumerable<MaterialModel>> FilterAsync(string termo)
        {
            return await _context.MaterialModel.Where(x => x.Material.Contains(termo)).ToListAsync();
        }
    }
}
