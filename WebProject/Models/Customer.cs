using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelWebApp.Models
{
    public class Customer
    {
        [Key, Required]
        [DataType(DataType.EmailAddress)]
        // because if it is not the specific type of ID XXID, program will autumatically store in Database
        // But we don't want "Email" to store in DB, Because It is Primary key.
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Email
        {
            get;
            set;
        }

        [Required]
        [MinLength(2), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z'-]{2,20}$")]
        public string Surname
        {
            get;
            set;
        }

        [Required]
        [MinLength(2), MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z'-]{2,20}$")]
        public string GivenName
        {
            get;
            set;
        }


        
        [NotMapped] // not to map this property to database, but exist in application
        public string FullName => $"{GivenName} {Surname}";
        

        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "must be 4 digits")]
        public string Postcode
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
