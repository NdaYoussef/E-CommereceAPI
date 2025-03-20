
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
        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> DeleteAsync(int id)
        {
            var model = await _context.Set<T>().FindAsync(id);
            if (model != null)
            {
                _context.Set<T>().Remove(model);
                await _context.SaveChangesAsync();
            }
            return model;
        }
    }
}
