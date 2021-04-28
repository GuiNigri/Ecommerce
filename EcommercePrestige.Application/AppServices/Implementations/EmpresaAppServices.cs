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
    public class EmpresaAppServices:IEmpresaAppServices
    {
        private readonly IEmpresaServices _empresaServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public EmpresaAppServices(IEmpresaServices empresaServices, IUnitOfWork unitOfWork)
        {
            _empresaServices = empresaServices;
            _unitOfWork = unitOfWork;
            _mapper = AutoMapperConfig.Mapper;
        }

        public async Task Create(EmpresaViewModel empresaViewModel, UsuarioViewModel usuarioViewModel)
        {
            _unitOfWork.BeginTransaction();
            await _empresaServices.CreateAsync(_mapper.Map<EmpresaModel>(empresaViewModel), _mapper.Map<UsuarioModel>(usuarioViewModel));
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.BeginTransaction();
            await _empresaServices.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<CidadesViewModel>> GetCidadesEmpresasAsync()
        {
            var listaCidades = await _empresaServices.GetCidadesEmpresasAsync();

            return _mapper.Map<IEnumerable<CidadesViewModel>>(listaCidades);
        }

        public async Task<IEnumerable<object>> GetBairrosByCidadesAsync(CidadesViewModel cidadesViewModel)
        {
            var listBairros = await _empresaServices.GetBairrosByCidadesAsync(_mapper.Map<CidadesModel>(cidadesViewModel));

            return listBairros;
        }

        public async Task<(EmpresaViewModel,string)> GetDadosEmpresaAsync(string cnpj)
        {
            var empresaModel = await _empresaServices.GetDadosEmpresaAsync(cnpj);

            if (empresaModel.Status != "OK") return (null, empresaModel.Message);

            var empresaConvertida = _mapper.Map<EmpresaViewModel>(empresaModel);

            return (empresaConvertida, null);

        }

        public async Task<IEnumerable<EmpresaViewModel>> GetListaDeEmpresasByCidadesEBairro(string cidade, string bairro)
        {
            return _mapper.Map<IEnumerable<EmpresaViewModel>>(await _empresaServices.GetListaDeEmpresasByCidadesEBairro(cidade, bairro));

        }

        public async Task<EmpresaViewModel> GetEmpresaByCnpj(string cnpj)
        {
            return _mapper.Map<EmpresaViewModel>(await _empresaServices.GetEmpresaByCnpj(cnpj));
        }

        public async Task<EmpresaViewModel> GetEmpresaByUserId(string userId)
        {
            return _mapper.Map<EmpresaViewModel>(await _empresaServices.GetEmpresaByUserId(userId));
        }

        public async Task<IEnumerable<EmpresaViewModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<EmpresaViewModel>>(await _empresaServices.GetAllAsync());
        }

        public async Task<IEnumerable<EmpresaViewModel>> FilterAsync(string termo)
        {
            return _mapper.Map<IEnumerable<EmpresaViewModel>>(await _empresaServices.FilterAsync(termo));
        }

        public async Task<EmpresaViewModel> GetByIdAsync(int id)
        {
            return _mapper.Map<EmpresaViewModel>(await _empresaServices.GetByIdAsync(id));
        }

        public async Task UpdateAsync(EmpresaViewModel empresaViewModel)
        {
            _unitOfWork.BeginTransaction();
            await _empresaServices.UpdateAsync(_mapper.Map<EmpresaModel>(empresaViewModel));
            await _unitOfWork.CommitAsync();
        }
    }
}
