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
    public class KitAppServices:IKitAppServices
    {
        private readonly IKitsServices _kitsServices;
        private readonly IProdutoCorServices _produtoCorServices;
        private readonly IProdutoFotoServices _produtoFotoServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public KitAppServices(IKitsServices kitsServices, IProdutoCorServices produtoCorServices, IProdutoFotoServices produtoFotoServices, IUnitOfWork unitOfWork)
        {
            _kitsServices = kitsServices;
            _produtoCorServices = produtoCorServices;
            _produtoFotoServices = produtoFotoServices;
            _unitOfWork = unitOfWork;
            _mapper = AutoMapperConfig.Mapper;
        }
        public async Task<KitsViewModel> GetByIdAsync(int id)
        {
            return _mapper.Map<KitsViewModel>(await _kitsServices.GetByIdAsync(id));
        }
        public async Task<IEnumerable<KitsViewModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<KitsViewModel>>(await _kitsServices.GetAllAsync());
        }

        public async Task<IEnumerable<KitsViewModel>> FilterAsync(string termo)
        {
            return _mapper.Map<IEnumerable<KitsViewModel>>(await _kitsServices.FilterAsync(termo));
        }

        public async Task<KitsViewModel> GetKitsAsync(int id,string statusAtivacao)
        {
            var kitsViewModel = await GetByIdAsync(id).ConfigureAwait(false);

            var (listaCorModel,validacaoEstoque) = await _produtoCorServices.GetListByKitAsync(kitsViewModel.Nome,statusAtivacao);

            var (listKitProdutos, fotoPrincipal) = await BuildKitsProdutosViewModel(listaCorModel).ConfigureAwait(false);

            kitsViewModel.Produtos = listKitProdutos;
            kitsViewModel.fotoPrincipal = fotoPrincipal;

            kitsViewModel.StatusVenda = validacaoEstoque ? "Indisponivel" : "Em Estoque";
            
            return kitsViewModel;
        }

        public async Task UpdateAsync(KitsViewModel kitsViewModel)
        {
            _unitOfWork.BeginTransaction();
            await _kitsServices.UpdateAsync(_mapper.Map<KitModel>(kitsViewModel));
            await _unitOfWork.CommitAsync();
        }

        private async Task<(IEnumerable<KitsProdutoViewModel>, string uriPrincipal)> BuildKitsProdutosViewModel(IEnumerable<ProdutoCorModel> listaCorModel)
        {
            var listKitProdutos = new List<KitsProdutoViewModel>();
            var fotoPrincipal = new ProdutoFotoModel();

            foreach (var corModel in listaCorModel)
            {
                fotoPrincipal = await _produtoFotoServices.GetPrincipalByProdutoAsync(corModel.ProdutoModel.Id);

                var kitProdutosViewModel = new KitsProdutoViewModel
                {
                    Id = corModel.ProdutoModel.Id,
                    Descricao = corModel.ProdutoModel.Descricao,
                    UriFoto = fotoPrincipal.UriBlob,
                    Estoque = corModel.Estoque,
                    ImgCor = corModel.CorModel.ImgUrl,
                    Referencia = corModel.ProdutoModel.Referencia
                };

                listKitProdutos.Add(kitProdutosViewModel);
            }

            return (listKitProdutos, fotoPrincipal.UriBlob);

        }


    }
}
