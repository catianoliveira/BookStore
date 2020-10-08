﻿using BookStore.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class AddItemViewModel
    {
        [Display(Name = "Product")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
        public int ItemId { get; set; }

        public Item Item { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "The quantity must be a positive number")]
        public double Quantity { get; set; }


       public IEnumerable<SelectListItem> Items { get; set; }
    }
}
