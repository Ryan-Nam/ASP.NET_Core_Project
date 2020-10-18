using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelWebApp.Data;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelWebApp.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HotelWebApp.Pages.Bookings
{
    //[Authorize(Roles = "Customers")]

    public class IndexModel : PageModel
    {
        private readonly HotelWebApp.Data.ApplicationDbContext _context;

        public IndexModel(HotelWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        
        public IList<Booking> Booking { get;set; }
        /*
        Original one
        public async Task OnGetAsync()
        {
            Booking = await _context.Booking
                .Include(b => b.TheCustomer)
                .Include(b => b.TheRoom).Where(p => p.CustomerEmail == User.Identity.Name).ToListAsync();
        */
        //ToListAsynce = immedicate execution, 

        // I added Where.. I can add this WHERE in Prac 3 as well
        // at the end, is it necessary to put .ToListAsync(); I think so
        // Where(p => p.CustomerEmail == User.Identity.Name).ToListAsync();

        public async Task<IActionResult> OnGetAsync(string sortOrder)
        {

            if (String.IsNullOrEmpty(sortOrder))
            {
                // When the Index page is loaded for the first time, the sortOrder is empty.
                // By default, the bookings should be displayed in the order of check_in_asc
                sortOrder = "check_in_asc";
            }

            var bookings = (IQueryable<Booking>)_context.Booking;


            switch (sortOrder)
            {
                case "check_in_asc":
                    bookings = bookings.OrderBy(p => p.CheckIn);
                    break;
                case "check_in_desc":
                    bookings = bookings.OrderByDescending(p => p.CheckIn);
                    break;
                case "price_asc":
                    bookings = bookings.OrderBy(p => (double)p.Cost);
                    break;
                case "price_desc":
                    bookings = bookings.OrderByDescending(p => (double)p.Cost);
                    break;
               
            }

         
            ViewData["NextCheckInOrder"] = sortOrder != "check_in_asc" ? "check_in_asc" : "check_in_desc";
            ViewData["NextCostOrder"] = sortOrder != "price_asc" ? "price_asc" : "price_desc";


            string _email = User.FindFirst(ClaimTypes.Name).Value;
            Booking order = await _context.Booking.FirstOrDefaultAsync(m => m.CustomerEmail == _email);

            //where, let only the bookings by this customer should be displayed.
            Booking = await bookings.AsNoTracking()
                .Include(p => p.TheCustomer)
                .Include(p => p.TheRoom).Where(p => p.CustomerEmail == _email).ToListAsync();

            return Page();
        }
    }
}

