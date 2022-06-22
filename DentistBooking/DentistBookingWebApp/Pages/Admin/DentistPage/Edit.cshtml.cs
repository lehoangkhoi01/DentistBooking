﻿using System;
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

namespace DentistBookingWebApp.Pages.Admin.DentistPage
{
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Dentist dentist = dentistRepository.GetDentistByDentistId((int)id);
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
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                dentistRepository.UpdateDentist(new Dentist
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

            return RedirectToPage("./Details", new {id = Dentist.Id});
        }

      
    }
}