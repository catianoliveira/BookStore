using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.Entities
{
    public class Cart : IEntity
    {
       
        [Key]
        public int Id { get; set; }

        public int ItemId { get; set; }

        public Item Item { get; set; }


        public int Count { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
