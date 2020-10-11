using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelWebApp.Models
{
    public class CustomersViewModel
    {
        [Required]
        public string Surname
        {
            get;
            set;
        }

        [Required]
        public string GivenName
        {
            get;
            set;
        }

        [NotMapped] // not mapping this property to database, but exist in memory
        public string FullName => $"{GivenName} {Surname}";

        [Required]
        public string Postcode
        {
            get;
            set;
        }
    }
}
