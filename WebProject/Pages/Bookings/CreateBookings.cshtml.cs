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
using System.Security.Claims;

namespace HotelWebApp.Pages.Bookings

{
    [Authorize(Roles = "Customers")]

    public class CreateBookingsModel : PageModel
    {
        private readonly HotelWebApp.Data.ApplicationDbContext _context;
        public CreateBookingsModel(HotelWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }


        [BindProperty]
        // need 'using <ProjectName>.Models'
        public MakeBooking makeabook { get; set; }

        // List of different movies; for passing to Content file to display
        public IList<Room> DiffMovies { get; set; }


        // GET: MovieGoers/PeopleDiff

        // Search Rooms
        // GET: Bookings/SearchRooms
        public IActionResult OnGetAsync()
        {
            // Get the options for the MovieGoer select list from the database
            // and save them in ViewData for passing to Content file
            ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");
            ViewData["CustomerEmail"] = new SelectList(_context.Customer, "Email", "Email");
            return Page(); // what is different between return Page and View??
        }

       
        public async Task<IActionResult> OnPostAsync()
        {
            // prepare the parameters to be inserted into the query
            var rID = new SqliteParameter("rID", makeabook.RoomID);
            var cInDate = new SqliteParameter("checkIn", makeabook.CheckIn);
            var cOutDate = new SqliteParameter("checkOut", makeabook.CheckOut);

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
            /* 
             var diffMovies = _context.Room.FromSqlRaw("select * from [Room] "
                + "where [Room].BedCount = @bedCount and [Room].ID not in "
                 + "(select [Room].ID from [Room] inner join [Booking] on [Room].ID = [Booking].RoomID "
                  + "where ([Booking].CheckIn <= @checkIn and [Booking].CheckOut >= @checkIn)"
                   + "or ([Booking].CheckIn >= @checkIn and[Booking].CheckOut <= @checkOut)"
                      + "or ([Booking].CheckIn <= @checkOut and [Booking].CheckOut >= @checkOut))", rID, cInDate, cOutDate);
            
             var diffMovies = _context.Room.FromSql("select * from [Room]"
                + "where [Room].ID = @rID and [Room].ID not in " +
                "(select [Room].ID from [Room] inner join [Booking] on [Room].ID = [Booking].RoomID " +
                "where ([Booking].checkIn <= @checkIn and [Booking].checkOut >= @checkIn)" +
                "or ([Booking].checkIn >= @checkIn and[Booking].checkOut <= @checkOut)" +
                "or ([Booking].checkIn <= @checkOut and [Booking].checkOut >= @checkOut))", rID, cInDate, cOutDate);
             */
            var diffMovies = _context.Room.FromSqlRaw("select * from [Room]"
               + "where [Room].ID = @rID and [Room].ID not in " +
               "(select [Room].ID from [Room] inner join [Booking] on [Room].ID = [Booking].RoomID " +
               "where ([Booking].checkIn <= @checkIn and [Booking].checkOut >= @checkIn)" +
               "or ([Booking].checkIn >= @checkIn and[Booking].checkOut <= @checkOut)" +
               "or ([Booking].checkIn <= @checkOut and [Booking].checkOut >= @checkOut))", rID, cInDate, cOutDate);










            //.Select(mo => new Movie { ID = mo.ID, Genre = mo.Genre, Price = mo.Price, ReleaseDate = mo.ReleaseDate, Title = mo.Title });

            // Run the query and save the results in DiffMovies for passing to content file
            DiffMovies = await diffMovies.ToListAsync();

            if (DiffMovies.Count != 0)
            {
                if (User.IsInRole("Customers"))
                {
                    var booking = new Booking()
                    {
                        RoomID = makeabook.RoomID,
                        CustomerEmail = User.Identity.Name,
                        CheckIn = makeabook.CheckIn,
                        CheckOut = makeabook.CheckOut
                    };
                    var theRoom = await _context.Room.FindAsync(makeabook.RoomID);
                    var lengthOfStay = (makeabook.CheckOut - makeabook.CheckIn).TotalDays;

                    ViewData["MyBooking"] = booking;

                    if (lengthOfStay != 0)
                    {
                        booking.Cost = theRoom.Price * (decimal)lengthOfStay;
                    }
                    else
                    {
                        booking.Cost = theRoom.Price;
                    }


                    _context.Booking.Add(booking);
                    //await _context.SaveChangesAsync();
                    return Page();
                    // return RedirectToAction(nameof(Index));
                }
                else
                {
                    var booking = new Booking()
                    {
                        RoomID = makeabook.RoomID,
                        CustomerEmail = makeabook.CustomerEmail,
                        CheckIn = makeabook.CheckIn,
                        CheckOut = makeabook.CheckOut
                    };
                    var theRoom = await _context.Room.FindAsync(makeabook.RoomID);
                    var lengthOfStay = (makeabook.CheckOut - makeabook.CheckIn).TotalDays;

                    if (lengthOfStay != 0)
                    {
                        booking.Cost = theRoom.Price * (decimal)lengthOfStay;
                    }
                    else
                    {
                        booking.Cost = theRoom.Price;
                    }

                    _context.Booking.Add(booking);
                    //await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(BookingManagement));
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID", makeabook.RoomID);
            ViewData["CustomerEmail"] = new SelectList(_context.Customer, "Email", "Email", makeabook.CustomerEmail);

            // invoke the content file
            return Page();
        }
          
    
    }
      
  
}