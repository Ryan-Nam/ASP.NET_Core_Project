using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelWebApp.Pages.Bookings
{
    [Authorize(Roles = "Administrators")]
    // Fristly, Create Statistic.cs view class model
    // Secondly, Add the property _context && the class constructor below
    // Add using --.Models; && IList to pass data to Content file
    public class HotelStatisticsModel : PageModel
    {
        private readonly HotelWebApp.Data.ApplicationDbContext _context;

        public HotelStatisticsModel(HotelWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // For passing the results to the Content file
        // IList<ModelClass> name
        // For passing the results to the Content file
        public IList<Statistics> PostcodesStats;

        public IList<Statistics> RoomStats;

        // GET: Booking/HotelStatistics
        // OnGET -> OnGetAsync
        public async Task<PageResult> OnGetAsync()
        {
            // How many customers are located in each postcode
            var CustomersByPostcode = _context.Customer.GroupBy(c => c.Postcode);
            PostcodesStats = await CustomersByPostcode.Select(c => new Statistics {
                Postcode = c.Key, CustomerCount = c.Count()}).ToListAsync();

            // How many bookings have been made for each room
            var bookingByRoomId = _context.Booking.GroupBy(m => m.RoomID);
            // for each group, get its count value and the number of room in this group
            RoomStats = await bookingByRoomId.Select(r => new Statistics {
                Room = r.Key, BookingCount = r.Count()}).ToListAsync();

            return Page();

            // so we will know which area, customer normally live and come here
            // and which room is more popular.
        }
    }
}
