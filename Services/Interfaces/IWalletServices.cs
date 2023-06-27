using System.Collections.Generic;
using System.Threading.Tasks;
using WalletTransaction.Model;

namespace WalletTransaction.Services.Interfaces
{
    public interface IWalletServices
    {
        Wallet Authenticate(string walletNumber, string password);
        IEnumerable<Wallet> GetAllWallets();
        Wallet GetWalletByWalletNumber(string walletNumber);
        Wallet Create(Wallet wallet, string password, string confirmPassword);
        Task  Update(Wallet wallet);
        void  Delete(int walletId);
       Task < Wallet>  GetByEmail(string Email);
    }
}
