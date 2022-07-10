using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace DentistBookingWebApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ServicesModel : PageModel
    {
        private readonly IServiceRepository serviceRepository;
       
        public ServicesModel(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        public IEnumerable<Service> Services { get; set; }
        public IActionResult OnGetAsync()
        {
            Services = serviceRepository.GetServiceList();
            return Page();
        }

    }
}
