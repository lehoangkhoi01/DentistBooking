using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace DentistBookingWebApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
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

        public async Task<IActionResult> OnGet()
        {
            Account = userRepository.GetUsers().Count();
            Service = serviceRepository.GetServiceList().Count();
            Reservation = (await reservationRepository.GetReservations()).Count();
            return Page();
        }
    }
}
