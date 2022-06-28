using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DentistBookingWebApp.Pages.Dentist
{
    [Authorize(Roles = "Admin, Dentist")]
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

        public IActionResult OnGet(int? id)
        {

            try
            {
                string email = User.FindFirst(claim => claim.Type == ClaimTypes.Name)?.Value;
                string role = User.FindFirst(claim => claim.Type == ClaimTypes.Role)?.Value;
                if(!string.IsNullOrEmpty(email))
                {
                    if (id == null && role == "Dentist")
                    {
                        BusinessObject.Dentist dentist = GetDentist(email);
                        Reservations = reservationRepository.GetReservationsByDentistId(dentist.Id).ToList();
                    }
                    else if (id != null && role == "Admin")
                    {
                        Reservations = reservationRepository.GetReservationsByDentistId((int)id).ToList();
                    }
                    else return NotFound();
                }
                               
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Index");
            }
            return Page();
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
