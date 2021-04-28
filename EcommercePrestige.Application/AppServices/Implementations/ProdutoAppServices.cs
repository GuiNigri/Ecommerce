using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcommercePrestige.Application.AutoMapper;
using EcommercePrestige.Application.ViewModel;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Services;
using EcommercePrestige.Model.Interfaces.UoW;

namespace EcommercePrestige.Application.AppServices.Implementations
{
    public class ProdutoAppServices:IProdutoAppServices
    {
        private readonly IProdutoServices _domainService;
        private readonly IProdutoFotoServices _fotoServices;
        private readonly IMarcaServices _marcaServices;
        private readonly IMaterialServices _materialServices;
        private readonly IAviseMeServices _aviseMeServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProdutoAppServices(IProdutoServices domainService, IProdutoFotoServices fotoServices, IMarcaServices marcaServices, IMaterialServices materialServices, IAviseMeServices aviseMeServices,IUnitOfWork unitOfWork)
        {
            _domainService = domainService;
            _fotoServices = fotoServices;
            _marcaServices = marcaServices;
            _materialServices = materialServices;
            _aviseMeServices = aviseMeServices;
            _unitOfWork = unitOfWork;
            _mapper = AutoMapperConfig.Mapper;
        }


        public async Task<(int, bool, string)> CreateProdutoAsync(ProdutoCreateEtapaBasicaModel produtoCreateEtapaBasicaModel)
        {
            var produtoModel = _mapper.Map<ProdutoModel>(produtoCreateEtapaBasicaModel);
            produtoModel.SetStatusAtivacao("PE");

            return await _domainService.CreateProdutoReturningIdAsync(produtoModel);
        }

        public async Task<(bool,string)> UpdateAsync(ProdutoCreateEtapaBasicaModel produtoCreateEtapaBasicaModel,string novaReferencia)
        {
            bool status;
            string mensagem;

            var produtoModel = _mapper.Map<ProdutoModel>(produtoCreateEtapaBasicaModel);

            _unitOfWork.BeginTransaction();
            (status,mensagem)=await _domainService.UpdateProductAsync(produtoModel, novaReferencia);
            await _unitOfWork.CommitAsync();

            return (status,mensagem);
        }

        public async Task<IEnumerable<ProdutoViewModel>> FilterAndOrderAsync(FiltroProdutoViewModel filtroViewModel, string statusAtivacao)
        {
            return await ToListViewModel(await _domainService.FilterAndOrderListAsync(new List<ProdutoModel>(), await ToFilterModel(filtroViewModel).ConfigureAwait(false), statusAtivacao).ConfigureAwait(false));
        }

        public async Task<IEnumerable<ProdutoViewModel>> GetCategoryAsync(FiltroProdutoViewModel filtroViewModel, string statusAtivacao)
        {
            return await ToListViewModel(
                await _domainService.GetListByCategoryAsync(
                await ToFilterModel(filtroViewModel).ConfigureAwait(false), statusAtivacao)).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ProdutoViewModel>> GetFilterTermoAsync(FiltroProdutoViewModel filtroViewModel, string statusAtivacao)
        {
            return await ToListViewModel(
                await _domainService.GetListByTermoAsync(
                    await ToFilterModel(filtroViewModel).ConfigureAwait(false), statusAtivacao)).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ProdutoViewModel>> GetAllAsync(string statusAtivacao)
        {
            var listModel = await _domainService.GetAllAsync(statusAtivacao);

            return await ToListViewModel(listModel).ConfigureAwait(false);
        }

        public async Task<ProdutoViewModel> GetByIdAsync(int id)
        {
            return _mapper.Map<ProdutoViewModel>(await _domainService.GetByIdAsync(id));
        }

        public async Task<ProdutoViewModel> GetProductByIdAndCor(int idProd, int corId, string statusAtivacao)
        {
            var produtoFotoModel = await _fotoServices.GetPrincipalByProdutoAsync(idProd);

            var produtoViewModel = _mapper.Map<ProdutoViewModel>(await _domainService.GetProductByIdAndCor(idProd, corId, statusAtivacao));

            if (produtoViewModel != null)
            {
                produtoViewModel.UriFoto = produtoFotoModel.UriBlob;

                return produtoViewModel;
            }

            return null;
        }

        public async Task AlterarStatusAtivacaoProduto(int idProduto)
        {
            _unitOfWork.BeginTransaction();
            await _domainService.AlterarStatusAtivacaoAsync(idProduto);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ProdutoViewModel>> FilterBarraPesquisar(string termo)
        {
            var listModel = await _domainService.FilterBarraPesquisar(termo);

            return await ToListViewModel(listModel).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ProdutoViewModel>> GetCategoryForHomeAsync(string category, int marcaId)
        {
            var listModel = await _domainService.GetCategoryForHome(category, marcaId);

            return await ToListViewModel(listModel).ConfigureAwait(false);
        }

        public async Task CadastrarAviseMeAsync(int corId, string email)
        {
            var aviseMeModel = new AviseMeModel(email, corId);

            _unitOfWork.BeginTransaction();
            await _aviseMeServices.CreateAsync(aviseMeModel);
            await _unitOfWork.CommitAsync();

        }

        public async Task<IEnumerable<AviseMeViewModel>> GetAviseMe()
        {
            return _mapper.Map<IEnumerable<AviseMeViewModel>>(await _aviseMeServices.GetAllAsync());
        }

        public async Task AtualizarEstoqueMassa(int quantidade)
        {
            await _domainService.AtualizarEstoqueMassa(quantidade);
        }

        private async Task<IEnumerable<ProdutoViewModel>> ToListViewModel(IEnumerable<ProdutoModel> listModel)
        {
            var listViewModel = new List<ProdutoViewModel>();

            foreach (var model in listModel)
            {
                var fotoPrincipal = await _fotoServices.GetPrincipalByProdutoAsync(model.Id);
                listViewModel.Add(fotoPrincipal != null
                    ? new ProdutoViewModel(fotoPrincipal.UriBlob, model)
                    : new ProdutoViewModel(null, model));
            }

            return listViewModel;
        }

        private async Task<FiltroModel> ToFilterModel(FiltroProdutoViewModel filtroViewModel)
        {
            var filtroModel = _mapper.Map<FiltroModel>(filtroViewModel);

            if (filtroViewModel.MarcaOption != null)
            {
                var marcaModel = await _marcaServices.GetByNameAsync(filtroViewModel.MarcaOption);
                filtroModel.SetMarcaOption(marcaModel.Id);
            }

            if (filtroViewModel.MaterialOption != null)
            {
                var materialModel = await _materialServices.GetByNameAsync(filtroViewModel.MaterialOption);
                filtroModel.SetMaterialOption(materialModel.Id);
            }

            return filtroModel;
        }

    }
}
