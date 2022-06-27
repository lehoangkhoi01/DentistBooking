using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace DentistBookingWebApp.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly IServiceRepository serviceRepository;
        private readonly IReservationRepository reservationRepository;

        public DashboardModel(IUserRepository userRepository, IServiceRepository serviceRepository, IReservationRepository reservationRepository)
        {
            this.userRepository = userRepository;
            this.serviceRepository = serviceRepository;
            this.reservationRepository = reservationRepository;
        }

        public int Account { get; set; }
        public int Service { get; set; }
        public int Reservation { get; set; }

        public IActionResult OnGet()
        {
            Account = userRepository.GetUsers().Count();
            Service = serviceRepository.GetServiceList().Count();
            Reservation = reservationRepository.GetReservations().Count();
            return Page();
        }
    }
}
