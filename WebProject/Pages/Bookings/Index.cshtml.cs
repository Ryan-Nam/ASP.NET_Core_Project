using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelWebApp.Data;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelWebApp.Pages.Bookings
{
    [Authorize(Roles = "Customers")]

    public class IndexModel : PageModel
    {
        private readonly HotelWebApp.Data.ApplicationDbContext _context;

        public IndexModel(HotelWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get;set; }  

        public async Task OnGetAsync()
        {
            Booking = await _context.Booking
                .Include(b => b.TheCustomer)
                .Include(b => b.TheRoom).Where(p => p.CustomerEmail == User.Identity.Name).ToListAsync();
            //ToListAsynce = immedicate execution, 

            // I added Where.. I can add this WHERE in Prac 3 as well
            // at the end, is it necessary to put .ToListAsync(); I think so
            // Where(p => p.CustomerEmail == User.Identity.Name).ToListAsync();
        }
    }
}
