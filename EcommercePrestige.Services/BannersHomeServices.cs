using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Infrastructure;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Services
{
    public class BannersHomeServices:BaseServices<BannersHomeModel>, IBannersHomeServices
    {
        private readonly IBannersHomeRepository _baseRepository;
        private readonly IBlobInfrastructure _blobInfrastructure;

        public  BannersHomeServices(IBannersHomeRepository baseRepository, IBlobInfrastructure blobInfrastructure) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _blobInfrastructure = blobInfrastructure;
        }

        public async Task CreateAsync(Stream banner)
        {
            var uriBlob = await _blobInfrastructure.CreateBlobAsync(banner,"bannersblob");

            await _baseRepository.CreateAsync(new BannersHomeModel(uriBlob));
        }

        public override async Task DeleteAsync(int id)
        {
            var model = await _baseRepository.GetByIdAsync(id);

            await _baseRepository.DeleteAsync(model);

            await _blobInfrastructure.DeleteBlobAsync(model.UrlBanner, "bannersblob");

            
        }
    }
}
