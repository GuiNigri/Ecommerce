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
    public class ProdutoCorAppServices:IProdutoCorAppServices
    {
        private readonly IProdutoCorServices _domainService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICorService _corService;
        private readonly IMapper _mapper;

        public ProdutoCorAppServices(IProdutoCorServices domainService, IUnitOfWork unitOfWork, ICorService corService)
        {
            _domainService = domainService;
            _unitOfWork = unitOfWork;
            _corService = corService;
            _mapper = AutoMapperConfig.Mapper;
        }

        public async Task<IEnumerable<ProdutoCorViewModel>> GetByProdutoAsync(int id, string statusAtivacao)
        { 
            return _mapper.Map<IEnumerable<ProdutoCorViewModel>>(await _domainService.GetByProdutoAsync(id, statusAtivacao));

        }

        public async Task<IEnumerable<ProdutoCorViewModel>> GetAllGroupedAsync(string statusAtivacao)
        {
            return _mapper.Map<IEnumerable<ProdutoCorViewModel>>(await _domainService.GetAllGroupedAsync(statusAtivacao));

        }

        public async Task<bool> VerificarEstoque(int prodId, int idCor, int quantidade)
        {
            return await _domainService.VerificarEstoque(prodId, idCor, quantidade, "AT");
        }

        public async Task<(bool,string)> AddListaCor(ProdutoCorInputModel produtoCorInputModel)
        {
            var produtoCorModel = _mapper.Map<ProdutoCorModel>(produtoCorInputModel);
            produtoCorModel.SetStatusAtivacao("PE");

            return await _domainService.AddListaCor(produtoCorModel);
        }

        public async Task RemoveListaCor(int id)
        {
            _unitOfWork.BeginTransaction();
            await _domainService.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ProdutoCorInputModel>> RetornarListaDeCorDoProduto(int id, string statusAtivacao)
        {
            var produtoCorModel = await _domainService.RetornarListaDeCorDoProduto(id, statusAtivacao);

            
            var lista = new List<ProdutoCorInputModel>();

            foreach (var item in produtoCorModel)
            {
                CorModel corModel;

                if (item.CorModel == null)
                {
                    corModel = await _corService.GetByIdAsync(item.CorModelId);
                }
                else
                {
                    corModel = item.CorModel;
                }

                var produtoCorInputModel = new ProdutoCorInputModel
                {
                    Id = item.Id,
                    CorId = item.CorModelId,
                    DescricaoCor = corModel.Descricao,
                    ImgCor = corModel.ImgUrl,
                    CodigoInterno = item.CodigoInterno,
                    Estoque = item.Estoque,
                    Gold = item.PedidoGold,
                    Silver = item.PedidoSilver,
                    Basic = item.PedidoBasic,
                    ProdutoId = item.ProdutoModelId,
                    StatusAtivacao = item.StatusAtivacao,
                    CodigoBarras = item.CodigoBarras
                };

                lista.Add(produtoCorInputModel);
            }

            return lista;
        }

        public async Task UpdateAsync(int id)
        {
            _unitOfWork.BeginTransaction();
            await _domainService.UpdateAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task AlterarEstoqueAsync(int id, int quantidade)
        {
            _unitOfWork.BeginTransaction();
            await _domainService.AlterarEstoqueAsync(id,quantidade);
            await _unitOfWork.CommitAsync();
        }

        public async Task AlterarCodigoBarrasAsync(int id, string codigoBarras)
        {
            _unitOfWork.BeginTransaction();
            await _domainService.AlterarCodigoBarrasAsync(id, codigoBarras);
            await _unitOfWork.CommitAsync();
        }

        public async Task AlterarAtivacaoKitNoProduto(int idCor, string kit)
        {
            _unitOfWork.BeginTransaction();
            await _domainService.AlterarAtivacaoKitNoProduto(idCor,kit);
            await _unitOfWork.CommitAsync();
        }
    }
}
