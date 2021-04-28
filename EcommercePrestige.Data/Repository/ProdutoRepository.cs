using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace EcommercePrestige.Data.Repository
{
    public class ProdutoRepository:BaseRepository<ProdutoModel>,IProdutoRepository
    {
        private readonly EcommerceContext _context;

        public ProdutoRepository(EcommerceContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProdutoModel>> GetFilterAsync(IEnumerable<ProdutoModel> lista, FiltroModel filtroModel, string statusAtivacao)
        {
            var produtosModels = await lista.ToListAsync();

            if (produtosModels.Any())
            {
                return await Filter(produtosModels, filtroModel, statusAtivacao).OrderBy(y => y.Referencia).ToListAsync();
            }

            return await Filter(_context.ProdutosModel, filtroModel, statusAtivacao).OrderBy(y => y.Referencia).ToListAsync();
        }

        private IEnumerable<ProdutoModel> Filter(IEnumerable<ProdutoModel> lista, FiltroModel filtroModel, string statusAtivacao)
        {
            List<ProdutoModel> listaFiltrada;

            var produtosModels = lista.ToList();

            if (filtroModel.CorOption > 0)
            {
                listaFiltrada = (from produtos in produtosModels
                    join cor in _context.ProdutosCorModel on produtos.Id equals cor.ProdutoModelId
                    where (filtroModel.MarcaOption == 0 || produtos.MarcaModelId == filtroModel.MarcaOption) &&
                          (filtroModel.GeneroOption == null || produtos.Genero == filtroModel.GeneroOption) &&
                          (filtroModel.MaterialOption == 0 || produtos.MaterialModelId == filtroModel.MaterialOption) &&
                          cor.CorModelId == filtroModel.CorOption &&
                          (statusAtivacao == null || produtos.StatusAtivacao == statusAtivacao)
                    select new ProdutoModel(
                        produtos.Id,
                        produtos.MarcaModelId,
                        produtos.MarcaModel,
                        produtos.MaterialModelId,
                        produtos.MaterialModel,
                        produtos.Referencia,
                        produtos.Tamanho,
                        produtos.Descricao,
                        produtos.ValorVenda,
                        produtos.StatusProduto,
                        produtos.Genero
                        )).ToList();
            }
            else
            {
                listaFiltrada = produtosModels
                    .Where(x => (filtroModel.MarcaOption == 0 || x.MarcaModelId == filtroModel.MarcaOption) && (filtroModel.GeneroOption == null || x.Genero == filtroModel.GeneroOption) &&
                                (filtroModel.MaterialOption == 0 || x.MaterialModelId == filtroModel.MaterialOption) && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao)).ToList();
            }

            return listaFiltrada;
        }

        public async Task<IEnumerable<ProdutoModel>> OrderListAsync(IEnumerable<ProdutoModel> lista, string orderType)
        {
            var listaOrdenada = await lista.ToListAsync();

            listaOrdenada = orderType switch
            {
                "alphabetical" => await listaOrdenada.OrderBy(x => x.Referencia).ToListAsync(),
                "new" => await listaOrdenada.OrderByDescending(x => x.StatusProduto == "new").ToListAsync(),
                "lowPrice" => await listaOrdenada.OrderBy(x => x.ValorVenda).ToListAsync(),
                "highPrice" => await listaOrdenada.OrderByDescending(x => x.ValorVenda).ToListAsync(),
                _ => listaOrdenada
            };

            return listaOrdenada;
        }


        public async Task<IEnumerable<ProdutoModel>> GetListByCategoryAsync(string category, int marcaId, string statusAtivacao)
        {
            var lista = category switch
            {
                "bestSeller" => await _context.ProdutosModel.Where(x => x.BestSeller && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao) && (marcaId <= 0 || x.MarcaModel.Id == marcaId)).ToListAsync(),
                "sale" => await _context.ProdutosModel.Where(x => x.Liquidacao && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao)).ToListAsync(),
                "vitrine" => await _context.ProdutosModel.Where(x => x.Vitrine && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao)).ToListAsync(),
                "lancamento" => await _context.ProdutosModel.Where(x => x.StatusProduto == "new" && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao)).ToListAsync(),
                _ => await _context.ProdutosModel.ToListAsync()
            };

            return lista;
        }

        public async Task<IEnumerable<ProdutoModel>> GetFilterByTermoAsync(string termo, string statusAtivacao)
        {
            var lista = await _context.ProdutosModel.Where(x =>
                (x.Descricao.Contains(termo) || x.Referencia.Contains(termo) || x.Genero.Contains(termo) ||
                x.MarcaModel.Nome.Contains(termo) ||
                x.MaterialModel.Material.Contains(termo) || x.Tamanho.Contains(termo) ||
                x.ValorVenda.ToString().Contains(termo)) && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao)).OrderBy(y => y.Referencia).ToListAsync();

            return lista;
        }

        public async Task<IEnumerable<ProdutoModel>> GetOfflineProducts(string statusAtivacao)
        {
            return await _context.ProdutosModel.Where(x => x.StatusProduto == "offline" && (statusAtivacao == null || x.StatusAtivacao == statusAtivacao)).ToListAsync();
        }

        public async Task<ProdutoModel> GetProductByIdAndCor(int idProd, int corId, string statusAtivacao)
        {

            var produtosModel = from produtos in _context.ProdutosModel
                join cor in _context.ProdutosCorModel on produtos.Id equals cor.ProdutoModelId
                where produtos.Id == idProd && cor.CorModelId == corId &&
                      (statusAtivacao == null || produtos.StatusAtivacao == statusAtivacao)
                select new ProdutoModel(
                    produtos.Id,
                    produtos.MarcaModelId,
                    produtos.MarcaModel,
                    produtos.MaterialModelId,
                    produtos.MaterialModel,
                    produtos.Referencia,
                    produtos.Tamanho,
                    produtos.Descricao,
                    produtos.ValorVenda,
                    produtos.StatusProduto,
                    produtos.Genero
                );

            return await produtosModel.SingleOrDefaultAsync();
        }

        public async Task<int> CreateProdutoReturningIdAsync(ProdutoModel produtoModel)
        {
            var produtoAdicionado = await _context.ProdutosModel.AddAsync(produtoModel);
            await _context.SaveChangesAsync();

            return produtoAdicionado.Entity.Id;
        }

        public async Task<IEnumerable<ProdutoModel>> GetAllAsync(string statusAtivacao)
        {
            return await _context.ProdutosModel.Where(x => (statusAtivacao == null || x.StatusAtivacao == statusAtivacao)).ToListAsync();
        }

        public async Task<bool> VerificarReferencia(string referencia)
        {
            return await _context.ProdutosModel.AnyAsync(x => x.Referencia == referencia);
        }

        public async Task<IEnumerable<ProdutoModel>> FilterBarraPesquisar(string termo)
        {
            return await _context.ProdutosModel.Where(x =>
                    x.Referencia.Contains(termo) || x.MarcaModel.Nome.Contains(termo) ||
                    x.MaterialModel.Material.Contains(termo) || x.Genero.Contains(termo) || x.Descricao.Contains(termo)).OrderBy(y => y.Referencia)
                .ToListAsync();
        }

        public async Task AtualizarEstoqueMassa(int quantidade)
        {
            await _context.ProdutosCorModel.ForEachAsync(x=>x.SetEstoque(quantidade));
            await _context.SaveChangesAsync();
        }
    }
}
