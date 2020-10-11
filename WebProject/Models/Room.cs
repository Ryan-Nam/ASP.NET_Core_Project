using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelWebApp.Models
{
    public class Room
    {
        // Primary key
        public int ID
        {
            get;
            set;
        }

        [Required]
        [RegularExpression(@"^[G1-3]{1}$", ErrorMessage = "must be English letters, digits, spaces and underscore")]
        public string Level
        {
            get;
            set;
        }

        [Range(1, 3)]
        public int BedCount
        {
            get;
            set;
        }

        [Range(50, 300.0)]
        public decimal Price
        {
            get;
            set;
        }

        // Navigation properties
        public ICollection<Booking> TheBookings
        {
            get;
            set;
        }
    }
}
