using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;


namespace EcommercePrestige.Services
{
    public class ProdutoServices:BaseServices<ProdutoModel>,IProdutoServices
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoCorRepository _produtoCorRepository;

        public ProdutoServices(IProdutoRepository produtoRepository, IProdutoCorRepository produtoCorRepository) : base(produtoRepository)
        {
            _produtoRepository = produtoRepository;
            _produtoCorRepository = produtoCorRepository;
        }

        public async Task<ProdutoModel> GetProductByIdAndCor(int idProd, int corId, string statusAtivacao)
        {
            return await _produtoRepository.GetProductByIdAndCor(idProd, corId, statusAtivacao);
        }

        public async Task<IEnumerable<ProdutoModel>> FilterAndOrderListAsync(IEnumerable<ProdutoModel> listaProdutos, FiltroModel filtroModel, string statusAtivacao)
        {
            IEnumerable<ProdutoModel> listaFiltradaOrdenada;

            var listaProdutosModels = listaProdutos.ToList();

            if (listaProdutosModels.Any())
            {
                listaFiltradaOrdenada = await _produtoRepository.GetFilterAsync(listaProdutosModels, filtroModel, statusAtivacao);
            }
            else
            {
                listaFiltradaOrdenada = await _produtoRepository.GetFilterAsync(new List<ProdutoModel>(), filtroModel, statusAtivacao);
            }
            


            if (filtroModel.OrderType != null)
            {
                return await _produtoRepository.OrderListAsync(listaFiltradaOrdenada, filtroModel.OrderType);
            }

            return listaFiltradaOrdenada;
        }

        public async Task<IEnumerable<ProdutoModel>> GetListByCategoryAsync(FiltroModel filtroModel, string statusAtivacao)
        {
            IEnumerable<ProdutoModel> lista;

            if (filtroModel.Category == "offline")
            {
                lista = await _produtoRepository.GetOfflineProducts(statusAtivacao);
            }
            else
            {
                lista = await _produtoRepository.GetListByCategoryAsync(filtroModel.Category, 0,statusAtivacao);
            }
            

            if (!lista.Any())
            {
                return lista;
            }

            return await FilterAndOrderListAsync(lista, filtroModel, statusAtivacao).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ProdutoModel>> GetListByTermoAsync(FiltroModel filtroModel, string statusAtivacao)
        {
            IEnumerable<ProdutoModel> lista;

            lista = await _produtoRepository.GetFilterByTermoAsync(filtroModel.Termo,statusAtivacao);
            

            if (!lista.Any())
            {
                return lista;
            }

            return await FilterAndOrderListAsync(lista, filtroModel, statusAtivacao).ConfigureAwait(false);
        }

        public async Task<(int,bool,string)> CreateProdutoReturningIdAsync(ProdutoModel produtoModel)
        {
            var verificacao = await _produtoRepository.VerificarReferencia(produtoModel.Referencia);

            if (!verificacao)
            {
                var id = await _produtoRepository.CreateProdutoReturningIdAsync(produtoModel);
                return (id, true, null);
            }

            return (0, false, "A referencia já existe em nosso sistema");
        }

        public async Task<IEnumerable<ProdutoModel>> GetAllAsync(string statusAtivacao)
        {
            return await _produtoRepository.GetAllAsync(statusAtivacao);
        }

        public async Task<(bool, string)> UpdateProductAsync(ProdutoModel produtoModel, string novaReferencia)
        {
            if (produtoModel.Referencia != novaReferencia)
            {
                var verificacao = await _produtoRepository.VerificarReferencia(novaReferencia);

                if (!verificacao)
                {
                    produtoModel.SetReferencia(novaReferencia);
                    await _produtoRepository.UpdateAsync(produtoModel);
                    return (true, null);
                }

                return (false, "A referencia já existe em nosso sistema");
            }

            await _produtoRepository.UpdateAsync(produtoModel);
            return (true, null);
        }

        public async Task AlterarStatusAtivacaoAsync(int id)
        {
            var produtoModel = await _produtoRepository.GetByIdAsync(id);

            var status = produtoModel.StatusAtivacao == "AT" ? "IN" : "AT";
            produtoModel.SetStatusAtivacao(status);

            await _produtoRepository.UpdateAsync(produtoModel);

            var produtoCorList = await _produtoCorRepository.GetByProdutoAsync(id,null);

            foreach (var item in produtoCorList)
            {
                item.SetStatusAtivacao(status);
                await _produtoCorRepository.UpdateAsync(item);
            }
        }

        public async Task<IEnumerable<ProdutoModel>> FilterBarraPesquisar(string termo)
        {
            return await _produtoRepository.FilterBarraPesquisar(termo);
        }

        public async Task<IEnumerable<ProdutoModel>> GetCategoryForHome(string category, int marcaId)
        {
            return await _produtoRepository.GetListByCategoryAsync(category, marcaId,"AT");
        }

        public async Task AtualizarEstoqueMassa(int quantidade)
        {
            await _produtoRepository.AtualizarEstoqueMassa(quantidade);
        }
    }
}
