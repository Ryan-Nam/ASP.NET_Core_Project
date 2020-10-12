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
using System.Security.Claims;

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
        public MakeBooking OrderInput { get; set; }

        // List of different movies; for passing to Content file to display

        //public IList<Room> asdasd { get; set; }



        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");

            if (!ModelState.IsValid)
            {
                return Page();
            }
            else if (OrderInput.CheckIn < DateTime.Today)
            {
                //ViewData["CheckInErr"] = "checkInError";
                ModelState.AddModelError("Booking.CheckIn", "Check in Date Must be in the future");
                return Page();
            }
            else if (OrderInput.CheckIn > OrderInput.CheckOut)
            {
                //ViewData["CheckOutErr"] = "Check Out Date Must after Check In Date";
                ModelState.AddModelError("Booking.CheckOut", "Check Out Date Must after Check In Date");
                return Page();
            }

            string _email = User.FindFirst(ClaimTypes.Name).Value;




            Booking Booking = new Booking
            {
                //Use FirstOdDefault = only one data wou
                TheCustomer = await _context.Customer.FirstOrDefaultAsync(m => m.Email == _email),
                TheRoom = await _context.Room.FirstOrDefaultAsync(m => m.ID == OrderInput.RoomID),
                RoomID = OrderInput.RoomID,
                CustomerEmail = _email,
                CheckIn = OrderInput.CheckIn,
                CheckOut = OrderInput.CheckOut
            };
            int days = (int)(Booking.CheckOut - Booking.CheckIn).TotalDays;
            Booking.Cost = days * Booking.TheRoom.Price;


            //raw sql
            var roomID = new SqliteParameter("roomID", OrderInput.RoomID);
            var checkIn = new SqliteParameter("checkIn", OrderInput.CheckIn);
            var checkOut = new SqliteParameter("checkOut", OrderInput.CheckOut);



            String query = "SELECT [Room].* FROM Room " +
                            "WHERE [Room].ID = @roomID ";

            String subQuery = "(SELECT [Room].ID " +
                              "FROM [Room] " +
                              "INNER JOIN [Booking] " +
                              "ON [Room].ID = [Booking].RoomId " +
                              "WHERE @checkIn < Booking.Checkout " +
                              "AND Booking.CheckIn < @checkOut ) ";

            String notQuery = query + " AND [Room].ID NOT IN " + subQuery;


            var searchQuery = _context.Room.FromSqlRaw(notQuery, roomID, checkIn, checkOut);

            var thing = await searchQuery.ToListAsync();

            //TODO FIX BULLSHIT OUTPUT
            if (thing.Count == 1)
            {

                _context.Booking.Add(Booking);
                await _context.SaveChangesAsync();
                ViewData["SuccessDB"] = $"Booked, {Booking.RoomID} on level {Booking.TheRoom.Level}" +
                            $"from {Booking.CheckIn:d} to {Booking.CheckOut:d} for {Booking.Cost:C2} ";
            }
            else
            {
                ViewData["SuccessDB"] = "Booking not available";
            }



            return Page();
        }
    }
}
