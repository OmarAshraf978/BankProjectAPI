using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Shared.DTO.AccountDto
{
    public record GetAccountDetailsDto
    (
        string AccountName,
        string AccountType,
        string AccountStatus,
        decimal Balance,
        DateTimeOffset CreatedAt
    );
}
