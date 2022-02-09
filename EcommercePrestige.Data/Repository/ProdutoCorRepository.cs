using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using X.PagedList;

namespace EcommercePrestige.Data.Repository
{
    public class ProdutoCorRepository:BaseRepository<ProdutoCorModel>, IProdutoCorRepository
    {
        private readonly EcommerceContext _context;

        public ProdutoCorRepository(EcommerceContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProdutoCorModel>> GetAllGroupedAsync(string statusAtivacao)
        {
            

            var lista = await _context.ProdutosCorModel.Where(x => (statusAtivacao == null || x.StatusAtivacao == statusAtivacao))
                .AsEnumerable()
                .ToListAsync();

            lista = await MoreEnumerable.DistinctBy(lista, x => x.CorModelId).ToListAsync();

            return lista;
        }

        public async Task<IEnumerable<ProdutoCorModel>> GetByProdutoAsync(int id, string statusAtivacao)
        {
            return await _context.ProdutosCorModel.Where(x => x.ProdutoModelId == id && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao)).ToListAsync();
        }

        public async Task<IEnumerable<ProdutoCorModel>> GetListByKitAsync(string kitName, string statusAtivacao)
        {
            IEnumerable<ProdutoCorModel> listaFiltrada = kitName switch
            {
                "Gold" => await _context.ProdutosCorModel.Where(x => (x.PedidoGold) && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao)).ToListAsync(),
                "Silver" => await _context.ProdutosCorModel.Where(x => (x.PedidoSilver) && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao)).ToListAsync(),
                "Basic" => await _context.ProdutosCorModel.Where(x => (x.PedidoBasic) && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao)).ToListAsync(),
                _ => new List<ProdutoCorModel>()
            };

            return listaFiltrada;
        }

        public async Task<bool> VerificarEstoqueNegativo(IEnumerable<ProdutoCorModel> listCorModels)
        {
            return listCorModels.Any(x => x.Estoque <= 0);

        }

        public async Task<bool> VerificarEstoque(int prodId, int corId, int quantidade, string statusAtivacao)
        {
            var resposta = await _context.ProdutosCorModel.AnyAsync(x =>
                x.ProdutoModelId == prodId && x.CorModelId == corId && x.Estoque >= quantidade && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao));

            return resposta;
        }

        public async Task BaixaEstoque(int id, int quantidade)
        {
            var produtoCor = await _context.ProdutosCorModel.FirstOrDefaultAsync(x => x.Id == id);
            var newEstoque = produtoCor.Estoque + quantidade;
            produtoCor.SetEstoque(newEstoque);

            _context.Update(produtoCor);
            //await _context.SaveChangesAsync();
        }

        public async Task<ProdutoCorModel> GetCorByProdutoAsync(int corId, int prodId,string statusAtivacao)
        {
            return await _context.ProdutosCorModel.FirstOrDefaultAsync(x => x.CorModelId == corId && x.ProdutoModelId == prodId && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao));
        }

        public async Task<bool> VerificarSeCorExiste(int idProd, int idCor)
        {
            return await _context.ProdutosCorModel.AnyAsync(x => x.CorModelId == idCor && x.ProdutoModelId == idProd);
        }

        public override async Task CreateAsync(ProdutoCorModel produtoCorModel)
        {
            await _context.ProdutosCorModel.AddAsync(produtoCorModel);
            await _context.SaveChangesAsync();
        }

        public Task<ProdutoCorModel> ObterPeloCodigoBarrasAsync(string codigoBarras)
        {
            return _context.ProdutosCorModel
                .FirstOrDefaultAsync(x => x.CodigoBarras == codigoBarras);
        }

    }


}
