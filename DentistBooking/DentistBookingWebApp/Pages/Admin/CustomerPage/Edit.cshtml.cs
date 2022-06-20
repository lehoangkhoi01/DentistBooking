using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using BusinessObject.Data;
using DataAccess.Interfaces;

namespace DentistBookingWebApp.Pages.Admin.CustomerPage
{
    public class EditModel : PageModel
    {
        private readonly ICustomerRepository customerRepository;

        public EditModel(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        [BindProperty]
        public ViewModels.Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                customerRepository.UpdateCustomer(new Customer
                {
                    Id = Customer.Id,
                    FullName = Customer.FullName,
                    PhoneNumber = Customer.PhoneNumber,
                    UserId = Customer.UserId
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }

            return RedirectToPage("./Details", new { id = Customer.Id });
        }

    }
}
