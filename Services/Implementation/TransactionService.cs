using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WalletTransaction.DAL;
using WalletTransaction.Model;
using WalletTransaction.Services.Interfaces;
using WalletTransaction.Utility;

namespace WalletTransaction.Services.Implementation
{
    
    public class TransactionService : ITransactions
    {
        private readonly WalletDbContext walletDb;
        ILogger <TransactionService> logger;
        Appsetting setting;
        private static string _OurWalletStatementAccount;
        private readonly IWalletServices walletServices;
        private readonly IGetApi _getApi;
    
        public TransactionService(WalletDbContext walletDb, ILogger<TransactionService > logger, IOptions<Appsetting> setting, IWalletServices walletServices,IGetApi getApi)
        {
            this.walletDb = walletDb;
            this.logger = logger;
            this.setting = setting.Value;
            _OurWalletStatementAccount = this.setting.OurWalletStatementAccount;
            this.walletServices = walletServices;
            _getApi = getApi;
        }

        public async Task <Response>CreateNewTransaction(Transaction transactions)
        {
               Response response = new Response ();
             await walletDb.Transactions.AddAsync(transactions );
             await   walletDb.SaveChangesAsync();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction create sucessful";
            response.Data = null;
            return response;


        }

        public async Task < Response> FindTransactionByDate(DateTime dateTime)
        {
            Response response = new Response();
            var transaction = await  walletDb.Transactions.Where (x=>x.TransactionDate ==dateTime).ToListAsync ();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction create sucessful";
            response.Data = transaction ;
            return response;

        }

        public async Task < Response> MakeDeposit(string walletNumber, double amount, string TransactionPin)
        {
            Response response = new Response();
            //Wallet sourceAccount;
            Wallet destinationAccount;

            Transaction transaction = new Transaction ();
            var authUser = this.walletServices.Authenticate (walletNumber,TransactionPin );
            if (authUser == null)
                throw new ApplicationException("Invalid credentials");
            try
            {
                //sourceAccount = this.walletServices.GetWalletByWalletNumber(_OurWalletStatementAccount);
                destinationAccount = this.walletServices.GetWalletByWalletNumber(walletNumber);

                //sourceAccount.Amount -= amount;
                destinationAccount.Amount += amount;

                if((walletDb.Entry(destinationAccount).State == EntityState.Modified))
                {
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction sucessful";
                    response.Data = null;


                }
                else
                {
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseMessage = "Transaction fail";
                    response.ResponseCode = "02";
                    response.Data = null;
                }
                
               // transaction.tran

               
            }
            catch(Exception ex)
            {
                this.logger.LogError($"AN ERROR OCCURRED=> {ex.Message}");
            }
            transaction.TransactionType = Transtype.Deposit;
            transaction.TransactionSourceAccount = _OurWalletStatementAccount;
            transaction.TransactionDestinationAccount = walletNumber;
            transaction.TransactionAmount = (decimal)amount;
            transaction.TransactionDate = DateTime.Now;

            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $" TO DESTINATION ACCOUNT=>{JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} " +
                $"ON DATE => {transaction.TransactionDate}" +
                $" FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                $" TRANSACTION TYPE =>{JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $" TRANSACTION STATUS => {JsonConvert.SerializeObject (transaction.TransactionStatus)}";

                 await   walletDb.Transactions.AddAsync(transaction);
                    await     walletDb.SaveChangesAsync();



            return response;

        }

        public async Task < Response> MakeTransfer(string fromWallet, string toWallet, double amount, string TransactionPin)
        {
            Response response = new Response();
            Wallet sourceAccount;
            Wallet destinationAccount;

            Transaction transaction = new Transaction();
            var authUser = this.walletServices.Authenticate(fromWallet, TransactionPin);
            if (authUser == null)
                throw new ApplicationException("Invalid credentials");
            try
            {
                sourceAccount = this.walletServices.GetWalletByWalletNumber(fromWallet);
                destinationAccount = this.walletServices.GetWalletByWalletNumber(toWallet);

                sourceAccount.Amount -= amount;
                destinationAccount.Amount += amount;

                if ((walletDb.Entry(sourceAccount).State == EntityState.Modified) &&
                        (walletDb.Entry(destinationAccount).State == EntityState.Modified))
                {
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction sucessful";
                    response.Data = null;


                }
                else
                {
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseMessage = "Transaction fail";
                    response.ResponseCode = "02";
                    response.Data = null;
                }

                // transaction.tran


            }
            catch (Exception ex)
            {
                this.logger.LogError($"AN ERROR OCCURRED=> {ex.Message}");
            }
            transaction.TransactionType = Transtype.Transfer;
            transaction.TransactionSourceAccount = fromWallet ;
            transaction.TransactionDestinationAccount = toWallet ;
            transaction.TransactionAmount = (decimal)amount;
            transaction.TransactionDate = DateTime.Now;

            transaction.TransactionParticulars = $"NEW TRANSACTION FRO" +
                $"M SOURCE => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $" TO DESTINATION ACCOUNT=>{JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} " +
                $"ON DATE => {transaction.TransactionDate}" +
                $" FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                $" TRANSACTION TYPE =>{JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $" TRANSACTION STATUS => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            await walletDb.Transactions.AddAsync(transaction);
            await walletDb.SaveChangesAsync();



            return response;


        }

        public async Task < Response> MakeWithdrawal(string walletNumber, double amount, string TransactionPin)
        {
            Response response = new Response();
            Wallet sourceAccount;
           // Wallet destinationAccount;

            Transaction transaction = new Transaction();
            var authUser = this.walletServices.Authenticate(walletNumber, TransactionPin);
            if (authUser == null)
                throw new ApplicationException("Invalid credentials");
            try
            {
                sourceAccount = this.walletServices.GetWalletByWalletNumber(walletNumber );
               // destinationAccount = this.walletServices.GetWalletByWalletNumber(_OurWalletStatementAccount);

                sourceAccount.Amount -= amount;
               // destinationAccount.Amount += amount;

                if ((walletDb.Entry(sourceAccount).State == EntityState.Modified))
                {
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction sucessful";
                    response.Data = null;


                }
                else
                {
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseMessage = "Transaction fail";
                    response.ResponseCode = "02";
                    response.Data = null;
                }

                // transaction.tran


            }
            catch (Exception ex)
            {
                this.logger.LogError($"AN ERROR OCCURRED=> {ex.Message}");
            }
            transaction.TransactionType = Transtype.Withdraw;
            transaction.TransactionSourceAccount = _OurWalletStatementAccount;
            transaction.TransactionDestinationAccount = walletNumber;
            transaction.TransactionAmount = (decimal)amount;
            transaction.TransactionDate = DateTime.Now;

            transaction.TransactionParticulars = $"NEW TRANSACTION FROM SOURCE => " +
                $"{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $" TO DESTINATION ACCOUNT=>{JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} " +
                $"ON DATE => {transaction.TransactionDate}" +
                $" FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                $" TRANSACTION TYPE =>{JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $" TRANSACTION STATUS => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            await walletDb.Transactions.AddAsync(transaction);
            await walletDb.SaveChangesAsync();



            return response;


        }

        public async  Task<Response> CheckWalletBalance(string walletNumber, string currency)
        {
          var response = new Response();
          var getwallet = walletServices.GetWalletByWalletNumber (walletNumber);
            if (getwallet == null)
                throw new ApplicationException("Invalid Credential");
            var curr =  getwallet.Amount * await _getApi.GetApiAsync(currency);
            response.ResponseMessage = $"Your Balance {currency.ToUpper()} is {curr} ";
            response.ResponseCode = "00";
            
            return response;

        }
    }
}
