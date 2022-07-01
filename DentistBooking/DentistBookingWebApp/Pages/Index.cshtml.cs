using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistBookingWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IServiceRepository serviceRepository;
        private const int MAX_ITEM_PAGE = 3;

        public IndexModel(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        public IList<Service> Services { get; set; }

        public IActionResult OnGet()
        {
            try
            {
                Services = serviceRepository.GetActiveServiceList()
                                            .Take(MAX_ITEM_PAGE)
                                            .ToList();
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again later.";
            }
            return Page();
        }

        public async Task<IActionResult> OnGetLogOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
    }
}
