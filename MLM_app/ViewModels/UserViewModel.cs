using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLM_app.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "ID")]
        public string ID { get; set; }

        [StringLength(50)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [StringLength(50)]
        [Display(Name = "English name")]
        public string EnglishName { get; set; }

       

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "SIN Number")]
        [StringLength(10, ErrorMessage = "SIN Number must be 10 digits", MinimumLength = 10)]
        public string NationalCode { get; set; }

        [Display(Name = "Postal code")]
        [StringLength(10, ErrorMessage = "Postal code must be 10 digits", MinimumLength = 10)]
        public string ZipCode { get; set; }

        [Display(Name = "Date of birth")]
        public Nullable<DateTime> DateOfBirth { get; set; }

        [Display(Name = "Registration date")]
        public Nullable<DateTime> DateOfRegistration { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [EmailAddress(ErrorMessage = "The format of {0} entered is incorrect")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        public bool ValidCOntractInfo { get; set; }

    }
}