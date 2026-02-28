using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Bank.ServiceAbstraction.Services_Abstraction;
using Bank.Shared.DTO.AccountDto;
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
    #endregion
}
