using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;

namespace EcommercePrestige.Data.Repository
{
    public class TextoHomeRepository:BaseRepository<TextoHomeModel>, ITextoHomeRepository
    {
        public TextoHomeRepository(EcommerceContext context) : base(context)
        {
        }
    }
}
