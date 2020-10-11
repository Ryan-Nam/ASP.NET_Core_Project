using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelWebApp.Data;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace HotelWebApp.Pages.Bookings
{
    [Authorize(Roles = "Customers")]

    public class AnotherCreateModel : PageModel
    {
        private readonly HotelWebApp.Data.ApplicationDbContext _context;

        public AnotherCreateModel(HotelWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CustomerEmail"] = new SelectList(_context.Customer, "Email", "Email");
            ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");

            return Page();
        }


        [BindProperty]
        public MakeBooking OrderInput { get; set; }

        // List of different movies; for passing to Content file to display
        public IList<Room> asdasd { get; set; }



        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            // prepare the parameters to be inserted into the query
            var rID = new SqliteParameter("rID", OrderInput.RoomID);
            var cInDate = new SqliteParameter("checkIn", OrderInput.CheckIn);
            var cOutDate = new SqliteParameter("checkOut", OrderInput.CheckOut);

            var diffMovies = _context.Room.FromSqlRaw("select * from [Room]"
              + "where [Room].ID = @rID and [Room].ID not in " +
              "(select [Room].ID from [Room] inner join [Booking] on [Room].ID = [Booking].RoomID " +
              "where ([Booking].CheckIn <= @checkIn and [Booking].CheckOut >= @checkIn)" +
              "or ([Booking].CheckIn >= @checkIn and[Booking].CheckOut <= @checkOut)" +
              "or ([Booking].CheckIn <= @checkOut and [Booking].CheckOut >= @checkOut))", rID, cInDate, cOutDate);

            //ViewData["Result"] = await diffMovies.ToListAsync();
            asdasd = await diffMovies.ToListAsync();
           







                // creating an order object for inserting into database
                Booking order = new Booking();
            // Construct this Order object based on OrderInput
            order.RoomID = OrderInput.RoomID;
            order.CustomerEmail = User.Identity.Name;
            order.CheckIn = OrderInput.CheckIn;
            order.CheckOut = OrderInput.CheckOut;
           
            //orderinput. is the part of showing...
            // order. is real data is stored into DB

            var theRoom = await _context.Room.FindAsync(OrderInput.RoomID);
            var lengthOfStay = (OrderInput.CheckOut - OrderInput.CheckIn).TotalDays;
            OrderInput.TotalCost = theRoom.Price * (decimal)lengthOfStay;
            order.Cost = OrderInput.TotalCost;

            var theLevel = await _context.Room.FindAsync(OrderInput.RoomID);
            OrderInput.Level = theLevel.Level;

            _context.Booking.Add(order);
            await _context.SaveChangesAsync();

          
            ViewData["TotalPrice"] = order.Cost;
         


            //return RedirectToPage("./Index");
            return Page();
        }
    }
}
