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
using Microsoft.AspNetCore.Authorization;

namespace DentistBookingWebApp.Pages.Admin.CustomerPage
{
    [Authorize(Roles = "Admin")]
    public class AccountModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly IDentistRepository dentistRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IAdminRepository adminRepository;

        public AccountModel(IUserRepository userRepository, IDentistRepository dentistRepository, ICustomerRepository customerRepository, IAdminRepository adminRepository)
        {
            this.userRepository = userRepository;
            this.dentistRepository = dentistRepository;
            this.customerRepository = customerRepository;
            this.adminRepository = adminRepository;
        }

        //public IEnumerable<User> User { get;set; }
        public IEnumerable<BusinessObject.Dentist> Dentist { get; set; }
        public IEnumerable<Customer> Customer { get; set; }
        public IEnumerable<BusinessObject.Admin> Admin { get; set; }

        public IActionResult OnGetAsync()
        {
            try
            {
                Customer = customerRepository.GetCustomerList();
                Dentist = dentistRepository.GetDentistList();
                Admin = adminRepository.GetAllAdmins();
                return Page();
            }
            catch(Exception ex)
            {
                TempData["Message"] = "There is an error. Please try again later";
                return RedirectToPage("/Index");
            }
            
            
        }
    }
}
