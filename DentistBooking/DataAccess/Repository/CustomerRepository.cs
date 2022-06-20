using BusinessObject;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public void AddNewCustomer(Customer customer) => CustomerDAO.Instance.AddNewCustomer(customer);

        public Customer GetCustomerByCustomerId(int customerId) => CustomerDAO.Instance.GetCustomerByCustomerId(customerId);

        public Customer GetCustomerByPhone(string phoneNumber) => CustomerDAO.Instance.GetCustomerByPhone(phoneNumber);

        public Customer GetCustomerByUserId(int userId) => CustomerDAO.Instance.GetCustomerByUserId(userId);

        public IEnumerable<Customer> GetCustomerList() => CustomerDAO.Instance.GetCustomerList();

        public void UpdateCustomer(Customer customer) => CustomerDAO.Instance.UpdateCustomer(customer);
    }
}
