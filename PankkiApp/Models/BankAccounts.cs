// PankkiApp/Models/BankAccount.cs
using System;

namespace PankkiApp.Models
{
    public class BankAccount
    {
        public int AccountNumber { get; set; }
        public int CustomerId { get; set; }
        public decimal Balance { get; private set; }

        // Konstruktor ilman validointia, validointi on siirretty palvelutasolle
        public BankAccount(int accountNumber, int customerId, decimal balance)
        {
            AccountNumber = accountNumber;
            CustomerId = customerId;
            Balance = balance;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive.");
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive.");
            if (Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");
            Balance -= amount;
        }
    }
}
