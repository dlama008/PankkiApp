// PankkiApp/Services/BankAccountService.cs
using PankkiApp.Interfaces;
using PankkiApp.Models;
using System;

namespace PankkiApp.Services
{
    public class BankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        public BankAccountService(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        // Päivitetty CreateAccount-metodi, joka ottaa parametrit
        public void CreateAccount(int accountNumber, int customerId, decimal initialBalance)
        {
            if (accountNumber <= 0)
                throw new ArgumentException("Account number must be positive.");
            if (customerId <= 0)
                throw new ArgumentException("Customer ID must be positive.");
            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative.");

            var account = new BankAccount(accountNumber, customerId, initialBalance);
            _bankAccountRepository.AddAccount(account);
        }

        public void Deposit(int accountNumber, decimal amount)
        {
            var account = _bankAccountRepository.GetAccountByNumber(accountNumber);
            if (account == null)
                throw new ArgumentException("Account not found.");
            account.Deposit(amount);
        }

        public void Withdraw(int accountNumber, decimal amount)
        {
            var account = _bankAccountRepository.GetAccountByNumber(accountNumber);
            if (account == null)
                throw new ArgumentException("Account not found.");
            account.Withdraw(amount);
        }

        public decimal GetBalance(int accountNumber)
        {
            var account = _bankAccountRepository.GetAccountByNumber(accountNumber);
            if (account == null)
                throw new ArgumentException("Account not found.");
            return account.Balance;
        }
    }
}
