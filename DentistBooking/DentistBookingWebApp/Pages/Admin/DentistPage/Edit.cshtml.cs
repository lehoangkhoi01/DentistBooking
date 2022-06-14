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

namespace DentistBookingWebApp.Pages.Admin.DentistPage
{
    public class EditModel : PageModel
    {
        private readonly BusinessObject.Data.DentistBookingContext _context;

        public EditModel(BusinessObject.Data.DentistBookingContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Dentist Dentist { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dentist = await _context.Dentists
                .Include(d => d.User).FirstOrDefaultAsync(m => m.Id == id);

            if (Dentist == null)
            {
                return NotFound();
            }
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
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

            _context.Attach(Dentist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DentistExists(Dentist.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DentistExists(int id)
        {
            return _context.Dentists.Any(e => e.Id == id);
        }
    }
}
