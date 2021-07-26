using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Services
{
    public class CorService:BaseServices<CorModel>, ICorService
    {
        public CorService(ICorRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
