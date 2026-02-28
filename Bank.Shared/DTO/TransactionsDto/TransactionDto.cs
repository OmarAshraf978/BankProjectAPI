using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Shared.DTO.TransactionsDto
{
    public record TransactionDto
    (
        int AccountId,
        int AccountTargetId,
        decimal Amount,
        string TransactionType
    );
}
