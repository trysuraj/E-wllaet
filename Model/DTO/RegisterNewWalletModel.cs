using System;
using System.ComponentModel.DataAnnotations;

namespace WalletTransaction.Model.DTO
{
    public class RegisterNewWalletModel
    {

        public string WalletLastName { get; set; }
        public string WalletFirstName { get; set; }
        public string WalletName { get; set; }
        [Required]
        // [RegularExpression ("[A-Za-z0-9!#%]{8,32}",ErrorMessage ="password must be between 8 and 32 letters")]
        public string WalletPassword { get; set; }
        [Required]
        [Compare("WalletPassword", ErrorMessage = "Wallet password must match")]
        public string ConfirmPassword { get; set; }
        public double Amount { get; set; }

        public string Email { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastUpdatedDate { get; set; }

    }
}
