using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelWebApp.Models;

namespace HotelWebApp.Pages.Customers
{
    [Authorize(Roles = "Customers")]
    public class MyDetailsModel : PageModel
    {
        private readonly HotelWebApp.Data.ApplicationDbContext _context;

        public MyDetailsModel(HotelWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CustomersViewModel Myself { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // retrieve the logged-in user's email
            // need to add "using System.Security.Claims;"
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Customer movieGoer = await _context.Customer.FirstOrDefaultAsync(m => m.Email == _email);

            if (movieGoer != null)
            {
                // The user has been created in the database
                ViewData["ExistInDB"] = "true";
                Myself = new CustomersViewModel
                {
                    // Retrieve his/her details for display in the web form
                    Surname = movieGoer.Surname,
                    GivenName = movieGoer.GivenName,
                    Postcode = movieGoer.Postcode
                };
            }
            else
            {
                ViewData["ExistInDB"] = "false";
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // retrieve the logged-in user's email
            // need to add "using System.Security.Claims;"
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Customer movieGoer = await _context.Customer.FirstOrDefaultAsync(m => m.Email == _email);

            if (movieGoer != null)
            {
                // This ViewData entry is needed in the content file
                // The user has been created in the database
                ViewData["ExistInDB"] = "true";
            }
            else
            {
                ViewData["ExistInDB"] = "false";
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (movieGoer == null)
            {
                // creating a moviegoer object for inserting database
                movieGoer = new Customer();
            }

            // Construct this moviegoer object based on 'Myself'
            movieGoer.Email = _email;
            movieGoer.Surname = Myself.Surname;
            movieGoer.GivenName = Myself.GivenName;
            movieGoer.Postcode = Myself.Postcode;

            if ((string)ViewData["ExistInDB"] == "true")
            {
                _context.Attach(movieGoer).State = EntityState.Modified;
            }
            else
            {
                _context.Customer.Add(movieGoer);
            }

            try  // catching the conflict of editing this record concurrently
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            ViewData["SuccessDB"] = "success";
            return Page();
        }
        /*
        public void OnGet()
        {

        }
        */
    }
}