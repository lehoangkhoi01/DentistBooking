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

        public DetailsModel(IReservationRepository reservationRepository,
                            IUserRepository userRepository,
                            IDentistRepository dentistRepository)
        {
            this.reservationRepository = reservationRepository;
            this.dentistRepository = dentistRepository;
            this.userRepository = userRepository;
        }

        [BindProperty]
        public BusinessObject.Reservation Reservation { get; set; }

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
                AuthorizeForAdminAndChosenDentist(Reservation);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Reservation/Index");
            }
            return Page();
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

        private void AuthorizeForAdminAndChosenDentist(BusinessObject.Reservation reservation)
        {
            string email = HttpContext.Session.GetString("EMAIL");
            string roleId = HttpContext.Session.GetString("ROLE");
            try
            {
                User user = userRepository.GetUserByEmail(email);
                BusinessObject.Dentist dentist = dentistRepository.GetDentistByUserId(user.Id);

                if(roleId != "Admin" || dentist != null || dentist.Id != reservation.Id)
                {
                    NotFound();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
