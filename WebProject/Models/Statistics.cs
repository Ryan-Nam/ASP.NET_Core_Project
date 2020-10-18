using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HotelWebApp.Models
{
    public class Statistics
    {
        // - how many customersa are located in each postcode.
        // - how many bookings have been made for each room.

        [Display(Name = "Room ID")]
        public int Room 
        {
            get;
            set;
        }
        [Display(Name = "Number of Bookings")]
        public int BookingCount
        {
            get;
            set;
        }


        [Display(Name = "Postcode")]
        public String Postcode
        {
            get;
            set;
        }

        [Display(Name = "Number of Customers")]
        public int CustomerCount;
    }
}
