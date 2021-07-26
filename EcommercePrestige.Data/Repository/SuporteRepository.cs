using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;

namespace EcommercePrestige.Data.Repository
{
    public class SuporteRepository:BaseRepository<SuporteModel>,ISuporteRepository
    {
        public SuporteRepository(EcommerceContext context) : base(context)
        {
        }

    }
}
