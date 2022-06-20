using BusinessObject;
using BusinessObject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CustomerDAO
    {
        private static CustomerDAO instance = null;
        private static readonly object instanceLock = new object();

        private CustomerDAO() { }
        public static CustomerDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CustomerDAO();
                    }
                }
                return instance;
            }
        }
        //----------------------------------------------------------

        public void AddNewCustomer(Customer customer)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Customers.AddAsync(customer);
                dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Customer GetCustomerByUserId(int id)
        {
            Customer customer;
            try
            {
                var dbContext = new DentistBookingContext();
                customer = dbContext.Customers.Include(c => c.User).FirstOrDefault(x => x.UserId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return customer;
        }

        public Customer GetCustomerByCustomerId(int id)
        {
            Customer customer;
            try
            {
                var dbContext = new DentistBookingContext();
                customer = dbContext.Customers.Include(c => c.User).FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return customer;
        }
        public Customer GetCustomerByPhone(string phone)
        {
            Customer customer;
            try
            {
                var dbContext = new DentistBookingContext();
                customer = dbContext.Customers.Include(c => c.User).FirstOrDefault(x => x.PhoneNumber == phone);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return customer;
        }

        public void UpdateCustomer(Customer customer)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Customers.Update(customer);
                dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Customer> GetCustomerList()
        {
            IEnumerable<Customer> customerList;
            try
            {
                var dbContext = new DentistBookingContext();
                customerList = dbContext.Customers.Include(c => c.User.Role).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return customerList;
        }
    }
}
