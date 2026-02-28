using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.ServiceAbstraction.Services_Abstraction;
using Bank.Shared.DTO.TransactionsDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Presentation.Controllers
{
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

        [Authorize(Roles ="Admin")]
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

        [Authorize(Roles ="Admin,Teller")]
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
}
