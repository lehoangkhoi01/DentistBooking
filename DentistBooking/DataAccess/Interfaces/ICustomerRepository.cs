using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Add new customer
        /// </summary>
        /// <param name="customer"></param>
        public void AddNewCustomer(Customer customer);

        /// <summary>
        /// Get customer by their user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Customer GetCustomerByUserId(int userId);

        /// <summary>
        /// Get customer by their customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer GetCustomerByCustomerId (int customerId);
        public Customer GetCustomerByPhone(string phoneNumber);

        public void UpdateCustomer(Customer customer);

        public IEnumerable<Customer> GetCustomerList();
    }
}
