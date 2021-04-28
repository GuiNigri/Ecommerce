using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcommercePrestige.Application.AutoMapper;
using EcommercePrestige.Application.ViewModel;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Services;
using EcommercePrestige.Model.Interfaces.UoW;
using Microsoft.AspNetCore.Http;

namespace EcommercePrestige.Application.AppServices.Implementations
{
    public class ProdutoFotoAppServices : IProdutoFotoAppServices
    {
        private readonly IProdutoFotoServices _domainService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProdutoFotoAppServices(IProdutoFotoServices domainService, IUnitOfWork unitOfWork)
        {
            _domainService = domainService;
            _unitOfWork = unitOfWork;
            _mapper = AutoMapperConfig.Mapper;
        }

        public async Task<IEnumerable<ProdutoFotoViewModel>> GetByProdutoAsync(int id)
        {
            return _mapper.Map<IEnumerable<ProdutoFotoViewModel>>(await _domainService.GetByProdutoAsync(id));
        }

        public async Task<ProdutoFotoViewModel> GetFotosToKitAsync(int id)
        {
            return _mapper.Map<ProdutoFotoViewModel>(await _domainService.GetFotosToKitAsync(id));
        }

        public async Task<IEnumerable<ProdutoFotoViewModel>> GetFotosByCorAsync(int id)
        {
            return _mapper.Map<IEnumerable<ProdutoFotoViewModel>>(await _domainService.GetFotosbyCorAsync(id));
        }

        public async Task<ProdutoFotoViewModel> GetFotosByCorIndexAsync(int id)
        {
            return _mapper.Map<ProdutoFotoViewModel>(await _domainService.GetFotosbyCorIndexAsync(id));
        }

        public async Task CreateAsync(int idProduto, string statusAtivacao)
        {
            _unitOfWork.BeginTransaction();
            await _domainService.CreateAsync(idProduto, statusAtivacao);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> CheckFotoAndPrincipal(int idProd)
        {
            return await _domainService.CheckFotoAndPrincipal(idProd);
        }

        public async Task<(bool, string)> AddFotoLista(IFormFile file, ProdutoFotoInputModel produtoFotoInputModel,
            string statusAtivacao)
        {
            bool status;
            string mensagem;

            var produtoFotoModel = _mapper.Map<ProdutoFotoModel>(produtoFotoInputModel);
            produtoFotoModel.SetStatusAtivacao("PE");
            var corId = produtoFotoInputModel.CorId;

            _unitOfWork.BeginTransaction();
            (status, mensagem) =
                await _domainService.AddFotoList(file.OpenReadStream(), corId, produtoFotoModel, statusAtivacao);
            await _unitOfWork.CommitAsync();

            return (status, mensagem);
        }

        public async Task RemoveListaFoto(int id)
        {
            _unitOfWork.BeginTransaction();
            await _domainService.RemoveFotoList(id);
            await _unitOfWork.CommitAsync();

        }

        public async Task<IEnumerable<ProdutoFotoInputModel>> RetornarListaFotoInput(int idProd)
        {
            var lista = await _domainService.RetornarListaFotoInput(idProd);

            var listaInput = new List<ProdutoFotoInputModel>();

            foreach (var item in lista)
            {
                var produtoFotoInputModel = new ProdutoFotoInputModel
                {
                    Id = item.Id,
                    CorId = item.ProdutoCorModel.CorModelId,
                    ImgCor = item.ProdutoCorModel.CorModel.ImgUrl,
                    Foto = item.UriBlob,
                    Principal = item.Principal,
                    ProdutoId = item.ProdutoModelId,
                    StatusAtivacao = item.StatusAtivacao
                };

                listaInput.Add(produtoFotoInputModel);
            }


            return listaInput;
        }

        public async Task UpdateAsync(int idFoto)
        {
            _unitOfWork.BeginTransaction();
            await _domainService.UpdateAsync(idFoto);
            await _unitOfWork.CommitAsync();
        }
    }
}
