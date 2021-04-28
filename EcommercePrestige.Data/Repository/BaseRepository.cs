using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommercePrestige.Data.Repository
{
    public class BaseRepository<TModel>:IBaseRepository<TModel> where TModel:BaseModel
    {
        private readonly DbSet<TModel> _dbSet;

        protected BaseRepository(EcommerceContext context)
        {
            _dbSet = context.Set<TModel>();
        }
        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<TModel> GetByIdAsync(int id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task CreateAsync(TModel model)
        {
            await _dbSet.AddAsync(model);
        }

        public virtual async Task UpdateAsync(TModel model)
        {
            _dbSet.Update(model);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var model = await _dbSet.FindAsync(id);
            _dbSet.Remove(model);
        }
    }
}
