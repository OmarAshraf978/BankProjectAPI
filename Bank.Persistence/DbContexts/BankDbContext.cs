using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Domain.Entities.AccountModule;
using Bank.Domain.Entities.IdentityModule;
using Bank.Domain.Entities.TransactionModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bank.Persistence.DbContexts
{
    #region Contexts
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
         => modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankDbContext).Assembly);

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> transactions { get; set; }
    }
    public class BankIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public BankIdentityDbContext(DbContextOptions<BankIdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Address>().ToTable("Addresses");
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<ApplicationUser>()
                   .HasOne(a => a.Address)
                   .WithOne(a => a.User)
                   .HasForeignKey<Address>(a => a.UserId);
        }
    }
    #endregion
}
