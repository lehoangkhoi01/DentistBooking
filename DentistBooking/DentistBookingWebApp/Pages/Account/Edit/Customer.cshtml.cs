using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using DentistBookingWebApp.Validation;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DentistBookingWebApp.Pages.Account.Edit
{
    [Authorize(Roles = "Customer")]
    public class CustomerModel : PageModel
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IUserRepository userRepository;
        public CustomerModel(ICustomerRepository customerRepository,
                                IUserRepository userRepository)
        {
            this.customerRepository = customerRepository;
            this.userRepository = userRepository;
        }

        [BindProperty]
        public ViewModels.Customer Customer { get; set; }

        [BindProperty]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Maximum length for passwod is 50 characters")]
        public string Password { get; set; }

        [BindProperty]
        public string Email { get; set; }
        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (userId != id)
            {
                return NotFound();
            }
            try
            {
                Customer customer = customerRepository.GetCustomerByUserId((int)id);
                if (customer == null)
                {
                    return NotFound();
                }

                Customer = new ViewModels.Customer
                {
                    Id = customer.Id,
                    FullName = customer.FullName,
                    PhoneNumber = customer.PhoneNumber,
                    UserId = customer.UserId,
                };
                Email = customer.User.Email;

            }
            catch(Exception)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again later.";
            }
            
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Customer customer = new Customer
                {
                    Id = Customer.Id,
                    FullName = Customer.FullName,
                    PhoneNumber = Customer.PhoneNumber,
                    UserId = Customer.UserId,
                };
                if(!string.IsNullOrEmpty(Password))
                {
                    User user = userRepository.GetUserById(Customer.UserId);
                    user.Password = Validation.HashCode.HashPassword(Password);
                    userRepository.Update(user);
                }
                customerRepository.UpdateCustomer(customer);
                TempData["Message"] = "Update information successfully";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again later.";
            }

            return Page();
        }


    }
}
