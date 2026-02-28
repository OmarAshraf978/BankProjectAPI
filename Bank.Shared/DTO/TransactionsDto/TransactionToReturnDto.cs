using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Shared.DTO.TransactionsDto
{
    public class TransactionToReturnDto
    {
        public int AccountId { get; set; }
        public int? AccountTargetId { get; set; }
        public string TransactionType { get; set; } = default!;
        public string TransactionStatus { get; set; } = default!;
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
    }
}
