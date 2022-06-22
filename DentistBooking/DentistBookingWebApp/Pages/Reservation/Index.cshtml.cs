using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DentistBookingWebApp.Pages.Reservation
{
    public class IndexModel : PageModel
    {
        private readonly IServiceRepository serviceRepository;
        private readonly IDentistRepository dentistRepository;

        public IndexModel(IServiceRepository serviceRepository, 
                            IDentistRepository dentistRepository)
        {
            this.serviceRepository = serviceRepository;
            this.dentistRepository = dentistRepository;
        }

        public IList<string> TimeList { get; set; }

        public IActionResult OnGet()
        {
            string roleId = HttpContext.Session.GetString("ROLE");
            if(string.IsNullOrEmpty(roleId))
            {
                return RedirectToPage("/Login");
            }
            else if(roleId != "2")
            {
                return NotFound();
            }

            TimeList = new List<string> { "09:00", "10:00", "11:00", "14:00", "15:00", "16:00"};

            try
            {
                IEnumerable<Service> services = serviceRepository.GetServiceList();
                IEnumerable<Dentist> dentists = dentistRepository.GetDentistList();
                
                ViewData["Service"] = new SelectList(services, "Id", "Name");
                ViewData["DentistList"] = new SelectList(dentists, "Id", "FullName");
                ViewData["TimeList"] = new SelectList(TimeList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return Page();
        }

        public IActionResult OnPostLoadDentist([FromForm] string date, string time)
        {
            date = "Array";
            return Page();
        }
    }
}
