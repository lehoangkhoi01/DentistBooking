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
    public class IndexModel : PageModel
    {
        private readonly BusinessObject.Data.DentistBookingContext _context;

        public IndexModel(BusinessObject.Data.DentistBookingContext context)
        {
            _context = context;
        }

        public IList<Dentist> Dentist { get;set; }

        public async Task OnGetAsync()
        {
            Dentist = await _context.Dentists
                .Include(d => d.User).ToListAsync();
        }
    }
}
