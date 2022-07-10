using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using BusinessObject.Data;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DentistBookingWebApp.Pages.Admin.DentistPage
{
    [Authorize]
    public class EditModel : PageModel
    {
        //private readonly IUserRepository userRepository;
        private readonly IDentistRepository dentistRepository;

        public EditModel(/*IUserRepository userRepository,*/ IDentistRepository dentistRepository)
        {
            //this.userRepository = userRepository;
            this.dentistRepository = dentistRepository;
        }

        [BindProperty]
        public ViewModels.Dentist Dentist { get; set; }

        public IActionResult OnGet(int? id)
        {
            string role = User.FindFirst(claim => claim.Type == ClaimTypes.Role)?.Value;
            string userId = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
            {
                id = Int32.Parse(userId);
                BusinessObject.Dentist dent = dentistRepository.GetDentistByUserId((int)id);
                if (dent == null)
                {
                    return NotFound();
                }
                Dentist = new ViewModels.Dentist
                {
                    Id = dent.Id,
                    PhoneNumber = dent.PhoneNumber,
                    FullName = dent.FullName,
                    UserId = dent.UserId,
                    User = dent.User,
                };
                return Page();
            }
            BusinessObject.Dentist dentist = dentistRepository.GetDentistByDentistId((int)id);

            if (userId != dentist.UserId.ToString() && role != "Admin")
            {
                return LocalRedirect("/AccessDenied");
            }
            Dentist = new ViewModels.Dentist
            {
                Id = dentist.Id,
                PhoneNumber = dentist.PhoneNumber,
                FullName = dentist.FullName,
                UserId = dentist.UserId,
                User = dentist.User,
            };


            if (Dentist == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                dentistRepository.UpdateDentist(new BusinessObject.Dentist
                {
                    Id = Dentist.Id,
                    FullName = Dentist.FullName,
                    PhoneNumber = Dentist.PhoneNumber,
                    UserId = Dentist.UserId
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }

            return RedirectToPage("./Details", new { id = Dentist.Id });
        }


    }
}
