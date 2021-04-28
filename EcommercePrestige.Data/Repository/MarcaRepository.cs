using EcommercePrestige.Model.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommercePrestige.Data.Repository
{
    public class MarcaRepository:BaseRepository<MarcaModel>, IMarcaRepository
    {
        private readonly EcommerceContext _context;

        public MarcaRepository(EcommerceContext context):base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<MarcaModel>> GetAllAsync()
        {
            return await _context.MarcaModel.OrderBy(x=>x.Nome).ToListAsync();
        }

        public async Task<MarcaModel> GetByNameAsync(string name)
        {
            return await _context.MarcaModel.FirstOrDefaultAsync(x => x.Nome == name);
        }
        
        public async Task<IEnumerable<MarcaModel>> Filter(string termo)
        {
            return await _context.MarcaModel.Where(x => x.Nome.Contains(termo)).ToListAsync();
        }

        public async Task<bool> VerificarMarca(string marca)
        {
            return await _context.MarcaModel.AnyAsync(x => x.Nome == marca);
        }
    }
}
