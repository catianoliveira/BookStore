using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data.Entities
{
    public class Item : IEntity
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contains {1} characters length.")]
        [Required]
        public string Title { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contains {1} characters length.")]
        [Required]
        public string Author { get; set; }


        [Required]
        public string ISBN { get; set; }



        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }



        [Display(Name = "Image")]
        public string ImageUrl { get; set; }




        [Display(Name = "Is Avaiable?")]
        public bool IsAvaiable { get; set; }




        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }




        public User User { get; set; }


        public int CategoryId { get; set; }



        public IEnumerable<SelectListItem> Categories { get; set; }


        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImageUrl))
                {
                    return null;
                }

                return $"https://webistes.azurewebsites.net{this.ImageUrl.Substring(1)}";
            }
        }
    }
}
