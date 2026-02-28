using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Domain.Entities.AccountModule;
using Bank.Domain.Entities.TransactionModule;
using Bank.Domain.IUnitOfWork;
using Bank.ServiceAbstraction.Services_Abstraction;
using Bank.Shared.DTO.TransactionsDto;
using Bank.Shared.ResultPattern;

namespace Bank.Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<TransactionToReturnDto>> CreateTransactionAsync(TransactionDto transactionDto)
        {
            var Account = await _unitOfWork.GetRepository<BankAccount, int>().GetByIdAsync(transactionDto.AccountId);
            if (Account is null)
            {
                return Error.NotFound("Account.NotFound", $"Account With Id: {transactionDto.AccountId} Is Not Found");
            }
            if ((Account!.Balance < transactionDto.Amount) && (transactionDto.TransactionType.ToLower() == TransactionType.Transfer.ToString().ToLower()))
            {
                var error = Error.Failure("Transaction.Failure", "Account Balance Is Less Than The Amount You Want To Transfer");
                var Transaction = _mapper.Map<Transaction>(transactionDto);
                Transaction.Status = TransactionStatus.Failed;
                Transaction.CreatedAt = DateTimeOffset.UtcNow;
                await _unitOfWork.GetRepository<Transaction, int>().AddAsync(Transaction);
                await _unitOfWork.SaveChangesAsync();
                return error;
            }    
            if ((Account.Balance < transactionDto.Amount) && (transactionDto.TransactionType.ToLower() == TransactionType.Withdraw.ToString().ToLower()))
            {
                var error = Error.Failure("Transaction.Failure", "Account Balance Is Less Than The Amount You Want To WithDraw");
                var Transaction = _mapper.Map<Transaction>(transactionDto);
                Transaction.Status = TransactionStatus.Failed;
                Transaction.CreatedAt = DateTimeOffset.UtcNow;
                await _unitOfWork.GetRepository<Transaction, int>().AddAsync(Transaction);
                await _unitOfWork.SaveChangesAsync();
                return error;
            }
            var transaction = _mapper.Map<Transaction>(transactionDto);
            transaction.Status = TransactionStatus.Success;
            transaction.CreatedAt = DateTimeOffset.UtcNow;  
            if (transactionDto.TransactionType.ToLower() == TransactionType.Transfer.ToString().ToLower())
            {
                var AccountTarget = await _unitOfWork.GetRepository<BankAccount, int>().GetByIdAsync(transactionDto.AccountTargetId);
                if (AccountTarget is null)
                    return Error.NotFound("AccountTarget.NotFound", $"AccountTarget With Id: {transactionDto.AccountTargetId} Is Not Found");
                transaction.BalanceBefore = Account.Balance;
                transaction.BalanceAfter = Account.Balance - transactionDto.Amount;
                Account.Balance -= transactionDto.Amount;
                AccountTarget.Balance += transactionDto.Amount;
                _unitOfWork.GetRepository<BankAccount, int>().Update(AccountTarget);
            }
            if (transactionDto.TransactionType.ToLower() == TransactionType.Deposit.ToString().ToLower())
            {
                transaction.BalanceBefore = Account.Balance;
                transaction.BalanceAfter = Account.Balance + transactionDto.Amount;
                Account.Balance += transactionDto.Amount;
            }
            if (transactionDto.TransactionType.ToLower() == TransactionType.Withdraw.ToString().ToLower())
            {
                transaction.BalanceBefore = Account.Balance;
                transaction.BalanceAfter = Account.Balance - transactionDto.Amount;
                Account.Balance -= transactionDto.Amount;
            }
            await _unitOfWork.GetRepository<Transaction, int>().AddAsync(transaction);
            _unitOfWork.GetRepository<BankAccount, int>().Update(Account);
            var save = await _unitOfWork.SaveChangesAsync();
            if (save <= 0)
                return Error.Failure("SaveChanges.Failure", "Error While Save Changes");
            return _mapper.Map<TransactionToReturnDto>(transaction);
        }

        public async Task<Result<IEnumerable<TransactionToReturnDto>>> GetAllTransactionsAsync()
        {
            var Transactions = await _unitOfWork.GetRepository<Transaction, int>().GetAllAsync();
            if (!Transactions.Any())
                return Error.NotFound("Transactions.NotFound", "There Is No Transactions");
            return _mapper.Map<IEnumerable<TransactionToReturnDto>>(Transactions).ToList();
        }

        public async Task<Result<IEnumerable<TransactionToReturnDto>>> GetAllTransactionsByAccountIdAsync(int AccountId)
        {
            var Transactions = await _unitOfWork.GetRepository<Transaction, int>().GetByColumnAsync(t => t.AccountId == AccountId);
            if (!Transactions.Any()) 
                return Error.NotFound("Transactions.NotFound", "There Is No Transactions");
            return _mapper.Map<IEnumerable<TransactionToReturnDto>>(Transactions).ToList();
        }

        public async Task<Result<TransactionToReturnDto>> GetTransactionByIdAsync(int transactionId)
        {
            var Transaction = await _unitOfWork.GetRepository<Transaction, int>().GetByIdAsync(transactionId);
            if (Transaction is null)
                return Error.NotFound("Transaction.NotFound", $"Transaction With Id: {transactionId} Is Not Found");
            return _mapper.Map<TransactionToReturnDto>(Transaction);
        }

        public async Task<Result<IEnumerable<TransactionToReturnDto>>> GetTransactionsByStatusAsync(string status)
        {
            var Transactions = await _unitOfWork.GetRepository<Transaction, int>().GetByColumnAsync(t => t.Status.ToString() == status);
            if (!Transactions.Any())
                return Error.NotFound("Transactions.NotFound", "There Is No Transactions");
            return _mapper.Map<IEnumerable<TransactionToReturnDto>>(Transactions).ToList();
        }
    }
}
