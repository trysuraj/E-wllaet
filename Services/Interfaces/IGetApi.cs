using System.Threading.Tasks;

namespace WalletTransaction.Services.Interfaces
{
    public interface IGetApi
    {
        Task <double > GetApiAsync (string currency);
    }
}
