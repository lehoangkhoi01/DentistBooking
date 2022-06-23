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
        private readonly ICustomerRepository customerRepository;
        private readonly IUserRepository userRepository;

        public IndexModel(IServiceRepository serviceRepository,
                            IDentistRepository dentistRepository,
                            IReservationRepository reservationRepository,
                            ICustomerRepository customerRepository,
                            IUserRepository userRepository)
        {
            this.serviceRepository = serviceRepository;
            this.dentistRepository = dentistRepository;
            this.reservationRepository = reservationRepository;
            this.customerRepository = customerRepository;
            this.userRepository = userRepository;
        }

        public IList<string> TimeList { get; set; }

        public IActionResult OnGet()
        {
            string roleId = HttpContext.Session.GetString("ROLE");
            if (string.IsNullOrEmpty(roleId))
            {
                return RedirectToPage("/Login");
            }
            else if (roleId != "2")
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

        public IActionResult OnPostMakeReservation([FromForm] string date, string time, int serviceId, int dentistId)
        {
            string email = HttpContext.Session.GetString("EMAIL");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/Login");
            }

            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(time))
            {
                TempData["ErrorMessage"] = "You must choose date and time to make reservation";
                return Page();
            }

            try
            {
                var dateTimeString = date + " " + time;
                DateTime dateTime = DateTime.ParseExact(dateTimeString, "dd-MM-yyyy HH:mm", CultureInfo.CurrentCulture);
                User user = userRepository.GetUserByEmail(email);
                Customer customer = customerRepository.GetCustomerByUserId(user.Id);
                Service service = serviceRepository.GetServiceById(serviceId);
                if(service == null)
                {
                    return NotFound();
                }

                // Auto choose random dentist if user not demand specific
                if(dentistId == 0)
                {
                    IEnumerable<Dentist> availableDentists = GetAvailableDentist(dateTime);
                    int indexRandom = new Random().Next(availableDentists.Count());
                    Dentist randomDentist = availableDentists.ElementAt(indexRandom);
                    dentistId = randomDentist.Id;
                }

                BusinessObject.Reservation reservation = new BusinessObject.Reservation
                {
                    CustomerId = customer.Id,
                    DentistId = dentistId,
                    Price = service.Price,
                    ServiceId = serviceId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    ResevrationDate = dateTime,
                    Status = "Waiting",
                };

                reservationRepository.AddNewReservation(reservation);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostLoadDentist([FromForm] string date, string time)
        {
            IEnumerable<Dentist> dentists;
            IEnumerable<Service> services;
            try
            {
                services = serviceRepository.GetServiceList();
                //dentists = dentistRepository.GetDentistList();

                if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(time))
                {
                    var dateTimeString = date + " " + time;
                    DateTime dateTime = DateTime.ParseExact(dateTimeString, "dd-MM-yyyy HH:mm", CultureInfo.CurrentCulture);
                    dentists = GetAvailableDentist(dateTime);

                    //IList<Dentist> busyDentists = new List<Dentist>().ToList();
                    //IEnumerable<BusinessObject.Reservation> reservations = reservationRepository.GetReservationsByDateTime(dateTime);
                    //if (reservations.Count() > 0)
                    //{
                    //    foreach (var item in reservations)
                    //    {
                    //        busyDentists.Add(item.Dentist);
                    //    }
                    //    dentists = dentists.Where(i => !busyDentists.Contains(i));
                    //}
                    //ViewData["DentistList"] = new SelectList(dentists, "Id", "FullName");
                }
                else
                {
                    dentists = null;
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

        private IEnumerable<Dentist> GetAvailableDentist(DateTime dateTime)
        {
            IEnumerable<Dentist> dentists;
            try
            {
                dentists = dentistRepository.GetDentistList();
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
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dentists;
        }
    }
}
