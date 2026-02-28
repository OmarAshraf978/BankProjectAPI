using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Shared.DTO.TransactionsDto;
using Bank.Shared.ResultPattern;

namespace Bank.ServiceAbstraction.Services_Abstraction
{
    public interface ITransactionService
    {
        public Task<Result<TransactionToReturnDto>> CreateTransactionAsync(TransactionDto transactionDto);
        public Task<Result<TransactionToReturnDto>> GetTransactionByIdAsync(int transactionId);
        public Task<Result<IEnumerable<TransactionToReturnDto>>> GetAllTransactionsAsync();
        public Task<Result<IEnumerable<TransactionToReturnDto>>> GetAllTransactionsByAccountIdAsync(int AccountId);
        Task<Result<IEnumerable<TransactionToReturnDto>>> GetTransactionsByStatusAsync(string status);
    }
}
