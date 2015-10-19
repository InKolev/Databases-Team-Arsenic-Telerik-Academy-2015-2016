namespace Toys.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        private ICollection<Sale> sales;

        public Product()
        {
            this.sales = new HashSet<Sale>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Index("Sku", IsUnique = true)]
        public string Sku { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public decimal WholesalePrice { get; set; }

        public decimal? RetailPrice { get; set; }

        public decimal? TradeDiscount { get; set; }

        public float? TradeDiscountRate { get; set; }

        public int ManufacturerId { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        public virtual ICollection<Sale> Sales
        {
            get { return this.sales; }
            set { this.sales = value; }
        }
    }
}
