using System.ComponentModel.DataAnnotations;
using System;

namespace WalletTransaction.Model
{
    public class GetwalletModel
    {
        [Key]
        public int WalletId { get; set; }
        public string WalletFirstName { get; set; }
        [Required]
        public string WalletLastName { get; set; }
        public string WalletName { get; set; }
        
        public double Amount { get; set; } = 0;
        public string WalletNumber { get; set; }
        public string Email { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastUpdatedDate { get; set; }

    }
}
