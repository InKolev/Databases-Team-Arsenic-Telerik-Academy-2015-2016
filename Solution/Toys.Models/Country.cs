namespace Toys.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        private ICollection<Manufacturer> manufacturers;

        public Country()
        {
            this.manufacturers = new HashSet<Manufacturer>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string CapitalCity { get; set; }

        [StringLength(10)]
        public string PhonePrefix { get; set; }

        [StringLength(10)]
        public string IsoCode { get; set; }

        [StringLength(50)]
        public string Continent { get; set; }

        public virtual ICollection<Manufacturer> Manufacturers
        {
            get { return this.manufacturers; }
            set { this.manufacturers = value; }
        }
    }
}