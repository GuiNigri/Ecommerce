using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommercePrestige.Data.Repository
{
    public class ProdutoFotoRepository:BaseRepository<ProdutoFotoModel>, IProdutoFotoRepository
    {
        private readonly EcommerceContext _context;

        public ProdutoFotoRepository(EcommerceContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProdutoFotoModel>> GetByProdutoAsync(int id)
        {
            return await _context.ProdutosFotoModel.Where(x => x.ProdutoModelId == id).ToListAsync();
        }

        public async Task<ProdutoFotoModel> GetPrincipalByProdutoAsync(int id)
        {
            return await _context.ProdutosFotoModel.FirstOrDefaultAsync(x => x.ProdutoModelId == id && x.Principal);
        }

        public async Task<ProdutoFotoModel> GetFotosToKitAsync(int id)
        {
            var result = (from t in _context.ProdutosFotoModel
                where t.ProdutoModelId == id && t.Principal
                select new ProdutoFotoModel(
                    $"<img src= {t.UriBlob} title='{t.ProdutoModel.Referencia}' style='max-height: 377px;'>"));

            return result.SingleOrDefault();
        }

        public async Task<IEnumerable<ProdutoFotoModel>> GetFotosbyCorAsync(int idCor)
        {
            var result = await _context.ProdutosFotoModel
                .Where(x => x.ProdutoCorModelId == idCor).ToListAsync();

            return result;
        }

        public async Task<ProdutoFotoModel> GetFotosbyCorIndexAsync(int idCor)
        {
            var result = await _context.ProdutosFotoModel
                    .Where(x => x.ProdutoCorModelId == idCor).ToListAsync();

                return result.FirstOrDefault();

        }

        public async Task<IEnumerable<ProdutoFotoModel>> RetornarListaFotoInput(int idProd)
        {
            return _context.ProdutosFotoModel.Where(x => x.ProdutoModelId == idProd);
        }

        public async Task<bool> CheckFotoAndPrincipal(int idProd)
        {
            return await _context.ProdutosFotoModel.AnyAsync(x => x.ProdutoModelId == idProd && x.Principal);
        }

    }
}
