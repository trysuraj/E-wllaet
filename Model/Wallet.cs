using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletTransaction.Model
{
    [Table("Wallets")]
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; } 
        public string WalletFirstName { get; set; }
        public string  WalletLastName { get; set; }
        public string WalletName { get; set; }
        public string WalletPassword { get; set; }
        public double Amount { get; set; } = 0;
        public string WalletNumber { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastUpdatedDate { get; set; } 
        public User User { get; set; }

        Random random = new Random();
        public Wallet()
        {
            WalletNumber = Convert.ToString(Math.Floor (random.NextDouble() * 9_000_000_000L + 1_000_000_000L));
            WalletName = $"{WalletLastName}{WalletFirstName}";

        }
    }
}
