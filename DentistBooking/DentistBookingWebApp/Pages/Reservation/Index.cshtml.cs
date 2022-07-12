using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DentistBookingWebApp.Pages.Reservation
{
    [Authorize(Roles = "Customer")]
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



        public async Task<IActionResult> OnGet([FromQuery] int? serviceId)
        {
            var dateTimeNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);

            try
            {
                IEnumerable<Service> services = serviceRepository.GetActiveServiceList();
                IEnumerable<BusinessObject.Dentist> dentists = await GetAvailableDentist(dateTimeNow);

                if(serviceId != null)
                {
                    ViewData["Service"] = new SelectList(services, "Id", "Name", serviceId);
                }
                else ViewData["Service"] = new SelectList(services, "Id", "Name");

                ViewData["DentistList"] = new SelectList(dentists, "Id", "FullName");
                ViewData["TimeList"] = new SelectList(TIME_LIST);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSubmit()
        {
            string userId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var dateTimeString = Date + " " + Time;

            try
            {               
                if (string.IsNullOrEmpty(Date) || string.IsNullOrEmpty(Time))
                {
                    throw new Exception("You must choose date and time to make reservation");
                }

                
                DateTime dateTime = DateTime.ParseExact(dateTimeString, "dd-MM-yyyy HH:mm", CultureInfo.CurrentCulture);                
                Service service = serviceRepository.GetServiceById(ServiceId);
                if (service == null)
                {
                    throw new Exception("This service is not available now. Please choose another.");
                }

                Customer customer = customerRepository.GetCustomerByUserId(int.Parse(userId));

                if(!string.IsNullOrEmpty(await ValidationReservation(customer.Id, dateTime)))
                {
                    throw new Exception(await ValidationReservation(customer.Id, dateTime));
                }
                //---------------------------------------------------------------------

                int dentistId;
                //    // Auto choose random dentist if user not demand specific
                if (DentistId == 0)
                {
                    IEnumerable<BusinessObject.Dentist> availableDentists = await GetAvailableDentist(dateTime);
                    int indexRandom = new Random().Next(availableDentists.Count());
                    BusinessObject.Dentist randomDentist = availableDentists.ElementAt(indexRandom);
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

                await reservationRepository.AddNewReservation(reservation);
                TempData["Message"] = "Make reservation successfully. Keep up checking your reservation status.";
            }
            catch (Exception ex)
            {
                DateTime dateTime = DateTime.ParseExact(dateTimeString, "dd-MM-yyyy HH:mm", CultureInfo.CurrentCulture);
                IEnumerable<Service> services = serviceRepository.GetActiveServiceList();
                IEnumerable<BusinessObject.Dentist> availableDentists = await GetAvailableDentist(dateTime);
                TempData["ErrorMessage"] = ex.Message;
                ViewData["Service"] = new SelectList(services, "Id", "Name");
                ViewData["DentistList"] = new SelectList(availableDentists, "Id", "FullName");
                ViewData["TimeList"] = new SelectList(TIME_LIST);
                return Page();
            }

            return RedirectToPage("/Reservation/History");
        }

        public async Task<IActionResult> OnPostLoadDentist([FromForm] string date, string time)
        {
            IEnumerable<BusinessObject.Dentist> dentists;
            IEnumerable<Service> services;
            try
            {
                services = serviceRepository.GetActiveServiceList();

                if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(time))
                {
                    var dateTimeString = date + " " + time;
                    DateTime dateTime = DateTime.ParseExact(dateTimeString, "dd-MM-yyyy HH:mm", CultureInfo.CurrentCulture);
                    dentists = await GetAvailableDentist(dateTime);
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


        //-------------------------------------------------------------
        private async Task<IEnumerable<BusinessObject.Dentist>> GetAvailableDentist(DateTime dateTime)
        {
            IList<BusinessObject.Dentist> dentists;
            try
            {
                dentists = dentistRepository.GetDentistList().ToList();
                IList<BusinessObject.Dentist> busyDentists = new List<BusinessObject.Dentist>().ToList();

                IEnumerable<BusinessObject.Reservation> reservations = await reservationRepository.GetReservationsByDateTime(dateTime);
                if (reservations.Count() > 0)
                {
                    foreach (var item in reservations)
                    {
                        BusinessObject.Dentist busyDentist = dentists.FirstOrDefault(i => i.Id == item.DentistId);
                        bool result = dentists.Remove(busyDentist);
                    }
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
            if (timeDiff.TotalHours < 24) //must make request before at least 24 hour
            {
                return "You should make reservation before at least 24 hours";
            }
            else if (timeDiff.TotalDays > 30) //Can not make reservation before 30 days
            {
                return "Can not make reservation before 30 days";
            }

            return "";
        }

        private async Task<string> CheckDuplicateReservation(DateTime dateTime, int customerId)
        {
            string error = "";
            try
            {
                BusinessObject.Reservation reservation = await reservationRepository.GetReservationByCustomerIdAndDateTime(customerId, dateTime);
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

        private async Task<string> MaximumAllowReservationValidation(int customerId) {
            string error = "";
            
            try
            {
                IEnumerable<BusinessObject.Reservation> reservations = await reservationRepository.GetReservationsByCustomerId(customerId);
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
    
        private async Task<string> ValidationReservation(int customerId, DateTime dateTime)
        {
            string error = "";
            try
            {
                if (!string.IsNullOrEmpty(ValidateReservationTime(dateTime)))
                {
                    error = ValidateReservationTime(dateTime);
                }
                else if (!string.IsNullOrEmpty(await CheckDuplicateReservation(dateTime, customerId)))
                {
                    error = await CheckDuplicateReservation(dateTime, customerId);
                }
                else if (string.IsNullOrEmpty(await MaximumAllowReservationValidation(customerId)))
                {
                    error = await MaximumAllowReservationValidation(customerId);
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
