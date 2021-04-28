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
    public class MaterialAppServices:IMaterialAppServices
    {
        private readonly IMaterialServices _materialServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MaterialAppServices(IMaterialServices materialServices,IUnitOfWork unitOfWork)
        {
            _materialServices = materialServices;
            _unitOfWork = unitOfWork;
            _mapper = AutoMapperConfig.Mapper;
        }

        public async Task<IEnumerable<MaterialViewModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<MaterialViewModel>>(await _materialServices.GetAllAsync());
        }

        public async Task<MaterialViewModel> GetByIdAsync(int id)
        {
            return _mapper.Map<MaterialViewModel>(await _materialServices.GetByIdAsync(id));
        }

        public async Task CreateAsync(MaterialViewModel materialViewModel)
        {
            _unitOfWork.BeginTransaction();
            await _materialServices.CreateAsync(_mapper.Map<MaterialModel>(materialViewModel));
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(MaterialViewModel materialViewModel)
        {
            _unitOfWork.BeginTransaction();
            await _materialServices.UpdateAsync(_mapper.Map<MaterialModel>(materialViewModel));
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<MaterialViewModel>> FilterAsync(string termo)
        {
            return _mapper.Map<IEnumerable<MaterialViewModel>>(await _materialServices.FilterAsync(termo));
        }
    }
}
