using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Domain.Repository;

namespace Bank.Domain.IUnitOfWork
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangesAsync();
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class;
    }
}
