using DataAccess.Interfaces;
using DataAccess.Repository;
using System;

namespace DentistBookingWebApp.Validation
{
    public static class SignUpValidation
    {
        static IUserRepository userRepository = new UserRepository();
        static ICustomerRepository customerRepository = new CustomerRepository();
        static IDentistRepository dentistRepository = new DentistRepository();
        
        public static bool CheckEmail(String email)
        {
            bool result = false;
            if (userRepository.GetUserByEmail(email) != null)
            {
                result = true;
            }
            return result;
        }
        public static bool CheckPhoneCustomer(String phone)
        {
            bool result = false;
            if (customerRepository.GetCustomerByPhone(phone) != null)
            {
                result = true;
            }
            return result;
        }
        public static bool CheckPhoneDentist(String phoneDentist)
        {
            bool result = false;
            if (dentistRepository.GetDentistByPhone(phoneDentist) != null)
            {
                result = true;
            }
            return result;
        }
    }
}
