using System.ComponentModel.DataAnnotations;
using System;

namespace WalletTransaction.Model.DTO
{
    public class TransationRequestDto
    {
        
       
        
        public Transtype TransactionType { get; set; }
        
        public string TransactionSourceAccount { get; set; }
        [Required]
        public string TransactionDestinationAccount { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
       
    }
}
