using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DentistBookingWebApp.Pages.Dentist
{
    public class ScheduleModel : PageModel
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IUserRepository userRepository;
        private readonly IDentistRepository dentistRepository;

        public ScheduleModel(IReservationRepository reservationRepository,
                            IUserRepository userRepository,
                            IDentistRepository dentistRepository)
        {
            this.reservationRepository = reservationRepository;
            this.userRepository = userRepository;
            this.dentistRepository = dentistRepository;
        }

        public IList<BusinessObject.Reservation> Reservations { get; set; }

        public IActionResult OnGet()
        {
            AuthorizeForDentist();

            try
            {
                string email = HttpContext.Session.GetString("EMAIL");
                BusinessObject.Dentist dentist = GetDentist(email);

                Reservations = reservationRepository.GetReservationsByDentistId(dentist.Id).ToList();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Index");
            }
            return Page();
        }

        private void AuthorizeForDentist()
        {
            string role = HttpContext.Session.GetString("ROLE");
            if (string.IsNullOrEmpty(role))
            {
                RedirectToPage("/Login");
            }
            else if (role != "3")
            {
                NotFound();
            }
        }

        private BusinessObject.Dentist GetDentist(string email)
        {
            BusinessObject.Dentist dentist;
            try
            {
                User user = userRepository.GetUserByEmail(email);
                dentist = dentistRepository.GetDentistByUserId(user.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dentist;
        }
    }
}
