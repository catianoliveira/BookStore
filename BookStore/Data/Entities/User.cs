using BookStore.CustomValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.Entities
{
    public class User : IdentityUser
    {

        public string RoleId { get; set; }



        public string FirstName { get; set; }



        public string LastName { get; set; }



       



        [LessThanDate(ErrorMessage = "Date of birth must be less than today's day")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }




        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return $"{this.FirstName} {this.LastName}";
            }

        }




        [MaxLength(70, ErrorMessage = "The field {0} can only contain {1} characters")]
        public string Address { get; set; }




        [MaxLength(50, ErrorMessage = "The field {0} can only contain {1} characters")]
        public string City { get; set; }





        [Display(Name = "Country")]
        public int CountryId { get; set; }




        public IEnumerable<SelectListItem> Countries { get; set; }



    }
}