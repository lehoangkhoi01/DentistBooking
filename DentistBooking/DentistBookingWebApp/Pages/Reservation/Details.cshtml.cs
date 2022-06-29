using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;

namespace DentistBookingWebApp.Pages.Reservation
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IUserRepository userRepository;
        private readonly IDentistRepository dentistRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IFeedbackRepository feedbackRepository;

        public DetailsModel(IReservationRepository reservationRepository,
                            IUserRepository userRepository,
                            IDentistRepository dentistRepository,
                            ICustomerRepository customerRepository,
                            IFeedbackRepository feedbackRepository)
        {
            this.reservationRepository = reservationRepository;
            this.dentistRepository = dentistRepository;
            this.userRepository = userRepository;
            this.customerRepository = customerRepository;
            this.feedbackRepository = feedbackRepository;
        }

        [BindProperty]
        public BusinessObject.Reservation Reservation { get; set; }

        [BindProperty]
        public Feedback Feedback { get; set; }

        [BindProperty]
        public string Status { get; set; }

        [BindProperty]
        public string Role { get; set; }

        public IActionResult OnGet(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            try
            {
                Role = User.FindFirstValue(ClaimTypes.Role);
                Reservation = reservationRepository.GetReservationById((int)id);
                if(Reservation == null)
                {
                    return NotFound();
                } 

                Feedback = feedbackRepository.GetFeedbackByReservationId(Reservation.Id);
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

        public IActionResult OnPostSendFeedback([FromForm] int reservationId, int rate, string comment)
        {
            string email = User.FindFirst(claim => claim.Type == ClaimTypes.Name)?.Value;
            string userId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            try
            {
                Customer customer = customerRepository.GetCustomerByUserId(int.Parse(userId));
                Feedback prevFeedback = feedbackRepository.GetFeedbackByReservationId(reservationId);
                if (prevFeedback == null)
                {
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
                }
                else
                {
                    prevFeedback.Star = rate;
                    prevFeedback.Comment = comment;
                    prevFeedback.UpdatedDate = DateTime.Now;
                    feedbackRepository.UpdateFeedback(prevFeedback);

                }
                TempData["Message"] = "Send feedback successfully.";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return LocalRedirect("/Reservation/Details?id="+ reservationId);
        }

        private bool AuthorizeForAdminAndChosenDentist(BusinessObject.Reservation reservation)
        {
            string role = User.FindFirst(claim => claim.Type == ClaimTypes.Role)?.Value;
            string userId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            try
            {
                if(role == "Customer")
                {
                    Customer customer = customerRepository.GetCustomerByUserId(int.Parse(userId));
                    if(customer == null || customer.Id != reservation.CustomerId)
                    {
                        return false;
                    }
                }
                else if(role == "Dentist")
                {
                    BusinessObject.Dentist dentist = dentistRepository.GetDentistByUserId(int.Parse(userId));
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
