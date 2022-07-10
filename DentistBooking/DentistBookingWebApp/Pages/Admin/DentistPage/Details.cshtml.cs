using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using BusinessObject.Data;
using DataAccess.Interfaces;
using System.Security.Claims;

namespace DentistBookingWebApp.Pages.Admin.DentistPage
{
    public class DetailsModel : PageModel
    {
        private readonly IUserRepository userRepository;
        private readonly IDentistRepository dentistRepository;

        public DetailsModel(IUserRepository userRepository, IDentistRepository dentistRepository)
        {
            this.userRepository = userRepository;
            this.dentistRepository = dentistRepository;
        }
        [BindProperty]
        public ViewModels.Dentist Dentist { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }

        }
    }
}
