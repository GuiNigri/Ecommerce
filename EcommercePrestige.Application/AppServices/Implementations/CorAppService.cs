using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcommercePrestige.Application.AutoMapper;
using EcommercePrestige.Application.ViewModel;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Application.AppServices.Implementations
{
    public class CorAppService:ICorAppServices
    {
        private readonly ICorService _corService;
        private readonly IMapper _mapper;

        public CorAppService(ICorService corService)
        {
            _corService = corService;
            _mapper = AutoMapperConfig.Mapper;
        }
        public async Task<IEnumerable<CorViewModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CorViewModel>>(await _corService.GetAllAsync());
        }

        public async Task<CorViewModel> GetByIdAsync(int id)
        {
            return _mapper.Map<CorViewModel>(await _corService.GetByIdAsync(id));
        }
    }
}
