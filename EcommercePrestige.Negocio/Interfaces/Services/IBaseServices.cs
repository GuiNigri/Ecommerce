using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface IBaseServices<TModel>:IBaseRepository<TModel> where TModel:BaseModel
    {
    }
}
