using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DentistBookingWebApp.Pages.Account.Edit
{
    [Authorize(Roles = "Dentist, Admin")]
    public class DentistModel : PageModel
    {
        private readonly IDentistRepository dentistRepository;
        private readonly IUserRepository userRepository;
        public DentistModel(IDentistRepository dentistRepository,
                                IUserRepository userRepository)
        {
            this.dentistRepository = dentistRepository;
            this.userRepository = userRepository;
        }

        [BindProperty]
        public ViewModels.Dentist Dentist { get; set; }

        [BindProperty]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Maximum length for passwod is 50 characters")]
        public string Password { get; set; }

        [BindProperty]
        public string Email { get; set; }


        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            string role = User.FindFirstValue(ClaimTypes.Role);
            if (role == "Dentist" && userId != id)
            {
                return NotFound();
            }

            try
            {
                BusinessObject.Dentist dentist = dentistRepository.GetDentistByUserId((int)id);
                if (dentist == null)
                {
                    return NotFound();
                }

                Dentist = new ViewModels.Dentist
                {
                    Id = dentist.Id,
                    FullName = dentist.FullName,
                    PhoneNumber = dentist.PhoneNumber,
                    UserId = dentist.UserId,
                };
                Email = dentist.User.Email;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again later.";
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                BusinessObject.Dentist dentist = new BusinessObject.Dentist
                {
                    Id = Dentist.Id,
                    FullName = Dentist.FullName,
                    PhoneNumber = Dentist.PhoneNumber,
                    UserId = Dentist.UserId,
                };
                if (!string.IsNullOrEmpty(Password))
                {
                    User user = userRepository.GetUserById(Dentist.UserId);
                    user.Password = Validation.HashCode.HashPassword(Password);
                    userRepository.Update(user);
                }
                dentistRepository.UpdateDentist(dentist);
                TempData["Message"] = "Update information successfully";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again later.";
            }

            return Page();
        }
    }
}
