namespace Toys.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Sale
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Sku { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int SellerId { get; set; }

        public virtual Seller Seller { get; set; }
    }
}
