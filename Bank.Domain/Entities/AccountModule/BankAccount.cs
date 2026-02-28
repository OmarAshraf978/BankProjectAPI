using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Domain.Entities.IdentityModule;
using Bank.Domain.Entities.TransactionModule;

namespace Bank.Domain.Entities.AccountModule
{
    #region BankAccountModule
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountName { get; set; } = default!;
        public decimal Balance { get; set; }
        public string UserId { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
        public AccountType AccountType { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
        public ICollection<Transaction> TargetTransactions { get; set; } = new HashSet<Transaction>();
    }
    public enum AccountType
    {
        Saving = 1,
        Current = 2,
        Business = 3
    }
    public enum AccountStatus
    {
        Active = 1,
        Blocked = 2,
        Closed = 3
    }
    #endregion
}
