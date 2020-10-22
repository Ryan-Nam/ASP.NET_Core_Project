using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelWebApp.Models
{
    public class Booking
    {
        // primary key
        public int ID
        {
            get;
            set;
        }


        // foreign key = FK naming is Convention for example, ClassName + ID 
        public int RoomID
        {
            get;
            set;
        }

        // foreign key
        public string CustomerEmail
        {
            get;
            set;
        }

        //Only data part is needed
        //I changed from ture to false because in Edit, I could not retrieve the Check in and out data properly.
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime CheckIn
        {
            get;
            set;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime CheckOut
        {
            get;
            set;
        }
     
        
        public decimal Cost
        {
            get;
            set;
        }

        // Navigation properties
        public Room TheRoom
        {
            get;
            set;
        }
        public Customer TheCustomer
        {
            get;
            set;
        }
    }
}
