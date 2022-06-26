using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DentistBookingWebApp.Pages.Reservation
{
    public class HistoryModel : PageModel
    {
        private readonly IReservationRepository reservationRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IUserRepository userRepository;
        private const int MAX_ITEM_PAGE = 4;

        public HistoryModel(IReservationRepository reservationRepository, ICustomerRepository customerRepository, IUserRepository userRepository)
        {
            this.reservationRepository = reservationRepository;
            this.customerRepository = customerRepository;
            this.userRepository = userRepository;
        }

        public IList<BusinessObject.Reservation> Reservations { get; set; }

        public void OnGet([FromQuery] int? page = 1)
        {
            AuthorizeForCustomer();
            string email = HttpContext.Session.GetString("EMAIL");
            try
            {
                User user = userRepository.GetUserByEmail(email);
                Customer customer = customerRepository.GetCustomerByUserId(user.Id);
                Reservations = reservationRepository.GetReservationsByCustomerId((int)page, MAX_ITEM_PAGE, customer.Id)
                                                    .ToList();

                
                int pageCount = (int)Math.Ceiling(reservationRepository.GetReservationsByCustomerId(customer.Id).Count() / (double)MAX_ITEM_PAGE);
                if (page <= 0 || page > pageCount)
                {
                    NotFound();
                }
                ViewData["PageCount"] = pageCount;
                ViewData["CurrentPage"] = page;
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        private void AuthorizeForCustomer()
        {
            string roleId = HttpContext.Session.GetString("ROLE");
            if(string.IsNullOrEmpty(roleId))
            {
                RedirectToPage("/Login");
            }
            else if (roleId != "1")
            {
                NotFound();
            }
        }
    }
}
