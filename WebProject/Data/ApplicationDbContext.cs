using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelWebApp.Models;

namespace HotelWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<HotelWebApp.Models.Booking> Booking { get; set; }
        public DbSet<HotelWebApp.Models.Customer> Customer { get; set; }
        public DbSet<HotelWebApp.Models.Room> Room { get; set; }
    }
}
