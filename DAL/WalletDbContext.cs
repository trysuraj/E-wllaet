using Microsoft.EntityFrameworkCore;
using WalletTransaction.Model;

namespace WalletTransaction.DAL
{
    public class WalletDbContext: DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext>options):base(options)
        {
                
        }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
