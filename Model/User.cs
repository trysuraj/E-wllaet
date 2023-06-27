using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace WalletTransaction.Model
{
    public class User
    {
        public Guid userId { get; set; } = new Guid();
        public string firstName { get; set; }
        public string lastName { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public string password { get; set; }
        ICollection<Wallet> WalletList { get; set; }


        public string FullName()
        {
              return firstName + lastName; 
        }

    }
}
