using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bank.Domain.Repository;
using Bank.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Bank.Persistence.Repository
{
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
}
