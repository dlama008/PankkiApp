// PankkiApp.Tests/CustomerServiceTests.cs
using Xunit;
using Moq;
using PankkiApp.Interfaces;
using PankkiApp.Models;
using PankkiApp.Services;
using System;

namespace PankkiApp.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockRepo;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_mockRepo.Object);
        }

        [Fact]
        public void RegisterCustomer_ValidCustomer_AddsCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Matti Meikäläinen", Email = "matti@example.com" };

            // Act
            _customerService.RegisterCustomer(customer);

            // Assert
            _mockRepo.Verify(repo => repo.AddCustomer(customer), Times.Once);
        }

        [Theory]
        [InlineData("", "matti@example.com")]
        [InlineData("Matti", "")]
        public void RegisterCustomer_InvalidCustomer_ThrowsArgumentException(string name, string email)
        {
            // Arrange
            var customer = new Customer { Name = name, Email = email };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _customerService.RegisterCustomer(customer));
            if (string.IsNullOrWhiteSpace(name))
                Assert.Equal("Customer name cannot be empty.", ex.Message);
            else if (string.IsNullOrWhiteSpace(email))
                Assert.Equal("Customer email cannot be empty.", ex.Message);
        }

        [Fact]
        public void GetCustomer_ExistingId_ReturnsCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Matti Meikäläinen", Email = "matti@example.com" };
            _mockRepo.Setup(repo => repo.GetCustomerById(1)).Returns(customer);

            // Act
            var result = _customerService.GetCustomer(1);

            // Assert
            Assert.Equal("Matti Meikäläinen", result.Name);
        }

        [Fact]
        public void GetCustomer_NonExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetCustomerById(2)).Returns((Customer)null);

            // Act
            var result = _customerService.GetCustomer(2);

            // Assert
            Assert.Null(result);
        }


        [Theory]
        [InlineData("Matti Meikäläinen", "invalid-email")] // invalid email
        [InlineData("", "matti@example.com")] // empty name
        [InlineData("Matti", "")] // empty email
        public void RegisterCustomer_MultipleScenarios_ThrowsArgumentException(string name, string email)
        {
            // Arrange
            var customer = new Customer { Name = name, Email = email };

            // Act & Assert
            if (string.IsNullOrWhiteSpace(name))
            {
                var ex = Assert.Throws<ArgumentException>(() => _customerService.RegisterCustomer(customer));
                Assert.Equal("Customer name cannot be empty.", ex.Message);
            }
            else if (string.IsNullOrWhiteSpace(email))
            {
                var ex = Assert.Throws<ArgumentException>(() => _customerService.RegisterCustomer(customer));
                Assert.Equal("Customer email cannot be empty.", ex.Message);
            }
            else if (!IsValidEmail(email)) // Check for valid email format
            {
                var ex = Assert.Throws<ArgumentException>(() => _customerService.RegisterCustomer(customer));
                Assert.Equal("Customer email is not valid.", ex.Message);
            }
            else
            {
                // For valid case, we simply want to call the method without exceptions
                _customerService.RegisterCustomer(customer);
                _mockRepo.Verify(repo => repo.AddCustomer(customer), Times.Once);
            }
        }



        // Helper method to validate email format (could be replaced with a better validation)
        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }



    }
}
