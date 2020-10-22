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
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HotelWebApp.Pages.Bookings
{
    [Authorize(Roles = "Administrators")]
    public class CreateModel : PageModel
    {
        private readonly HotelWebApp.Data.ApplicationDbContext _context;

        public CreateModel(HotelWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CustomerEmail"] = new SelectList(_context.Set<Customer>(), "Email", "FullName");
        ViewData["RoomID"] = new SelectList(_context.Set<Room>(), "ID", "ID");
            return Page();
        }

        [BindProperty]
        public Booking Booking { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else if (Booking.CheckIn < DateTime.Today)
            {
                ViewData["CheckInerr"] = "Check in Date Must be in the future";
                return Page();
            }
            else if (Booking.CheckIn > Booking.CheckOut)
            {
                ViewData["CheckOuterr"] = "Check Out Date Must be in the future";
                return Page();
            }

            var roomID = new SqliteParameter("roomID", Booking.RoomID);
            var cInDate = new SqliteParameter("cInDate", Booking.CheckIn);
            var cOutDate = new SqliteParameter("cOutDate", Booking.CheckOut);

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

            if (succesQuerys.Count == 1)
            {
                _context.Booking.Add(Booking);
                await _context.SaveChangesAsync();
                ViewData["SuccessDB"] = "success";
            }
            else
            {
                ViewData["Fail"] = "During your period, Booking not available, This room is this period already taken";
                return Page();
            }
            return RedirectToPage("./BookingManagement");
        }
    }
}
