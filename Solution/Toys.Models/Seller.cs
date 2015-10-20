namespace Toys.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Seller
    {
        private ICollection<Sale> sales;

        public Seller()
        {
            this.sales = new HashSet<Sale>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        public virtual ICollection<Sale> Sales
        {
            get { return this.sales; }
            set { this.sales = value; }
        }
    }
}
