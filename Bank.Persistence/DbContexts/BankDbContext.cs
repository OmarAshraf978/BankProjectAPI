using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Domain.Entities.AccountModule;
using Bank.Domain.Entities.TransactionModule;
using Microsoft.EntityFrameworkCore;

namespace Bank.Persistence.DbContexts
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
         => modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankDbContext).Assembly);

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> transactions { get; set; }
    }
}
