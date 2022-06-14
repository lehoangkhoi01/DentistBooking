using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using BusinessObject.Data;

namespace DentistBookingWebApp.Pages.Admin.DentistPage
{
    public class DeleteModel : PageModel
    {
        private readonly BusinessObject.Data.DentistBookingContext _context;

        public DeleteModel(BusinessObject.Data.DentistBookingContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dentist = await _context.Dentists.FindAsync(id);

            if (Dentist != null)
            {
                _context.Dentists.Remove(Dentist);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
