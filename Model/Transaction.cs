using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace WalletTransaction.Model
{
    [Table ("Transactions")]
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; } 
        public string TransactionUniqueReference { get; set; }
        public TranStatus TransactionStatus { get; set; }
        public Transtype TransactionType { get; set; }
        public bool IsSuccessful =>  TransactionStatus.Equals(TranStatus.Success);

        public string TransactionSourceAccount { get; set; }
        [Required]
        public string TransactionDestinationAccount { get; set; }
        public string TransactionParticulars{ get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public Wallet Wallet { get; set; }

        public Transaction()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("_","").Substring(1,27)}";
        }
    }
    public enum TranStatus
    {
        Failed,
        Success,
        Error
    }
    public enum Transtype
    {
        Withdraw,
        Deposit,
        Transfer
    }
}
