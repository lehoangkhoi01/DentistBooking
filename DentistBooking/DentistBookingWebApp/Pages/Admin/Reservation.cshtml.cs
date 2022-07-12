using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistBookingWebApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ReservationModel : PageModel
    {
        private readonly IReservationRepository reservationRepository;

        public ReservationModel(IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
        }

        public IList<BusinessObject.Reservation> Reservations { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Reservations = (await reservationRepository.GetReservations()).ToList();
            }
            catch(Exception ex)
            {
                TempData["Message"] = "There is an error. Please try again later";
            }
            
            return Page();
        }
    }
    
}
