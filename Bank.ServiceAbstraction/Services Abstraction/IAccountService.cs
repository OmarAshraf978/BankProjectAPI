using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Shared.DTO.AccountDto;
using Bank.Shared.ResultPattern;

namespace Bank.ServiceAbstraction.Services_Abstraction
{
    public interface IAccountService
    {
        public Task<Result<BankAccountDto>> CreateAccountAsync(BankAccountDto accountDto, string UserId);
        public Task<Result<IEnumerable<BankAccountDto>>> GetAccountsByIdAsync(string UserId);
        public Task<Result<GetAccountBalanceDto>> GetAccountBalance(string UserId, int AccountId);
        public Task<Result<ActivateAccountDto>> ActivateAccountAsync(int AccountId);
        public Task<Result<DeActivateAccountDto>> DeactivateAccountAsync(int AccountId);
        public Task<Result<BlockedAccountDto>> BlockAccountAsync(int AccountId);
        public Task<Result<DeleteAccountDto>> DeleteAccountAsync(int AccountId);
        public Task<Result<GetAccountDetailsDto>> GetAccountDetailsAsync(int AccountId, string UserId);
    }
}
