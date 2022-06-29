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

namespace DentistBookingWebApp.Pages.Reservation
{
    [Authorize(Roles = "Customer")]
    public class HistoryModel : PageModel
    {
        private readonly IReservationRepository reservationRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IUserRepository userRepository;
        private readonly IFeedbackRepository feedbackRepository;
        private const int MAX_ITEM_PAGE = 4;

        public HistoryModel(IReservationRepository reservationRepository, 
                            ICustomerRepository customerRepository, 
                            IUserRepository userRepository,
                            IFeedbackRepository feedbackRepository)
        {
            this.reservationRepository = reservationRepository;
            this.customerRepository = customerRepository;
            this.userRepository = userRepository;
            this.feedbackRepository = feedbackRepository;
        }

        public IList<BusinessObject.Reservation> Reservations { get; set; }

        public IActionResult OnGet([FromQuery] int? page = 1)
        {
            
            string email = User.FindFirst(claim => claim.Type == ClaimTypes.Name)?.Value;
            try
            {
                User user = userRepository.GetUserByEmail(email);
                Customer customer = customerRepository.GetCustomerByUserId(user.Id);
                Reservations = reservationRepository.GetReservationsByCustomerId((int)page, MAX_ITEM_PAGE, customer.Id)
                                                    .ToList();

                
                int pageCount = (int)Math.Ceiling(reservationRepository.GetReservationsByCustomerId(customer.Id).Count() / (double)MAX_ITEM_PAGE);
                if (page <= 0 || page > pageCount)
                {
                   return NotFound();
                }
                ViewData["PageCount"] = pageCount;
                ViewData["CurrentPage"] = page;
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again later";
            }
            return Page();
        }

        public IActionResult OnPostCancelReservation([FromForm] int reservationId)
        {

            string email = User.FindFirst(claim => claim.Type == ClaimTypes.Name)?.Value;
            try
            {
                User user = userRepository.GetUserByEmail(email);
                Customer customer = customerRepository.GetCustomerByUserId(user.Id);
                BusinessObject.Reservation reservation = reservationRepository.GetReservationById(reservationId);
                if(reservation == null || reservation.CustomerId != customer.Id)
                {
                    return NotFound();
                }
                else
                {
                    var timeDiff = (reservation.ResevrationDate - DateTime.Now).TotalDays;
                    if(timeDiff < 1)
                    {
                        TempData["ErrorMessage"] = "Reservation can only be canceled before 1 day.";
                    }
                    reservationRepository.DeleteReservation(reservation);
                    TempData["Message"] = "Cancel reservation successfully.";
                }               
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again later.";
            }
            return RedirectToPage("/Reservation/History");

        }

        public IActionResult OnPostSendFeedback([FromForm] int reservationId, int rate, string comment)
        {
            string email = User.FindFirst(claim => claim.Type == ClaimTypes.Name)?.Value;
            string userId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            try
            {
                Customer customer = customerRepository.GetCustomerByUserId(int.Parse(userId));
                Feedback feedback = new Feedback
                {
                    CustomerId = customer.Id,
                    Star = rate,
                    Comment = comment,
                    ReservationId = reservationId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                feedbackRepository.AddNewFeedback(feedback);
                TempData["Message"] = "Send feedback successfully.";

            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToPage("/Reservation/History");
        }

       
    }
}
