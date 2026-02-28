using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Bank.Domain.Entities.AccountModule;

namespace Bank.Domain.Entities.TransactionModule
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public BankAccount BankAccount { get; set; } = default!;
        public int? AccountTargetId { get; set; }
        public BankAccount? BankAccountTarget { get; set; } 
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionType Type { get; set; }
        public  DateTimeOffset CreatedAt { get; set; }
    }
}
