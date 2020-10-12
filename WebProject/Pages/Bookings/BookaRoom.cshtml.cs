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
            ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");
            return Page();
        }

        [BindProperty]
        public MakeBooking RoomBooking { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else if (RoomBooking.CheckIn < DateTime.Today)
            {
                //ViewData["SuccessDB"] = "Check in Date Must be in the future";
                ViewData["CheckInerr"] = "Check in Date Must be in the future";
                ModelState.AddModelError("Booking.CheckIn", "Check in Date Must be in the future");
                return Page();
            }
            else if (RoomBooking.CheckIn > RoomBooking.CheckOut)
            {
                //ViewData["SuccessDB"] = "Check Out Date Must after Check In Date";
                ViewData["CheckOuterr"] = "Check Out Error Date Must be in the future";
                ModelState.AddModelError("Booking.CheckOut", "Check Out Date Must after Check In Date");
                return Page();
            } 
            


            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Booking Booking = new Booking
            {
                TheCustomer = await _context.Customer.FirstOrDefaultAsync(m => m.Email == _email),
                TheRoom = await _context.Room.FirstOrDefaultAsync(m => m.ID == RoomBooking.RoomID),
                RoomID = RoomBooking.RoomID,
                CustomerEmail = _email,
                CheckIn = RoomBooking.CheckIn,
                CheckOut = RoomBooking.CheckOut
            };

            // Calculate the total price of room booking
            int lengthOfStay = (int)(Booking.CheckOut - Booking.CheckIn).TotalDays;
            Booking.Cost = lengthOfStay * Booking.TheRoom.Price;

            //Level .... am i right????? or should i use this into Booking??{} 
            var theLevel = await _context.Room.FirstOrDefaultAsync(m => m.ID == RoomBooking.RoomID);
            RoomBooking.Level = theLevel.Level;


            //raw sql
            var roomID = new SqliteParameter("roomID", RoomBooking.RoomID);
            var checkIn = new SqliteParameter("checkIn", RoomBooking.CheckIn);
            var checkOut = new SqliteParameter("checkOut", RoomBooking.CheckOut);



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
            //If query result is one (we only want 1 result), store into DB
            if (thing.Count == 1)
            {
                _context.Booking.Add(Booking);
                await _context.SaveChangesAsync();
                ViewData["SuccessDB"] = "success";
                /*
                ViewData["SuccessDB"] = $"Booked, {Booking.RoomID} on level {Booking.TheRoom.Level}" +
                            $"from {Booking.CheckIn:d} to {Booking.CheckOut:d} for {Booking.Cost:C2} ";*/
            }
            else
            //If query result is more than one, (which means my query could be wrong or ..something worng), error mes display
            {
                ViewData["Fail"] = "Booking not available, during your period, maybe someone already book, but I actually do not know from when to when....";
                //ViewData["SuccessDB"] = "Booking not available";
            }



            return Page();
        }
    }
}
