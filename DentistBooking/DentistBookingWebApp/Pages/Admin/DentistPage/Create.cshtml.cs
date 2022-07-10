using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject;
using BusinessObject.Data;
using DataAccess.Interfaces;
using HashCode = DentistBookingWebApp.Validation.HashCode;
using DentistBookingWebApp.Validation;
using Microsoft.AspNetCore.Authorization;

namespace DentistBookingWebApp.Pages.Admin.DentistPage
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IDentistRepository dentistRepository;
        private readonly IUserRepository userRepository;

        public CreateModel(IDentistRepository _dentist, IUserRepository _user)
        {
            dentistRepository = _dentist;
            userRepository = _user;
        }

        [BindProperty]
        public ViewModels.Dentist Dentist { get; set; }
        [BindProperty]
        public ViewModels.User User { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostAsync()
        {
            try
            {
                if (SignUpValidation.CheckEmail(User.Email))
                {
                    ModelState.AddModelError("User.Email", "This email already exists");
                }
                if (SignUpValidation.CheckPhoneDentist(Dentist.PhoneNumber))
                {
                    ModelState.AddModelError("Dentist.PhoneNumber", "This phone number already exists");
                }
                if (ModelState.IsValid)
                {
                    BusinessObject.User userObj = new BusinessObject.User
                    {
                        Email = User.Email,
                        Password = HashCode.HashPassword(User.Password),
                        RoleId = 3
                    };
                    int userId = userRepository.SignUp(userObj);
                    if (userId > 0)
                    {
                        BusinessObject.Dentist dentist = new BusinessObject.Dentist
                        {
                            UserId = userId,
                            FullName = Dentist.FullName,
                            PhoneNumber = Dentist.PhoneNumber
                        };
                        dentistRepository.AddNewDentist(dentist);
                        return RedirectToPage("/Admin/Account");
                    }
                }
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return Page();
            }

        }
    }
}
