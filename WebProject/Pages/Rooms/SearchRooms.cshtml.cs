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

namespace HotelWebApp.Pages.Rooms
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
        public RoomSearchModel RoomSearch { get; set; }

        // List of different movies; for passing to Content file to display
        public IList<Room> DiffRooms { get; set; }


        // GET: MovieGoers/PeopleDiff

        // Search Rooms
        // GET: Bookings/SearchRooms
        public IActionResult OnGet()
        {
            // Get the options for the MovieGoer select list from the database
            // and save them in ViewData for passing to Content file
            //ViewData["MovieGoerList"] = new SelectList(_context.Customer, "Email", "FullName");
            //ViewData["BedCountList"] = new SelectList(_context.Room, "BedCount", "BedCount");

            
            ViewData["BedCountList"] = new SelectList(new[] 
            { 
                new { BedCount = "1", Name = "1 bed" }, 
                new { BedCount = "2", Name = "2 beds" }, 
                new { BedCount = "3", Name = "3 beds" },
                        
            }, "BedCount", "Name", 1);
            
            
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
            ViewData["BedCountList"] = new SelectList(new[]
            {
                            new { BedCount = "1", Name = "1 bed" },
                            new { BedCount = "2", Name = "2 beds" },
                            new { BedCount = "3", Name = "3 beds" },
                        },
            "BedCount", "Name");
            
            // Validation of user input.
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Firstly, Flag today's data, because user can't select check in Data the data before today.
            else if (RoomSearch.CheckIn < DateTime.Today)
            {
                ModelState.AddModelError("RoomSearch.CheckIn", "Check in Date Must be in the future");
                return Page();
            }
            // Secondly, User can't select check out date, previous than check in date.
            else if (RoomSearch.CheckIn > RoomSearch.CheckOut)
            {
                ModelState.AddModelError("RoomSearch.CheckOut", "Check Out Date Must after Check In Date");
                return Page();
            }

            // parameters will be be inserted into the query
            var totalBeds = new SqliteParameter("totalBeds", RoomSearch.BedCount);
            var cInDate = new SqliteParameter("cInDate", RoomSearch.CheckIn);
            var cOutDate = new SqliteParameter("cOutDate", RoomSearch.CheckOut);

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
           
            
            var searchQuery = _context.Room.FromSqlRaw("SELECT [Room].* FROM Room " +
                              "WHERE [Room].BedCount = @totalBeds " +
                              " AND [Room].ID NOT IN " +
                              "(SELECT [Room].ID " +
                              "FROM [Room] " +
                              "INNER JOIN [Booking] " +
                              "ON [Room].ID = [Booking].RoomId " +
                              "WHERE @cInDate < Booking.Checkout " +
                              "AND Booking.CheckIn < @cOutDate ) ", totalBeds, cInDate, cOutDate);
            


            // Run the query and save the results in DiffRooms for passing to content file
            DiffRooms = await searchQuery.ToListAsync();
            // invoke the content file
            return Page();

        }
    }
}