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

        public int SalerId { get; set; }

        public virtual Saler Saler { get; set; }
    }
}
