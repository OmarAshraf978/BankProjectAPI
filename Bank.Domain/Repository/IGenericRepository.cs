using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Repository
{
    #region GenericRepository
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<TEntity> GetByIdAsync(TKey id);
        public Task<IEnumerable<TEntity>> GetByColumnAsync(Expression<Func<TEntity, bool>> predicate);
        public Task AddAsync(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);
    }
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
    {
        private readonly BankDbContext _dbContext;
        public GenericRepository(BankDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(TEntity entity)
         => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void Delete(TEntity entity)
         => _dbContext.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync()
         => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<IEnumerable<TEntity>> GetByColumnAsync(Expression<Func<TEntity, bool>> predicate)
         => await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();

        public async Task<TEntity> GetByIdAsync(TKey id)
         => await _dbContext.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity)
         => _dbContext.Set<TEntity>().Update(entity);
    }
    #endregion
}
