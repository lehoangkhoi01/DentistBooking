using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DentistBookingWebApp.Pages.Reservation
{
    public class DetailsModel : PageModel
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IUserRepository userRepository;
        private readonly IDentistRepository dentistRepository;
        private readonly ICustomerRepository customerRepository;

        public DetailsModel(IReservationRepository reservationRepository,
                            IUserRepository userRepository,
                            IDentistRepository dentistRepository,
                            ICustomerRepository customerRepository)
        {
            this.reservationRepository = reservationRepository;
            this.dentistRepository = dentistRepository;
            this.userRepository = userRepository;
            this.customerRepository = customerRepository;
        }

        [BindProperty]
        public BusinessObject.Reservation Reservation { get; set; }

        [BindProperty]
        public string Status { get; set; }

        public IActionResult OnGet(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            string roleId = HttpContext.Session.GetString("ROLE");
            if(string.IsNullOrEmpty(roleId))
            {
                return RedirectToPage("/Login");
            }

            try
            {                
                Reservation = reservationRepository.GetReservationById((int)id);
                Status = Reservation.Status;
                if(Reservation.ResevrationDate < DateTime.Now)
                {
                    Status = "Invalid";
                }
                if(!AuthorizeForAdminAndChosenDentist(Reservation))
                {
                    return NotFound();
                }
                
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                
            }
            return RedirectToPage("/Reservation/Index");

        }

        public IActionResult OnPostAcceptReservation([FromForm] int reservationId)
        {
            string roleId = HttpContext.Session.GetString("ROLE");
            if(string.IsNullOrEmpty(roleId))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                BusinessObject.Reservation reservation = reservationRepository.GetReservationById(reservationId);
                if(reservation == null)
                {
                    return NotFound();
                }

                AuthorizeForAdminAndChosenDentist(reservation);
                reservation.Status = "Accepted";
                reservationRepository.UpdateReservation(reservation);
                TempData["Message"] = "Update successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("/Reservation/Details", new { id = reservationId });
        }

        public IActionResult OnPostRejectReservation([FromForm] int reservationId, string rejectReason)
        {
            string roleId = HttpContext.Session.GetString("ROLE");
            if(string.IsNullOrEmpty(roleId))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                BusinessObject.Reservation reservation = reservationRepository.GetReservationById(reservationId);
                if (reservation == null)
                {
                    return NotFound();
                }
                AuthorizeForAdminAndChosenDentist(reservation);
                reservation.Status = "Rejected";
                if(!string.IsNullOrEmpty(rejectReason.Trim()))
                {
                    reservation.NoteMessage = rejectReason;
                }
                reservationRepository.UpdateReservation(reservation);
                TempData["Message"] = "Update successfully";
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("/Reservation/Details", new { id = reservationId });
        } 

        private bool AuthorizeForAdminAndChosenDentist(BusinessObject.Reservation reservation)
        {
            string email = HttpContext.Session.GetString("EMAIL");
            string roleId = HttpContext.Session.GetString("ROLE");
            
            
            try
            {
                User user = userRepository.GetUserByEmail(email);
                if(roleId == "2")
                {
                    Customer customer = customerRepository.GetCustomerByUserId(user.Id);
                    if(customer == null || customer.Id != reservation.CustomerId)
                    {
                        return false;
                    }
                }
                else if(roleId == "3")
                {
                    BusinessObject.Dentist dentist = dentistRepository.GetDentistByUserId(user.Id);
                    if (dentist == null || dentist.Id != reservation.DentistId)
                    {
                        return false;
                    }
                }
                 
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }
    }
}
