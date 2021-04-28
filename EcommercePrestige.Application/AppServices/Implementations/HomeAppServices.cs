using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using EcommercePrestige.Application.AutoMapper;
using EcommercePrestige.Application.ViewModel;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Services;
using EcommercePrestige.Model.Interfaces.UoW;

namespace EcommercePrestige.Application.AppServices.Implementations
{
    public class HomeAppServices:IHomeAppServices
    {
        private readonly ITextoHomeServices _textoHomeServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBannersHomeServices _bannersHomeServices;
        private readonly IMapper _mapper;

        public HomeAppServices(ITextoHomeServices textoHomeServices,IUnitOfWork unitOfWork, IBannersHomeServices bannersHomeServices)
        {
            _textoHomeServices = textoHomeServices;
            _unitOfWork = unitOfWork;
            _bannersHomeServices = bannersHomeServices;
            _mapper = AutoMapperConfig.Mapper;
        }

        public async Task<IEnumerable<TextoHomeViewModel>> GetAllTextosAsync()
        {
            return _mapper.Map<IEnumerable<TextoHomeViewModel>>(await _textoHomeServices.GetAllAsync());
        }

        public async Task CreateTextoAsync(TextoHomeViewModel textoHomeViewModel)
        {
            _unitOfWork.BeginTransaction();
            await _textoHomeServices.CreateAsync(_mapper.Map<TextoHomeModel>(textoHomeViewModel));
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteTextoAsync(int id)
        {
            _unitOfWork.BeginTransaction();
            await _textoHomeServices.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<BannersHomeViewModel>> GetAllBannersAsync()
        {
            return _mapper.Map<IEnumerable<BannersHomeViewModel>>(await _bannersHomeServices.GetAllAsync());
        }

        public async Task CreateBannerAsync(Stream banner)
        {
            _unitOfWork.BeginTransaction();
            await _bannersHomeServices.CreateAsync(banner);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteBannerAsync(int id)
        {
            _unitOfWork.BeginTransaction();
            await _bannersHomeServices.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }
    }
}
