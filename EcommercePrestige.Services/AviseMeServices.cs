using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Services
{
    public class AviseMeServices:BaseServices<AviseMeModel>,IAviseMeServices
    {
        public AviseMeServices(IAviseMeRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
