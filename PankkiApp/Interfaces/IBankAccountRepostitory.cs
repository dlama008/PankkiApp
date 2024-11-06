// PankkiApp/Interfaces/IBankAccountRepository.cs
using PankkiApp.Models;
using System.Collections.Generic;

namespace PankkiApp.Interfaces
{
    public interface IBankAccountRepository
    {
        void AddAccount(BankAccount account);
        BankAccount GetAccountByNumber(int accountNumber);
        IEnumerable<BankAccount> GetAccountsByCustomerId(int customerId);
    }
}
