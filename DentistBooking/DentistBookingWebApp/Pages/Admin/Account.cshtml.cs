using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using BusinessObject.Data;
using DataAccess.Interfaces;

namespace DentistBookingWebApp.Pages.Admin.CustomerPage
{
    public class AccountModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly IDentistRepository dentistRepository;
        private readonly ICustomerRepository customerRepository;


        public AccountModel(IUserRepository userRepository, IDentistRepository dentistRepository, ICustomerRepository customerRepository)
        {
            this.userRepository = userRepository;
            this.dentistRepository = dentistRepository;
            this.customerRepository = customerRepository;
        }

        //public IEnumerable<User> User { get;set; }
        public IEnumerable<Dentist> Dentist { get; set; }
        public IEnumerable<Customer> Customer { get; set; }


        public IActionResult OnGetAsync()
        {
            Customer = customerRepository.GetCustomerList();
            Dentist = dentistRepository.GetDentistList();
            return Page();
        }
    }
}
