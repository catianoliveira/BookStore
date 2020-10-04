using BookStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<Cart> CartItems { get; set; }


        public decimal CartTotal { get; set; }
    }
}
