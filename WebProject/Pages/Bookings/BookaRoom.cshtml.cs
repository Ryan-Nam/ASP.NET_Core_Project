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
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.Sqlite;

namespace HotelWebApp.Pages.Bookings
{
    [Authorize(Roles = "Customers")]
    public class BookaRoomModel : PageModel
    {
        private readonly HotelWebApp.Data.ApplicationDbContext _context;

        public BookaRoomModel(HotelWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            //ViewData["CustomerEmail"] = new SelectList(_context.Customer, "Email", "Email");
            ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");
            return Page();
        }

        [BindProperty]
        public MakeBooking BookingRoom { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");
            
            // Validation, when user tried to book invalid data, will return error message instead of success message.
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else if (BookingRoom.CheckIn < DateTime.Today)
            {
                ViewData["CheckInerr"] = "Check in Date Must be in the future";
                return Page();
            }
            else if (BookingRoom.CheckIn > BookingRoom.CheckOut)
            {
                ViewData["CheckOuterr"] = "Check Out Date Must be in the future";
                return Page();
            } 

            // Current logged in User, Customer, will only book.
            // So, can meet the requirmenet of "1 customer books 1 room"
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Booking Booking = new Booking
            {
                TheCustomer = await _context.Customer.FirstOrDefaultAsync(m => m.Email == _email),
                TheRoom = await _context.Room.FirstOrDefaultAsync(m => m.ID == BookingRoom.RoomID),
                RoomID = BookingRoom.RoomID,
                CustomerEmail = _email,
                CheckIn = BookingRoom.CheckIn,
                CheckOut = BookingRoom.CheckOut
            };

            // Caculate Data of night.
            var lengthOfStay = (int)(Booking.CheckOut - Booking.CheckIn).TotalDays;
            BookingRoom.TotalCost = lengthOfStay * Booking.TheRoom.Price;
            Booking.Cost = BookingRoom.TotalCost;

            //Level requirement,
            var theLevel = await _context.Room.FirstOrDefaultAsync(m => m.ID == BookingRoom.RoomID);
            BookingRoom.Level = theLevel.Level;

            var roomID = new SqliteParameter("roomID", BookingRoom.RoomID);
            var cInDate = new SqliteParameter("cInDate", BookingRoom.CheckIn);
            var cOutDate = new SqliteParameter("cOutDate", BookingRoom.CheckOut);
            
            var Query = _context.Room.FromSqlRaw("SELECT [Room].* FROM Room " +
                            "WHERE [Room].ID = @roomID " +
                            " AND [Room].ID NOT IN " +
                            "(SELECT [Room].ID " +
                              "FROM [Room] " +
                              "INNER JOIN [Booking] " +
                              "ON [Room].ID = [Booking].RoomId " +
                              "WHERE @cInDate < Booking.Checkout " +
                              "AND Booking.CheckIn < @cOutDate ) ", roomID, cInDate, cOutDate);


            var succesQuerys = await Query.ToListAsync();

            // The result is only 1, store into DB,
            // If the result came out more than one, which means bookings are duplicated, means, someone already book,
            // so, use else state to display error message.
            if (succesQuerys.Count == 1)
            {
                _context.Booking.Add(Booking);
                await _context.SaveChangesAsync();
                ViewData["SuccessDB"] = "success";
            }
            else
            //If query result is more than one, (which means my query could be wrong or ..something worng), error mes display
            {
                ViewData["Fail"] = "During your period, Booking not available, This room is this period already taken";
            }
            return Page();
        }
    }
}
