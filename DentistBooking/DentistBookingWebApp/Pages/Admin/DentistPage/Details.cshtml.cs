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
                if (id == null)
                {
                    return NotFound();
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
