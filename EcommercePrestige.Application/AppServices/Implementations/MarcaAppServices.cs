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
    public class MarcaAppServices:IMarcaAppServices
    {
        private readonly IMarcaServices _marcaServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MarcaAppServices(IMarcaServices marcaServices,IUnitOfWork unitOfWork)
        {
            _marcaServices = marcaServices;
            _unitOfWork = unitOfWork;
            _mapper = AutoMapperConfig.Mapper;
        }

        public async Task<IEnumerable<MarcaViewModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<MarcaViewModel>>(await _marcaServices.GetAllAsync());
        }

        public async Task<MarcaViewModel> GetByIdAsync(int id)
        {
            return _mapper.Map<MarcaViewModel>(await _marcaServices.GetByIdAsync(id));
        }

        public async Task CreateAsync(MarcaViewModel marcaViewModel)
        {
            _unitOfWork.BeginTransaction();
            await _marcaServices.CreateAsync(_mapper.Map<MarcaModel>(marcaViewModel));
            await _unitOfWork.CommitAsync();
        }
        public async Task UpdateAsync(MarcaViewModel marcaViewModel)
        {
            _unitOfWork.BeginTransaction();
            await _marcaServices.UpdateAsync(_mapper.Map<MarcaModel>(marcaViewModel));
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<MarcaViewModel>> Filter(string termo)
        {
            return _mapper.Map<IEnumerable<MarcaViewModel>>(await _marcaServices.Filter(termo));
        }
    }
}
