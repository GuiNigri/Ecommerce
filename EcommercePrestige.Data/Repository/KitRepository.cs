using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using X.PagedList;

namespace EcommercePrestige.Data.Repository
{
    public class KitRepository:BaseRepository<KitModel>,IKitsRepository
    {
        private readonly EcommerceContext _context;

        public KitRepository(EcommerceContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KitModel>> FilterAsync(string termo)
        {
            return await _context.KitsModel.Where(x => x.Nome.Contains(termo) || x.Descricao.Contains(termo))
                .ToListAsync();
        }
    }
}
