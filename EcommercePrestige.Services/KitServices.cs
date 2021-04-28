using EcommercePrestige.Model.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Services
{
    public class KitServices:BaseServices<KitModel>,IKitsServices
    {
        private readonly IKitsRepository _baseRepository;

        public KitServices(IKitsRepository baseRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<IEnumerable<KitModel>> FilterAsync(string termo)
        {
            return await _baseRepository.FilterAsync(termo);
        }
    }
}
