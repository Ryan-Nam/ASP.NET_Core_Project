using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelWebApp.Pages.Bookings
{
    [Authorize(Roles = "Customers")]
    public class SearchRoomsModel : PageModel
    {
        private readonly HotelWebApp.Data.ApplicationDbContext _context;
        public SearchRoomsModel(HotelWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        // need 'using <ProjectName>.Models'
        public RoomSearchModel roomSearch { get; set; }

        // List of different movies; for passing to Content file to display
        public IList<Room> DiffMovies { get; set; }


        // GET: MovieGoers/PeopleDiff

        // Search Rooms
        // GET: Bookings/SearchRooms
        public IActionResult OnGet()
        {
            // Get the options for the MovieGoer select list from the database
            // and save them in ViewData for passing to Content file
            //ViewData["MovieGoerList"] = new SelectList(_context.Customer, "Email", "FullName");
            ViewData["BedCountList"] = new SelectList(_context.Room, "BedCount", "BedCount");
          
            return Page();
        }



        /*
       public void OnGet()
       {

       }
       */


        // SearchRooms
        // POST: Bookings/SearchRooms
     
        public async Task<IActionResult> OnPostAsync()
        {
            // prepare the parameters to be inserted into the query
            var totalBeds = new SqliteParameter("bedCount", roomSearch.BedCount);
            var cInDate = new SqliteParameter("checkIn", roomSearch.CheckIn);
            var cOutDate = new SqliteParameter("checkOut", roomSearch.CheckOut);

            // Construct the query to get the movies watched by Moviegoer A but not Moviegoer B
            // Use placeholders as the parameters
          
            /*
            var diffMovies = _context.Room.FromSqlRaw("select * from [Room] "
                + "where [Room].BedCount = @bedCount and [Room].ID not in "
                 + "(select [Room].ID from [Room] inner join [Booking] on [Room].ID = [Booking].RoomID "
                  + "where ([Booking].CheckIn <= @checkIn and [Booking].CheckOut >= @checkIn)"
                   + "or ([Booking].CheckIn >= @checkIn and[Booking].CheckOut <= @checkOut)"
                      + "or ([Booking].CheckIn <= @checkOut and [Booking].CheckOut >= @checkOut))", totalBeds, cInDate, cOutDate).Select(ro => new Room { ID = ro.ID, Level = ro.Level, BedCount = ro.BedCount, Price = ro.Price });
            */

            var diffMovies = _context.Room.FromSqlRaw("select * from [Room] "
               + "where [Room].BedCount = @bedCount and [Room].ID not in "
                + "(select [Room].ID from [Room] inner join [Booking] on [Room].ID = [Booking].RoomID "
                 + "where ([Booking].CheckIn <= @checkIn and [Booking].CheckOut >= @checkIn)"
                  + "or ([Booking].CheckIn >= @checkIn and[Booking].CheckOut <= @checkOut)"
                     + "or ([Booking].CheckIn <= @checkOut and [Booking].CheckOut >= @checkOut))", totalBeds, cInDate, cOutDate).Select(ro => new Room { ID = ro.ID, Level = ro.Level, BedCount = ro.BedCount, Price = ro.Price });



            //.Select(mo => new Movie { ID = mo.ID, Genre = mo.Genre, Price = mo.Price, ReleaseDate = mo.ReleaseDate, Title = mo.Title });

            // Run the query and save the results in DiffMovies for passing to content file
            DiffMovies = await diffMovies.ToListAsync();
            // Save the options for both dropdown lists in ViewData for passing to content file
            ViewData["BedCountList"] = new SelectList(_context.Room, "BedCount", "BedCount");
            // invoke the content file
            return Page();
        }
    }
}