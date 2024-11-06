// PankkiApp.Tests/BankAccountServiceTests.cs
using Xunit;
using Moq;
using PankkiApp.Interfaces;
using PankkiApp.Models;
using PankkiApp.Services;
using System;

namespace PankkiApp.Tests
{
    public class BankAccountServiceTests
    {
        private readonly Mock<IBankAccountRepository> _mockRepo;
        private readonly BankAccountService _bankAccountService;

        public BankAccountServiceTests()
        {
            _mockRepo = new Mock<IBankAccountRepository>();
            _bankAccountService = new BankAccountService(_mockRepo.Object);
        }

        [Fact]
        public void CreateAccount_ValidAccount_AddsAccount()
        {
            // Arrange
            int accountNumber = 123456;
            int customerId = 1;
            decimal initialBalance = 100m;

            // Act
            _bankAccountService.CreateAccount(accountNumber, customerId, initialBalance);

            // Assert
            _mockRepo.Verify(repo => repo.AddAccount(It.Is<BankAccount>(
                a => a.AccountNumber == accountNumber &&
                     a.CustomerId == customerId &&
                     a.Balance == initialBalance)), Times.Once);
        }


        [Fact]
        public void Deposit_ValidAmount_AddsToBalance()
        {
            // Arrange
            int accountNumber = 123456;
            decimal initialBalance = 0m;
            decimal depositAmount = 50m;
            var account = new BankAccount(accountNumber, 1, initialBalance);
            _mockRepo.Setup(repo => repo.GetAccountByNumber(accountNumber)).Returns(account);

            // Act
            _bankAccountService.Deposit(accountNumber, depositAmount);

            // Assert
            Assert.Equal(initialBalance + depositAmount, account.Balance);
        }

        [Fact]
        public void Deposit_NegativeAmount_ThrowsArgumentException()
        {
            // Arrange
            int accountNumber = 123456;
            decimal initialBalance = 0m;
            decimal depositAmount = -50m;
            var account = new BankAccount(accountNumber, 1, initialBalance);
            _mockRepo.Setup(repo => repo.GetAccountByNumber(accountNumber)).Returns(account);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _bankAccountService.Deposit(accountNumber, depositAmount));
            Assert.Equal("Deposit amount must be positive.", ex.Message);
        }

        [Fact]
        public void Withdraw_ValidAmount_SubtractsFromBalance()
        {
            // Arrange
            int accountNumber = 123456;
            decimal initialBalance = 100m;
            decimal withdrawAmount = 40m;
            var account = new BankAccount(accountNumber, 1, initialBalance);
            _mockRepo.Setup(repo => repo.GetAccountByNumber(accountNumber)).Returns(account);

            // Act
            _bankAccountService.Withdraw(accountNumber, withdrawAmount);

            // Assert
            Assert.Equal(initialBalance - withdrawAmount, account.Balance);
        }

        [Fact]
        public void Withdraw_InsufficientFunds_ThrowsInvalidOperationException()
        {
            // Arrange
            int accountNumber = 123456;
            decimal initialBalance = 30m;
            decimal withdrawAmount = 50m;
            var account = new BankAccount(accountNumber, 1, initialBalance);
            _mockRepo.Setup(repo => repo.GetAccountByNumber(accountNumber)).Returns(account);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _bankAccountService.Withdraw(accountNumber, withdrawAmount));
            Assert.Equal("Insufficient funds.", ex.Message);
        }

        [Fact]
        public void GetBalance_ExistingAccount_ReturnsBalance()
        {
            // Arrange
            int accountNumber = 123456;
            decimal initialBalance = 100m;
            var account = new BankAccount(accountNumber, 1, initialBalance);
            _mockRepo.Setup(repo => repo.GetAccountByNumber(accountNumber)).Returns(account);

            // Act
            var balance = _bankAccountService.GetBalance(accountNumber);

            // Assert
            Assert.Equal(initialBalance, balance);
        }

        [Fact]
        public void GetBalance_NonExistingAccount_ThrowsArgumentException()
        {
            // Arrange
            int nonExistingAccountNumber = 999999;
            _mockRepo.Setup(repo => repo.GetAccountByNumber(nonExistingAccountNumber)).Returns((BankAccount)null);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _bankAccountService.GetBalance(nonExistingAccountNumber));
            Assert.Equal("Account not found.", ex.Message);
        }
    }
}
