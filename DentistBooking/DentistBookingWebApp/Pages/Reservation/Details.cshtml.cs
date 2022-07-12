using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public string ReservationStatus { get; set; }

        [BindProperty]
        public string Role { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            try
            {
                Role = User.FindFirstValue(ClaimTypes.Role);
                Reservation = await reservationRepository.GetReservationById((int)id);
                if(Reservation == null)
                {
                    return NotFound();
                } 

                Feedback = feedbackRepository.GetFeedbackByReservationId(Reservation.Id);
                Status = Reservation.Status;
                ReservationStatus = Reservation.Status;
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
            return LocalRedirect("/Reservation/Index");

        }

        
        public async Task<IActionResult> OnPostAcceptReservation([FromForm] int reservationId)
        {
            try
            {
                BusinessObject.Reservation reservation = await reservationRepository.GetReservationById(reservationId);
                if(reservation == null)
                {
                    return NotFound();
                }

                AuthorizeForAdminAndChosenDentist(reservation);
                reservation.Status = "Accepted";
                await reservationRepository.UpdateReservation(reservation);
                TempData["Message"] = "Update successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return LocalRedirect("/Reservation/Details?id="+reservationId);
        }

        public async Task<IActionResult> OnPostCompleteReservation([FromForm] int reservationId)
        {
            try
            {
                BusinessObject.Reservation reservation = await reservationRepository.GetReservationById(reservationId);
                if (reservation == null)
                {
                    return NotFound();
                }

                AuthorizeForAdminAndChosenDentist(reservation);
                reservation.Status = "Completed";
                await reservationRepository.UpdateReservation(reservation);
                TempData["Message"] = "Update successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return LocalRedirect("/Reservation/Details?id=" + reservationId);
        }

        public async Task<IActionResult> OnPostRejectReservation([FromForm] int reservationId, string rejectReason)
        {

            try
            {
                BusinessObject.Reservation reservation = await reservationRepository.GetReservationById(reservationId);
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
                await reservationRepository.UpdateReservation(reservation);
                TempData["Message"] = "Update successfully";
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return LocalRedirect("/Reservation/Details?id=" + reservationId);
        }

        public async Task<IActionResult> OnPostCancelReservation([FromForm] int reservationId, string rejectReason)
        {
            try
            {
                BusinessObject.Reservation reservation = await reservationRepository.GetReservationById(reservationId);
                if (reservation == null)
                {
                    return NotFound();
                }
                var timeDiff = DateTime.Now - reservation.ResevrationDate;
                if(timeDiff.TotalHours < 1)
                {
                    TempData["ErrorMessage"] = "Only cancel customer's reservation if customer is late for 1 hour.";
                    return LocalRedirect("/Reservation/Details?id=" + reservationId);
                }
                AuthorizeForAdminAndChosenDentist(reservation);
                reservation.Status = "Canceled";
                if (!string.IsNullOrEmpty(rejectReason.Trim()))
                {
                    reservation.NoteMessage = rejectReason;
                }
                await reservationRepository.UpdateReservation(reservation);
                TempData["Message"] = "Cancel successfully";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again later";
            }
            return LocalRedirect("/Reservation/Details?id=" + reservationId);
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

        public async Task<IActionResult> OnPostDeleteFeedback([FromForm] int reservationId)
        {
            string role = User.FindFirst(claim => claim.Type == ClaimTypes.Role)?.Value;
            string userId = User.FindFirst(claim => claim.Type == ClaimTypes.Role)?.Value;
            if(role != "Customer")
            {
                return LocalRedirect("/AccessDenied");
            }
            try
            {
                BusinessObject.Reservation reservation = await reservationRepository.GetReservationById(reservationId);
                Feedback feedback = feedbackRepository.GetFeedbackByReservationId(reservationId);
                if(feedback != null)
                {
                    feedbackRepository.DeleteFeedback(feedback);
                    TempData["Message"] = "Remove feedback successfully";
                }
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return LocalRedirect("/Reservation/Details?id=" + reservationId);
        }
        
        //-------------------------------------------------------------------------------
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
