using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Domain.IUnitOfWork;
using Bank.Domain.Repository;
using Bank.Persistence.DbContexts;
using Bank.Persistence.Repository;
using Microsoft.AspNetCore.Components.Forms;

namespace Bank.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];
        public UnitOfWork(BankDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class
        {
            var EntityType = typeof(TEntity);
            if(_repositories.TryGetValue(EntityType, out object? repository))
                return (IGenericRepository<TEntity, TKey>)repository;
            var newRepository = new GenericRepository<TEntity, TKey>(_dbContext);
            _repositories[EntityType] = newRepository;
            return newRepository;
        }

        public async Task<int> SaveChangesAsync()
         => await _dbContext.SaveChangesAsync();
    }
}
