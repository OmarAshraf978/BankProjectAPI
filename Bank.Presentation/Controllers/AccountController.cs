using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Bank.ServiceAbstraction.Services_Abstraction;
using Bank.Shared.DTO.AccountDto;
using Bank.Shared.DTO.IdentityDto;
using Bank.Shared.DTO.TransactionsDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Presentation.Controllers
{
    #region Controllers
    public class AccountController : ApiBaseController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ActivateAccount")]
        public async Task<ActionResult<ActivateAccountDto>> ActivateAccount(int AccountId)
        {
            var Result = await _accountService.ActivateAccountAsync(AccountId);
            return HandleResult(Result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("BlockAccount")]
        public async Task<ActionResult<BlockedAccountDto>> BlockAccount(int AccountId)
        {
            var Result = await _accountService.BlockAccountAsync(AccountId);
            return HandleResult(Result);
        }

        [Authorize]
        [HttpPost("CreateAccount")]
        public async Task<ActionResult<BankAccountDto>> CreateAccount(BankAccountDto bankAccountDto)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var Result = await _accountService.CreateAccountAsync(bankAccountDto, UserId);
            return HandleResult(Result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("DeActivateAccount")]
        public async Task<ActionResult<DeActivateAccountDto>> DeActivateAccount(int AccountId)
        {
            var Result = await _accountService.DeactivateAccountAsync(AccountId);
            return HandleResult(Result);
        }

        [Authorize]
        [HttpPost("DeleteAccount")]
        public async Task<ActionResult<DeleteAccountDto>> DeleteAccount(int AccountId)
        {
            var Result = await _accountService.DeleteAccountAsync(AccountId);
            return HandleResult(Result);
        }

        [Authorize]
        [HttpGet("GetAccountBalance")]
        public async Task<ActionResult<GetAccountBalanceDto>> GetAccountBalance(int AccountId)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var Result = await _accountService.GetAccountBalance(UserId, AccountId);
            return HandleResult(Result);
        }

        [Authorize]
        [HttpGet("GetAccountDetails")]
        public async Task<ActionResult<GetAccountDetailsDto>> GetAccountDetails(int AccountId)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var Result = await _accountService.GetAccountDetailsAsync(AccountId, UserId);
            return HandleResult(Result);
        }

        [Authorize]
        [HttpGet("GetAccountsById")]
        public async Task<ActionResult<IEnumerable<BankAccountDto>>> GetAccountsById()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var Result = await _accountService.GetAccountsByIdAsync(UserId);
            return HandleResult(Result);
        }
    }
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var Result = await _authenticationService.RegisterAsync(registerDto);
            return HandleResult(Result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var Result = await _authenticationService.LoginAsync(loginDto);
            return HandleResult(Result);
        }

        [HttpGet("CheckEmail")]
        public async Task<ActionResult<UserExistsDto>> CheckEmail(string email)
        {
            var Result = await _authenticationService.UserExistsAsync(email);
            return HandleResult(Result);
        }

        [Authorize]
        [HttpGet("GetUserByEmail")]
        public async Task<ActionResult<UserDto>> GetUserByEmail()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email)!;
            var Result = await _authenticationService.GetUserByEmailAsync(Email);
            return HandleResult(Result);
        }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<ActionResult<LogOutDto>> LogOut(string email)
        {
            var Result = await _authenticationService.LogOutAsync(email);
            return HandleResult(Result);
        }
    }
    public class TransactionController : ApiBaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize]
        [HttpPost("CreateTransaction")]
        public async Task<ActionResult<TransactionToReturnDto>> CreateTransaction(TransactionDto transactionDto)
        {
            var Result = await _transactionService.CreateTransactionAsync(transactionDto);
            return HandleResult(Result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllTransactions")]
        public async Task<ActionResult<IEnumerable<TransactionToReturnDto>>> GetAllTransactions()
        {
            var Result = await _transactionService.GetAllTransactionsAsync();
            return HandleResult(Result);
        }

        [Authorize]
        [HttpGet("GetAllTransactionsByAccountId")]
        public async Task<ActionResult<IEnumerable<TransactionToReturnDto>>> GetAllTransactionsByAccountId(int AccountId)
        {
            var Result = await _transactionService.GetAllTransactionsByAccountIdAsync(AccountId);
            return HandleResult(Result);
        }

        [Authorize(Roles = "Admin,Teller")]
        [HttpGet("GetTransactionById")]
        public async Task<ActionResult<TransactionToReturnDto>> GetTransactionByTransactionId(int TransactionId)
        {
            var Result = await _transactionService.GetTransactionByIdAsync(TransactionId);
            return HandleResult(Result);
        }

        [Authorize(Roles = "Admin,Teller")]
        [HttpGet("GetTransactionsByStatus")]
        public async Task<ActionResult<IEnumerable<TransactionToReturnDto>>> GetTransactionsByStatus(string status)
        {
            var Result = await _transactionService.GetTransactionsByStatusAsync(status);
            return HandleResult(Result);
        }
    }
    #endregion
}
