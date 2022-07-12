using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;

namespace DentistBookingWebApp.Pages.Account.Details
{
    [Authorize]
    public class DentistModel : PageModel
    {
        private readonly IDentistRepository dentistRepository;
        public DentistModel(IDentistRepository dentistRepository)
        {
            this.dentistRepository = dentistRepository;
        }
        public ViewModels.Dentist Dentist { get; set; }
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
                    User = new ViewModels.User
                    {
                        Id = dentist.User.Id,
                        Password = dentist.User.Password,
                        RoleId = dentist.User.RoleId,
                        Email = dentist.User.Email,
                        Role = dentist.User.Role
                    }
                };

                return Page();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again";
                return RedirectToPage("/Index");
            }
        }
    }
}

