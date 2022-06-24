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
        [BindProperty]
        public string Date { get; set; }
        [BindProperty]
        public int ServiceId { get; set; }
        [BindProperty]
        public string Time { get; set; }
        [BindProperty]
        public int DentistId { get; set; }



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

        public IActionResult OnPost()
        {
            string email = HttpContext.Session.GetString("EMAIL");


            // ---- Validation -------
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                if(string.IsNullOrEmpty(Date) || string.IsNullOrEmpty(Time))
                {
                    throw new Exception("You must choose date and time to make reservation");
                }

                var dateTimeString = Date + " " + Time;
                DateTime dateTime = DateTime.ParseExact(dateTimeString, "dd-MM-yyyy HH:mm", CultureInfo.CurrentCulture);
                
                Service service = serviceRepository.GetServiceById(ServiceId);
                if (service == null)
                {
                    throw new Exception("This service is not available now. Please choose another.");
                }

                User user = userRepository.GetUserByEmail(email);
                Customer customer = customerRepository.GetCustomerByUserId(user.Id);

                if(!string.IsNullOrEmpty(ValidationReservation(customer.Id, dateTime)))
                {
                    throw new Exception(ValidationReservation(customer.Id, dateTime));
                }
                //---------------------------------------------------------------------

                int dentistId;
                //    // Auto choose random dentist if user not demand specific
                if (DentistId == 0)
                {
                    IEnumerable<Dentist> availableDentists = GetAvailableDentist(dateTime);
                    int indexRandom = new Random().Next(availableDentists.Count());
                    Dentist randomDentist = availableDentists.ElementAt(indexRandom);
                    dentistId = randomDentist.Id;
                }
                else dentistId = DentistId;

                BusinessObject.Reservation reservation = new BusinessObject.Reservation
                {
                    CustomerId = customer.Id,
                    DentistId = dentistId,
                    Price = service.Price,
                    ServiceId = ServiceId,
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

                if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(time))
                {
                    var dateTimeString = date + " " + time;
                    DateTime dateTime = DateTime.ParseExact(dateTimeString, "dd-MM-yyyy HH:mm", CultureInfo.CurrentCulture);
                    dentists = GetAvailableDentist(dateTime);
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
    
        private string ValidateReservationTime(DateTime dateTime)
        {
            var timeDiff = (dateTime - DateTime.Now);
            if (timeDiff.TotalHours < 2) //must make request before at least 2 hour
            {
                return "You should make reservation before at least 2 hours";
            }
            else if (timeDiff.TotalDays > 30) //Can not make reservation before 30 days
            {
                return "Can not make reservation before 30 days";
            }

            return "";
        }

        private string CheckDuplicateReservation(DateTime dateTime, int customerId)
        {
            string error = "";
            try
            {
                BusinessObject.Reservation reservation = reservationRepository.GetReservationByCustomerIdAndDateTime(customerId, dateTime);
                if(reservation != null)
                {
                    error = "You have make a reservation request at this time before. Please choose another date and time to make other reservation.";
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return error;
        }

        private string MaximumAllowReservationValidation(int customerId) {
            string error = "";
            
            try
            {
                IEnumerable<BusinessObject.Reservation> reservations = reservationRepository.GetReservationsByCustomerId(customerId);
                reservations = reservations.Where(r => r.Status == "Waiting");
                if(reservations.Count() >= 3)
                {
                    error = "There are maximum 3 reservations allowed in status of waiting";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return error;
        }
    
        private string ValidationReservation(int customerId, DateTime dateTime)
        {
            string error = "";
            try
            {
                if (!string.IsNullOrEmpty(ValidateReservationTime(dateTime)))
                {
                    error = ValidateReservationTime(dateTime);
                }
                else if (!string.IsNullOrEmpty(CheckDuplicateReservation(dateTime, customerId)))
                {
                    error = CheckDuplicateReservation(dateTime, customerId);
                }
                else if (string.IsNullOrEmpty(MaximumAllowReservationValidation(customerId)))
                {
                    error = MaximumAllowReservationValidation(customerId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return error;            
        }
    }
}
