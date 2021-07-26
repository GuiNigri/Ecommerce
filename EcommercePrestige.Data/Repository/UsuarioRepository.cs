using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommercePrestige.Data.Repository
{
    public class UsuarioRepository:BaseRepository<UsuarioModel>,IUsuarioRepository
    {
        private readonly EcommerceContext _context;

        public UsuarioRepository(EcommerceContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UsuarioModel> GetByUserIdAsync(string userId)
        {
            return await _context.UsuarioModel.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<UsuarioModel>> Filter(string termo, bool pendente, bool bloqueado)
        {
            return await _context.UsuarioModel.Where(x => (termo == null || x.NomeCompleto.Contains(termo)) && (pendente == false || x.BloqueioAutomatico && !x.Verificado) && (bloqueado == false || x.BloqueioManual)).ToListAsync();
        }
        public async Task<IEnumerable<UsuarioModel>> GetPendentesAsync()
        {
            return await _context.UsuarioModel.Where(x => x.BloqueioAutomatico && !x.Verificado).ToListAsync();
        }

        public async Task<IEnumerable<UsuarioModel>> GetBloqueadosAsync()
        {
            return await _context.UsuarioModel.Where(x => x.BloqueioManual).ToListAsync();
        }
    }
}
