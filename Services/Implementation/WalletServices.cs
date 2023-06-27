using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using WalletTransaction.DAL;
using WalletTransaction.Model;
using WalletTransaction.Services.Interfaces;

namespace WalletTransaction.Services.Implementation
{
    public class WalletServices : IWalletServices
    {
        private readonly WalletDbContext _DbContext;
        public WalletServices(WalletDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public Wallet Authenticate(string walletNumber, string password)
        {
          var wallet = _DbContext.Wallets.Where(x => x.WalletNumber == walletNumber).FirstOrDefault();
            if (wallet == null)
                return null;
            

            if (!VerifyPasswordHash (password, wallet.PasswordHash,wallet.PasswordSalt))
                return null;

            return wallet;
        }

        private bool VerifyPasswordHash(string paswword, byte[] passwordHash, byte[] passwordSalt)
        {
            if(string.IsNullOrWhiteSpace(paswword))
                throw new ArgumentNullException("password");
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(paswword));
                for(int i = 0; i < computedPasswordHash.Length; i++)
                {
                    if(computedPasswordHash[i] != passwordHash[i])
                        return false;
                }
            }
            return true;

        }

        public Wallet Create(Wallet wallet, string password, string confirmPassword)
        {
            //if (_DbContext.Wallets.Any(x => x.Email == wallet.Email))
            //    throw new System.ApplicationException("A wallet already exist with this Email");
            if (!password.Equals(confirmPassword))
                throw new ArgumentException("Passwords do not match");

            byte[ ] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            wallet.PasswordHash = passwordHash;
            wallet.PasswordSalt = passwordSalt;
            _DbContext.Add(wallet);
            _DbContext.SaveChanges();
            return wallet;
            
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        { 
            using ( var Hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = Hmac.Key;
                passwordHash =Hmac.ComputeHash (Encoding .UTF8.GetBytes (password));

            }

        }
        public void  Delete(int walletId)
        {
            var wallet = _DbContext.Wallets.Where (x=>x.WalletId ==walletId).FirstOrDefault();
            if (wallet != null)
            {
                _DbContext .Wallets.Remove (wallet);
                _DbContext .SaveChanges();
            }
            
        }

        public IEnumerable<Wallet> GetAllWallets()
        {
            return _DbContext.Wallets.ToList();
        }

        public async Task < Wallet> GetByEmail(string Email)
        {
            var wallet = _DbContext.Wallets.Where(x => x.Email == Email).FirstOrDefaultAsync();
            if (wallet == null)
                return null;
            return await wallet;

        }

        public Wallet GetWalletByWalletNumber(string walletNumber)
        {
            var wallet = _DbContext .Wallets.Where (x=>x.WalletNumber == walletNumber).FirstOrDefault();
            if(wallet == null)
                return null ;
            return wallet;
        }

        public async Task  Update(Wallet wallet)
        {
           var WalletToUpdate =_DbContext .Wallets.Where (x => x.WalletId == wallet.WalletId).FirstOrDefault();
            if (WalletToUpdate != null)
            {
                WalletToUpdate.WalletId = wallet.WalletId;
                WalletToUpdate.WalletNumber = wallet.WalletNumber;
                WalletToUpdate.WalletFirstName= wallet.WalletFirstName;
                WalletToUpdate.WalletLastName= wallet.WalletLastName;
                WalletToUpdate.WalletPassword = wallet.WalletPassword;
                WalletToUpdate.LastUpdatedDate = DateTime.Now;
                WalletToUpdate.Email = wallet.Email;

            }
            _DbContext .Wallets.Update (wallet);
            _DbContext.SaveChanges();

          //if (!string.IsNullOrWhiteSpace(wallet.Email))
          //  {
          //      if (_DbContext.Wallets.Any(x => x.Email == wallet.Email))
          //          throw new ApplicationException($" This Email {wallet.Email } already exist");
          //      WalletToUpdate.Email = wallet.Email;
          //  }

            //if (!string.IsNullOrWhiteSpace(wallet.WalletPassword))
            //{
            //    byte[] passwordHash, passwordSalt;
            //    CreatePasswordHash (wallet.WalletPassword, out passwordHash, out passwordSalt);
            //    passwordHash = wallet.PasswordHash;

            //    passwordSalt = wallet.PasswordSalt;

            //}



        }
    }
}
