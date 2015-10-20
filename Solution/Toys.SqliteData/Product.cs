namespace Toys.SqliteData
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {

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
    }
}
