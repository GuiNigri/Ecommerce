using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Services
{
    public class MaterialServices:BaseServices<MaterialModel>,IMaterialServices
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialServices(IMaterialRepository materialRepository) : base(materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<MaterialModel> GetByNameAsync(string name)
        {
            return await _materialRepository.GetByNameAsync(name);
        }

        public async Task<IEnumerable<MaterialModel>> FilterAsync(string termo)
        {
            return await _materialRepository.FilterAsync(termo);
        }
    }
}
