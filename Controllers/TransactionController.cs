using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WalletTransaction.DAL;
using WalletTransaction.Model;
using WalletTransaction.Model.DTO;
using WalletTransaction.Services.Interfaces;

namespace WalletTransaction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactions _transactions;
        private readonly IMapper _mapper;
        private readonly WalletDbContext _dbContext;
        public TransactionController(ITransactions transactions, IMapper mapper, WalletDbContext dbContext)
        {
            _transactions = transactions;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("Create_New_Transaction")]
        public async Task<IActionResult> Createnewtransaction([FromBody] TransationRequestDto transationRequest)
        {
            if (!ModelState.IsValid) return BadRequest(transationRequest);
            var transaction = _mapper.Map<Transaction>(transationRequest);
            return Ok(await _transactions.CreateNewTransaction(transaction));

        }
        [HttpPost]
        [Route("Make_Deposit")]
        public async Task<IActionResult> MakeDeposit(string walletNumber, double amount, string TransactionPin)
        {
            if (!Regex.IsMatch(walletNumber, @"(?<!\d)\d{10}(?!\d)")) return BadRequest("Wallet Number must be 10-digit");
            return Ok(await _transactions.MakeDeposit(walletNumber, amount, TransactionPin));
        }

        [HttpPost]
        [Route("Make_Withdraw")]
        public async Task<IActionResult> MakeWithraw(string walletNumber, double amount, string TransactionPin)
        {
            if (!Regex.IsMatch(walletNumber, @"(?<!\d)\d{10}(?!\d)")) return BadRequest("Wallet Number must be 10-digit");
            return Ok(await _transactions.MakeWithdrawal(walletNumber, amount, TransactionPin));
        }

        [HttpPost]
        [Route("Make_Fund_Transfer")]
        public async Task<IActionResult> MakeTransfer(string fromWallet, string toWallet, double amount, string TransactionPin)
        {
            if (!Regex.IsMatch(fromWallet, @"(?<!\d)\d{10}(?!\d)") || !Regex.IsMatch(toWallet, @"(?<!\d)\d{10}(?!\d)")) return BadRequest("Wallet Number must be 10-digit");
            return Ok(await _transactions.MakeTransfer(fromWallet, toWallet, amount, TransactionPin));
        }
        [HttpGet]
        [Route("Get_All_Transaction")]
        public async Task<ActionResult<List<Transaction>>> GetTransactions(int page)
        {
            if (_dbContext.Transactions == null)
                return NotFound();
            var pageResult = 2f;
            var pageCount = Math.Ceiling(_dbContext.Transactions.Count() / pageResult);
            var transactions = await _dbContext.Transactions
            .Skip((page - 1) * (int)pageResult)
                .Take((int)pageResult).ToListAsync();
            var responsonse = new paging<Transaction>
            {
                Translist = transactions,
                CurrentPage =page,
                Pages =(int)pageCount
            };
            return Ok(responsonse);

        }
        [HttpGet]
        [Route ("Check_Account_Balance")]
        public async Task<IActionResult >CheckBalance(string walletNumber, string Currency)
        {
            if (!Regex.IsMatch(walletNumber, @"(?<!\d)\d{10}(?!\d)")) return BadRequest("Wallet Number must be 10-digit");
            return Ok(await _transactions.CheckWalletBalance(walletNumber, Currency));
        }

    }
}
