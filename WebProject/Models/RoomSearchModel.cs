﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelWebApp.Models
{
    public class RoomSearchModel
    {
        public int RoomID { get; set; }


        [Display(Name = "Number of beds")]
        [Required]
        [Range(1, 3, ErrorMessage = "Rooms can only consist of '1', '2' or '3' beds ")]
        //[RegularExpression(@"^[123]{1}$", ErrorMessage = " Rooms can only consist of '1', '2' or '3' beds ")]
        public int BedCount { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CheckIn { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CheckOut { get; set; }
    }
}
