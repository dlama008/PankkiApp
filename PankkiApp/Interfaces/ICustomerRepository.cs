// PankkiApp/Interfaces/ICustomerRepository.cs
using PankkiApp.Models;
using System.Collections.Generic;

namespace PankkiApp.Interfaces
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customer);
        Customer GetCustomerById(int id);
        IEnumerable<Customer> GetAllCustomers();
    }
}
