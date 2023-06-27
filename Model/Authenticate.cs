using System.ComponentModel.DataAnnotations;

namespace WalletTransaction.Model
{
    public class Authenticate
    {
        [Required ]
        [RegularExpression ("/^\\d{10}$/")]
        public string WalletNumber { get; set; }

        public string Password { get; set; }
    }
}
