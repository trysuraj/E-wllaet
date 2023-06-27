using System.Collections.Generic;

namespace WalletTransaction.Model
{
    public class paging<T>
    {
        public List<T> Translist { get; set; } = new List<T>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
