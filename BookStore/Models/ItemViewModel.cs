using BookStore.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class ItemViewModel : Item
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
