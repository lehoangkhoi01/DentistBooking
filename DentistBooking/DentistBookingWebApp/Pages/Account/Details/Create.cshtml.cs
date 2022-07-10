using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using BusinessObject.Data;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using DentistBookingWebApp.Validation;

namespace DentistBookingWebApp.Pages.Admin.CustomerPage
{
    [Authorize(Roles ="Admin")]
    public class CreateModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly ICustomerRepository customerRepository;
        public CreateModel(IUserRepository _userRepository, ICustomerRepository _customerRepository)
        {
            userRepository = _userRepository;
            customerRepository = _customerRepository;
        }

        [BindProperty]
        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password length must be between 6 and 50", MinimumLength = 6)]
        public string Password { get; set; }

        [BindProperty]
        public ViewModels.Customer Customer { get; set; }

       
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            try
            {
                if (SignUpValidation.CheckEmail(Email))
                {
                    ModelState.AddModelError("Email", "This email already exists");
                }
                if (SignUpValidation.CheckPhoneCustomer(Customer.PhoneNumber))
                {
                    ModelState.AddModelError("Customer.PhoneNumber", "This phone number already exists");
                }
                if (ModelState.IsValid)
                {
                    User userObj = new User
                    {
                        Email = Email,
                        Password = Validation.HashCode.HashPassword(Password),
                        RoleId = 2
                    };

                    int userId = userRepository.SignUp(userObj);
                    if (userId > 0)
                    {
                        BusinessObject.Customer customerObj = new BusinessObject.Customer
                        {
                            FullName = Customer.FullName,
                            PhoneNumber = Customer.PhoneNumber,
                            UserId = userId
                        };
                        customerRepository.AddNewCustomer(customerObj);
                        return RedirectToPage("/Admin/Account");
                    }
                    else
                    {
                        ViewData["Message"] = "Cannot add customer!";
                        return Page();
                    }
                }
                else
                {
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return Page();
            }
        }
    }
}
