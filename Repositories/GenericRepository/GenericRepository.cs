
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TestToken.Data;

namespace TestToken.Repositories.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
           var  models = await _context.Set<T>().ToListAsync();
            return models;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var model = await _context.Set<T>().FindAsync(id);
            return model; 
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }
        public async Task<T> updateAsync(T entity)
        {
            var model = await _context.Set<T>().FindAsync(entity);
            if(entity is not null)
            {
                _context.Entry(entity).CurrentValues.SetValues(model);
                _context.Entry(entity).State = EntityState.Modified;
                return model;
            }
            return null;
        }
        public async Task<T> deleteAsync(int id)
        {
            var model = await _context.Set<T>().FindAsync(id);
            if (model is not null)
            {
                _context.Set<T>().Remove(model);
                return model;
            }
            return null;
        }
    }
}
