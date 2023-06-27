using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using WalletTransaction.DAL;
using WalletTransaction.Model;
using WalletTransaction.Model.DTO;
using WalletTransaction.Services.Interfaces;

namespace WalletTransaction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletServices _services;
        private readonly IMapper _mapper;
        private readonly WalletDbContext _dbContext;

        public WalletsController(IWalletServices services, IMapper mapper, WalletDbContext dbContext)
        {
            _services = services;
            _mapper = mapper;
            _dbContext = dbContext;

        }
        [HttpPost]
        [Route("Register_New_Wallet")]
        public IActionResult RegisterNewWalletAccount([FromBody] RegisterNewWalletModel newWallet)
        {
            if (!ModelState.IsValid)
                return BadRequest(newWallet);
            var wallet = _mapper.Map<Wallet>(newWallet);
            return Ok(_services.Create(wallet, newWallet.WalletPassword, newWallet.ConfirmPassword));
        }
        [HttpGet]
        [Route("Get_All_Wallet_Account")]
        public IActionResult GetAllWalletAccount()
        {
            var wallet = _services.GetAllWallets();
            var cleanAccount = _mapper.Map<GetwalletModel>(wallet);
            return Ok(cleanAccount);
        }
        [HttpGet]
        [Route("Get_wallet_By_Wallet_Number")]
        public IActionResult GetByWalletNumber(string WalletNumber)
        {
            if (Regex.IsMatch(WalletNumber, @"/^\d{10}$/")) ;
            {
                var wallNumber = _services.GetWalletByWalletNumber(WalletNumber);
                var cleanWallet = _mapper.Map<GetwalletModel>(wallNumber);
                return Ok(cleanWallet);

            }
           
            return BadRequest("wallet Number must be 10-digits");


        }
        [HttpGet]
        [Route ("Get_wallet_By_Email")]
        public async Task < IActionResult> GetByEmail(string Email)
        {
            {
                var walletEmail = await _services.GetByEmail(Email );
                var cleanWallet =  _mapper.Map<GetwalletModel>(walletEmail);
                return Ok( cleanWallet);

            }

           


        }
        [HttpPut ]
        [Route ("Update_wallet")]
        public async Task < IActionResult> UpdateWalletAccount([FromBody] UpdateWalletModel model)
        {
            if(!ModelState.IsValid ) return BadRequest(model );
            var wallet = _mapper.Map<Wallet>(model);
           await  _services.Update(wallet);
            return Ok(wallet);

        }
        [HttpGet]
        [Route("Get_All_Wallets")]
        public async Task<ActionResult<List<Wallet>>> GetWallets(int page)
        {
            if (_dbContext.Wallets == null)
                return NotFound();
            var pageResult = 2f;
            var pageCount = Math.Ceiling(_dbContext.Wallets.Count()/ pageResult);
            var wallet = await _dbContext.Wallets 
            .Skip((page - 1) * (int)pageResult)
                .Take((int)pageResult).ToListAsync();
            var responsonse = new paging<Wallet>
            {
                Translist = wallet,
                CurrentPage = page,
                Pages = (int)pageCount
            };
            return Ok(responsonse);

        }
    }
}