using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HotelWebApp.Models
{
    public class MakeBookingViewModel
    {
        [Range(1, 16)]
        public int RoomID { get; set; }

       // public string CustomerEmail { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CheckIn { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CheckOut { get; set; }

        // It was just cost in Booking, But we are going to use Total Cost, because we gonna multiply room cost * date ( checkout - checkin)
        // public decimal TotalCost { get; set; }

    }
}
