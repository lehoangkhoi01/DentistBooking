using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;

namespace DentistBookingWebApp.Pages.Account.Details
{
    [Authorize]
    public class CustomerModel : PageModel
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerModel(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public ViewModels.Customer Customer { get; set; }
        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            string role = User.FindFirstValue(ClaimTypes.Role);


            if(role == "Customer" && userId != id)
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
                    User = new ViewModels.User
                    {
                        Id = customer.User.Id,
                        Password = customer.User.Password,
                        RoleId = customer.User.RoleId,
                        Email = customer.User.Email,
                        Role = customer.User.Role
                    }
                };
                
                return Page();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again";
                return RedirectToPage("/Index");
            }
        }
    }
}
