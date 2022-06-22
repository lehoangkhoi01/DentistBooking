using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DentistBookingWebApp.Pages.Reservation
{
    public class IndexModel : PageModel
    {
        private readonly IList<string> TIME_LIST = new List<string> { "09:00", "10:00", "11:00", "14:00", "15:00", "16:00" };
        private readonly IServiceRepository serviceRepository;
        private readonly IDentistRepository dentistRepository;
        private readonly IReservationRepository reservationRepository;

        public IndexModel(IServiceRepository serviceRepository, 
                            IDentistRepository dentistRepository,
                            IReservationRepository reservationRepository)
        {
            this.serviceRepository = serviceRepository;
            this.dentistRepository = dentistRepository;
            this.reservationRepository = reservationRepository;
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

            //TimeList = new List<string> { "09:00", "10:00", "11:00", "14:00", "15:00", "16:00"};

            try
            {
                IEnumerable<Service> services = serviceRepository.GetServiceList();
                IEnumerable<Dentist> dentists = dentistRepository.GetDentistList();
                
                ViewData["Service"] = new SelectList(services, "Id", "Name");
                ViewData["DentistList"] = new SelectList(dentists, "Id", "FullName");
                ViewData["TimeList"] = new SelectList(TIME_LIST);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return Page();
        }

        public IActionResult OnPostLoadDentist([FromForm] string date, string time)
        {
            IEnumerable<Dentist> dentists;
            IEnumerable<Service> services;
            try
            {
                services = serviceRepository.GetServiceList();
                dentists = dentistRepository.GetDentistList();

                if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(time))
                {
                    var dateTimeString = date + " " + time;
                    DateTime dateTime = DateTime.ParseExact(dateTimeString, "dd-MM-yyyy HH:mm", CultureInfo.CurrentCulture);
                    IList<Dentist> busyDentists = new List<Dentist>().ToList();
                    IEnumerable<BusinessObject.Reservation> reservations = reservationRepository.GetReservationsByDateTime(dateTime);                                     
                    if (reservations.Count() > 0)
                    {
                        foreach (var item in reservations)
                        {
                            busyDentists.Add(item.Dentist);
                        }
                        dentists = dentists.Where(i => !busyDentists.Contains(i));
                    }
                    //ViewData["DentistList"] = new SelectList(dentists, "Id", "FullName");
                }
                ViewData["DentistList"] = new SelectList(dentists, "Id", "FullName");
                ViewData["Service"] = new SelectList(services, "Id", "Name");
                ViewData["TimeList"] = new SelectList(TIME_LIST, time);
                ViewData["Date"] = date;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            
            return Page();
        }
    }
}
