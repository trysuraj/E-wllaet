using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalletTransaction.Model;


namespace WalletTransaction.Services.Interfaces
{
    public interface ITransactions
    {
       Task <Response>CreateNewTransaction(Transaction transactions);
       Task <Response>FindTransactionByDate(DateTime dateTime);
       Task < Response> MakeDeposit(string walletNumber,double amount, string TransactionPin);
       Task < Response> MakeWithdrawal (string walletNumber,double amount,string TransactionPin);
       Task < Response> MakeTransfer(string fromWallet,string toWallet,double amount,string TransactionPin);
       Task <Response> CheckWalletBalance(string walletNumber, string currency);
    }
}
