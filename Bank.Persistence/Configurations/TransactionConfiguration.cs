using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Domain.Entities.TransactionModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> T)
        {
            T.HasOne(t=>t.BankAccount)
             .WithMany(b=>b.Transactions)
             .HasForeignKey(t=>t.AccountId)
             .OnDelete(DeleteBehavior.Restrict);

            T.HasOne(t => t.BankAccountTarget)
             .WithMany(b => b.TargetTransactions)
             .HasForeignKey(t => t.AccountTargetId)
             .OnDelete(DeleteBehavior.SetNull);

            T.Property(t => t.BalanceBefore).HasPrecision(18, 4);
            T.Property(t => t.BalanceAfter).HasPrecision(18, 4);
            T.Property(t => t.Amount).HasPrecision(18, 4);
        }
    }
}
