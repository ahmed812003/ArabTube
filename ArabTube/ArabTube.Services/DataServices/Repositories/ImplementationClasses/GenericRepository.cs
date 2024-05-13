using ArabTube.Services.DataServices.Data;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ArabTube.Services.DataServices.Repositories.ImplementationClasses
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> FindByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<bool> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public virtual bool Update(T entity)
        {
            _dbSet.Update(entity);
            return true;
        }

        public virtual async Task<bool> DeleteAsync(string Id)
        {
            T? Entity = await _dbSet.FindAsync(Id);
            if (Entity == null)
                return false;
            _dbSet.Remove(Entity);
            return true;
        }

    }
}
