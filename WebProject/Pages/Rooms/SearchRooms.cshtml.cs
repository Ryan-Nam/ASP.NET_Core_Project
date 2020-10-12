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


            ViewData["BedCount"] = new SelectList(new[]
                       {
                            new { BedCount = "1", Name = "1 bed" },
                            new { BedCount = "2", Name = "2 beds" },
                            new { BedCount = "3", Name = "3 beds" },
                        },
                       "BedCount", "Name", 1);


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

            int selected = 1;
            if (RoomSearch != null)
            {
                selected = RoomSearch.BedCount;
            }

            ViewData["BedCount"] = new SelectList(new[]
            {
                            new { BedCount = "1", Name = "1 bed" },
                            new { BedCount = "2", Name = "2 beds" },
                            new { BedCount = "3", Name = "3 beds" },
                        },
            "BedCount", "Name", selected);



            if (!ModelState.IsValid)
            {
                return Page();
            }
            else if (RoomSearch.CheckIn < DateTime.Today)
            {
                ModelState.AddModelError("RoomSearch.CheckIn", "Check in Date Must be in the future");
                return Page();
            }
            else if (RoomSearch.CheckIn > RoomSearch.CheckOut)
            {
                ModelState.AddModelError("RoomSearch.CheckOut", "Check Out Date Must after Check In Date");
                return Page();
            }



            // prepare the parameters to be inserted into the query
            var bedCount = new SqliteParameter("bedCount", RoomSearch.BedCount);
            var checkIn = new SqliteParameter("checkIn", RoomSearch.CheckIn);
            var checkOut = new SqliteParameter("checkOut", RoomSearch.CheckOut);

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
            String query = "SELECT [Room].* FROM Room " +
                              "WHERE [Room].BedCount = @bedCount ";

            String subQuery = "(SELECT [Room].ID " +
                              "FROM [Room] " +
                              "INNER JOIN [Booking] " +
                              "ON [Room].ID = [Booking].RoomId " +
                              "WHERE @checkIn < Booking.Checkout " +
                              "AND Booking.CheckIn < @checkOut ) ";

            String notQuery = query + " AND [Room].ID NOT IN " + subQuery;


            var searchQuery = _context.Room.FromSqlRaw(notQuery, bedCount, checkIn, checkOut);


            DiffRooms = await searchQuery.ToListAsync();

            return Page();

        }
    }
}