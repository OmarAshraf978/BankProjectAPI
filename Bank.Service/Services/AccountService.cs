using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Domain.Entities.AccountModule;
using Bank.Domain.IUnitOfWork;
using Bank.ServiceAbstraction.Services_Abstraction;
using Bank.Shared.DTO.AccountDto;
using Bank.Shared.ResultPattern;

namespace Bank.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<ActivateAccountDto>> ActivateAccountAsync(int AccountId)
        {
            var Account = await _unitOfWork.GetRepository<BankAccount, int>().GetByIdAsync(AccountId);
            if (Account is null)
                return Error.NotFound("Account.NotFound", $"Account With Id: {AccountId} Is Not Found");
            if(Account.AccountStatus == AccountStatus.Active)
                return Error.Failure("Account.AlreadyActive", $"Account {AccountId} is already active");
            Account.AccountStatus = AccountStatus.Active;
            var save = await _unitOfWork.SaveChangesAsync();
            if(save == 0)
                return Error.Failure("Account.Failure");
            return new ActivateAccountDto(Account.AccountName, "Account Was Activated Successfully");
        }

        public async Task<Result<BlockedAccountDto>> BlockAccountAsync(int AccountId)
        {
            var Account = await _unitOfWork.GetRepository<BankAccount, int>().GetByIdAsync(AccountId);
            if (Account is null)
                return Error.NotFound("Account.NotFound", $"Account With Id: {AccountId} Is Not Found");
            if (Account.AccountStatus == AccountStatus.Blocked)
                return Error.Failure("Account.AlreadyBlocked", $"Account {AccountId} is already Blocked");
            Account.AccountStatus = AccountStatus.Blocked;
            var save = await _unitOfWork.SaveChangesAsync();
            if (save == 0)
                return Error.Failure("Account.Failure");
            return new BlockedAccountDto(Account.AccountName, "Account Was Blocked Successfully");
        }

        public async Task<Result<BankAccountDto>> CreateAccountAsync(BankAccountDto accountDto, string UserId)
        {
            var Account = _mapper.Map<BankAccount>(accountDto);
            Account.CreatedAt = DateTimeOffset.Now;
            Account.UserId = UserId;
            Account.AccountStatus = AccountStatus.Active;
            await _unitOfWork.GetRepository<BankAccount, int>().AddAsync(Account);
            var save = await _unitOfWork.SaveChangesAsync();
            if (save == 0)
                return Error.Failure("Account.Failure");
            return _mapper.Map<BankAccountDto>(Account);
        }

        public async Task<Result<DeActivateAccountDto>> DeactivateAccountAsync(int AccountId)
        {
            var Account = await _unitOfWork.GetRepository<BankAccount, int>().GetByIdAsync(AccountId);
            if (Account is null)
                return Error.NotFound("Account.NotFound", $"Account With Id: {AccountId} Is Not Found");
            if (Account.AccountStatus == AccountStatus.Closed)
                return Error.Failure("Account.AlreadyInActive", $"Account {AccountId} is already DeActive");
            Account.AccountStatus = AccountStatus.Closed;
            var save = await _unitOfWork.SaveChangesAsync();
            if (save == 0)
                return Error.Failure("Account.Failure");
            return new DeActivateAccountDto(Account.AccountName, "Account Was DeActivated Successfully");
        }

        public async Task<Result<DeleteAccountDto>> DeleteAccountAsync(int AccountId)
        {
            var Account = await _unitOfWork.GetRepository<BankAccount, int>().GetByIdAsync(AccountId);
            if (Account is null)
                return Error.NotFound("Account.NotFound", $"Account With Id: {AccountId} Is Not Found");
            _unitOfWork.GetRepository<BankAccount, int>().Delete(Account);
            var save = await _unitOfWork.SaveChangesAsync();
            if (save == 0)
                return Error.Failure("Account.Failure", "Failed to delete account");
            return new DeleteAccountDto(Account.AccountName, "Account Was Deleted Successfully");
        }

        public async Task<Result<GetAccountBalanceDto>> GetAccountBalance(string UserId, int AccountId)
        {
            var Account = await _unitOfWork.GetRepository<BankAccount, int>().GetByIdAsync(AccountId);
            if (Account is null)
                return Error.NotFound("Account.NotFound", $"Account With Id: {AccountId} Is Not Found");
            if (Account.UserId != UserId)
                return Error.Unauthorized("Account.Unauthorized", $"Account With UserId: {UserId} Is Unauthorized");
            return new GetAccountBalanceDto(Account.AccountName, Account.Balance);
        }

        public async Task<Result<GetAccountDetailsDto>> GetAccountDetailsAsync(int AccountId, string UserId)
        {
            var Account = await _unitOfWork.GetRepository<BankAccount, int>().GetByIdAsync(AccountId);
            if (Account is null)
                return Error.NotFound("Account.NotFound", $"Account With Id: {AccountId} Is Not Found");
            if (Account.UserId != UserId)
                return Error.Unauthorized("Account.Unauthorized", $"Account With UserId: {UserId} Is Unauthorized");
            return new GetAccountDetailsDto
                (Account.AccountName,
                Account.AccountType.ToString(),
                Account.AccountStatus.ToString(),
                Account.Balance,
                Account.CreatedAt);
        }

        public async Task<Result<IEnumerable<BankAccountDto>>> GetAccountsByIdAsync(string UserId)
        {
            var Accounts = await _unitOfWork.GetRepository<BankAccount, int>().GetByColumnAsync(a => a.UserId == UserId);
            if (!Accounts.Any())
                return Error.NotFound("Account.NotFound", $"There Is No Accounts For UserId: {UserId}");
            return _mapper.Map<IEnumerable<BankAccountDto>>(Accounts).ToList();
        }
    }
}
