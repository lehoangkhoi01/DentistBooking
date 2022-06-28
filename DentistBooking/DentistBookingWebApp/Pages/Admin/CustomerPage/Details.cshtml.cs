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
    public class DetailsModel : PageModel
    {
        private readonly ICustomerRepository customerRepository;

        public DetailsModel(ICustomerRepository customerRepository)
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
            try
            {
                Customer cust = customerRepository.GetCustomerByCustomerId((int)id);
                Customer = new ViewModels.Customer
                {
                    Id = cust.Id,
                    FullName = cust.FullName,
                    PhoneNumber = cust.PhoneNumber,
                    UserId = cust.UserId,
                    User = cust.User
                };
                if (Customer == null)
                {
                    return NotFound();
                }
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }

        }
    }
}
