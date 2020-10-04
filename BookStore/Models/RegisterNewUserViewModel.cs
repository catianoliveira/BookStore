using BookStore.CustomValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class RegisterNewUserViewModel
    {

        [Display(Name = "Role")]
        public string RoleID { get; set; }


        public IEnumerable<SelectListItem> RoleChoices { get; set; }





        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }




        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [LessThanDate]
        public DateTime DateOfBirth { get; set; }






        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string Address { get; set; }



        [Required]
        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string PhoneNumber { get; set; }



        [Required]
        public string City { get; set; }



        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
        public int CountryId { get; set; }



        public IEnumerable<SelectListItem> Countries { get; set; }






        [Required]
        [Display(Name = "E-mail")]
        public string EmailAddress { get; set; }




        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}
