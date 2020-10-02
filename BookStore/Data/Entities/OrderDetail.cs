using System.ComponentModel.DataAnnotations;

namespace BookStore.Data.Entities
{
    public class OrderDetail : IEntity
    {

        public int Id { get; set; }

        [Required]
        public Item Item { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value { get { return this.Price * (decimal)this.Quantity; } }

    }
}
