using DataAccess.Interfaces;
using BusinessObject;
using DentistBookingWebApp.Validation;
using DentistBookingWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using HashCode = DentistBookingWebApp.Validation.HashCode;

namespace DentistBookingWebApp.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly ICustomerRepository customerRepository;
        public RegisterModel(IUserRepository _userRepository, ICustomerRepository _customerRepository)
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
        public ViewModels.Customer customer { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
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
                if (SignUpValidation.CheckPhoneCustomer(customer.PhoneNumber))
                {
                    ModelState.AddModelError("customer.PhoneNumber", "This phone number already exists");
                }
                if (ModelState.IsValid)
                {
                    BusinessObject.User userObj = new BusinessObject.User
                    {
                        Email = Email,
                        Password = HashCode.HashPassword(Password),
                        RoleId = 2
                    };
                   
                    int userId = userRepository.SignUp(userObj);
                    if (userId > 0)
                    {
                        BusinessObject.Customer customerObj = new BusinessObject.Customer
                        {
                            FullName = customer.FullName,
                            PhoneNumber = customer.PhoneNumber,
                            UserId = userId
                        };
                        customerRepository.AddNewCustomer(customerObj);
                        TempData["Message"] = "Signup new account successfully. Please login.";
                        return RedirectToPage("./Login");
                    }
                    else
                    {
                        ViewData["Message"] = "There is an error. Please try again later";
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
