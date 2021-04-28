using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommercePrestige.Data.Repository
{
    public class SuporteRepository:BaseRepository<SuporteModel>,ISuporteRepository
    {
        public SuporteRepository(EcommerceContext context) : base(context)
        {
        }

    }
}
