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
    public class UsuarioAppServices:IUsuarioAppServices
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioAppServices(IUsuarioServices usuarioServices, IUnitOfWork unitOfWork)
        {
            _usuarioServices = usuarioServices;
            _unitOfWork = unitOfWork;
            _mapper = AutoMapperConfig.Mapper;
        }

        public async Task<UsuarioViewModel> GetByUserIdAsync(string userId)
        {
            return _mapper.Map<UsuarioViewModel>(await _usuarioServices.GetByUserIdAsync(userId));
        }

        public async Task<UsuarioViewModel> GetByIdAsync(int id)
        {
            return _mapper.Map<UsuarioViewModel>(await _usuarioServices.GetByIdAsync(id));
        }

        public async Task<IEnumerable<UsuarioViewModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(await _usuarioServices.GetAllAsync());
        }

        public async Task<IEnumerable<UsuarioViewModel>> Filter(string termo, bool pendente, bool bloqueado)
        {
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(await _usuarioServices.Filter(termo,pendente,bloqueado));
        }

        public async Task<IEnumerable<UsuarioViewModel>> GetBloqueadosAsync()
        {
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(await _usuarioServices.GetBloqueadosAsync());
        }

        public async Task<IEnumerable<UsuarioViewModel>> GetPendentesAsync()
        {
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(await _usuarioServices.GetPendentesAsync());
        }

        public async Task UpdateAsync(UsuarioViewModel usuarioViewModel)
        {
            _unitOfWork.BeginTransaction();
            await _usuarioServices.UpdateAsync(_mapper.Map<UsuarioModel>(usuarioViewModel));
            await _unitOfWork.CommitAsync();
        }

        public async Task SetarStatusCadastroAsync(string status, string userId, string email)
        {
            _unitOfWork.BeginTransaction();
            await _usuarioServices.SetarStatusCadastroAsync(status, userId, email);
            await _unitOfWork.CommitAsync();
        }

        public async Task CreateAsync(UsuarioViewModel usuarioViewModel, string email,string callBackUrl)
        {
            _unitOfWork.BeginTransaction();
            await _usuarioServices.CreateAsync(_mapper.Map<UsuarioModel>(usuarioViewModel),email,callBackUrl);
            await _unitOfWork.CommitAsync();
        }
    }
}
