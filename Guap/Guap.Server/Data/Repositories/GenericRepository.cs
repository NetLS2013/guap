using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Guap.Server.Data.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }
        
        protected IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking();
        }
        
        protected async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        
        protected async Task Create(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        
        protected async Task Update(TEntity entity)
        {
            _dbSet.Update(entity);
            
            await _dbContext.SaveChangesAsync();
        }
        
        protected async Task Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            
            await _dbContext.SaveChangesAsync();
        }
    }
}