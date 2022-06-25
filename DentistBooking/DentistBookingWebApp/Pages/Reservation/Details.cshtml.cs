using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace DentistBookingWebApp.Pages.Reservation
{
    public class DetailsModel : PageModel
    {
        private readonly IReservationRepository reservationRepository;

        public DetailsModel(IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
        }

        [BindProperty]
        public BusinessObject.Reservation Reservation { get; set; }

        public IActionResult OnGet(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            try
            {
                Reservation = reservationRepository.GetReservationById((int)id);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return Page();
        }
    }
}
