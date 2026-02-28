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

    #endregion
}
