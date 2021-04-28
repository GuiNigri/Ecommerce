using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Services
{
    public class BaseServices<TModel>:IBaseServices<TModel> where TModel:BaseModel
    {
        private readonly IBaseRepository<TModel> _baseRepository;

        protected BaseServices(IBaseRepository<TModel> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
        {
           return await _baseRepository.GetAllAsync();
        }

        public virtual async Task<TModel> GetByIdAsync(int id)
        {
            return await _baseRepository.GetByIdAsync(id);
        }

        public virtual async Task CreateAsync(TModel model)
        {
            await _baseRepository.CreateAsync(model);
        }

        public virtual async Task UpdateAsync(TModel model)
        {
            await _baseRepository.UpdateAsync(model);
        }

        public virtual async Task DeleteAsync(int id)
        {
            await _baseRepository.DeleteAsync(id);
        }
    }
}
