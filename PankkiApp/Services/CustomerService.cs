using PankkiApp.Interfaces;
using PankkiApp.Models;
using System;

namespace PankkiApp.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public void RegisterCustomer(Customer customer)
        {
            // Tarkistetaan asiakkaan nimi
            if (string.IsNullOrWhiteSpace(customer.Name))
                throw new ArgumentException("Customer name cannot be empty.");

            // Tarkistetaan asiakkaan sähköposti
            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new ArgumentException("Customer email cannot be empty.");

            // Tarkistetaan, onko sähköposti kelvollinen
            if (!IsValidEmail(customer.Email))
                throw new ArgumentException("Customer email is not valid.");

            // Lisää asiakas repositorioon
            _repository.AddCustomer(customer);
        }

        public Customer GetCustomer(int id)
        {
            return _repository.GetCustomerById(id);
        }

        // Apumetodi sähköpostin kelpoisuuden tarkistamiseksi
        private bool IsValidEmail(string email)
        {
            // Voit laajentaa tätä tarkistusta kattamaan useampia ehtoja
            return !string.IsNullOrEmpty(email) && email.Contains("@");
        }
    }
}
