using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcommercePrestige.Application.AutoMapper;
using EcommercePrestige.Application.ViewModel;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Application.AppServices.Implementations
{
    public class PackageAppServices:IPackageAppServices
    {
        private readonly ICorreiosServices _correiosServices;
        private readonly IMapper _mapper;

        public PackageAppServices(ICorreiosServices correiosServices)
        {
            _correiosServices = correiosServices;
            _mapper = AutoMapperConfig.Mapper;
        }
        public async Task<IEnumerable<TrackingHistoryViewModel>> GetTracking(string codRastreio, int codPedido)
        {
            return _mapper.Map<IEnumerable<TrackingHistoryViewModel>>(await _correiosServices.GetTracking(codRastreio, codPedido));
        }
    }
}
