using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Entities.TransactionModule
{
    public enum TransactionStatus
    {
        Success = 1,
        Failed = 2,
        Pending = 3
    }
}
