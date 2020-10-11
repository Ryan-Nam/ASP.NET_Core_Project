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

namespace HotelWebApp.Pages.Rooms
{
    [Authorize(Roles = "Customers")]
    public class IndexModel : PageModel
    {
        private readonly HotelWebApp.Data.ApplicationDbContext _context;

        public IndexModel(HotelWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Room> Room { get;set; }

        //First Step
        [BindProperty(SupportsGet = true)]

        //Second Step, and, go to Content File, use this into name
        public string SearchString { get; set; }
        //Step 2, Modify OnGet
        //Most of search query, we use onGetMethod

        public async Task OnGetAsync()
        {
           

            var Rooms = (IQueryable<Room>)_context.Room;
            if (!String.IsNullOrEmpty(SearchString))
            {
               // Rooms = Rooms.Where(s => s.BedCount.ToString.Contains(SearchString));
                //Customers = Customers.Where(s => s.FamilyName.Contains(SearchString) || s.GivenName.Contains(SearchString));
            }
            Room = await _context.Room.ToListAsync();
            //Customer = await _context.Customer.ToListAsync();
            //Step3, Go to Content File
        }
    }
}
