using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace EcommercePrestige.Data.Repository
{
    public class AviseMeRepository:BaseRepository<AviseMeModel>,IAviseMeRepository
    {
        private readonly EcommerceContext _context;

        public AviseMeRepository(EcommerceContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AviseMeModel>> VerificarReferenciasSolicitadas(int idReferencia)
        {
            return await _context.AviseMeModel.Where(x => x.ProdutoCorModelId == idReferencia).ToListAsync();
        }
    }
}
