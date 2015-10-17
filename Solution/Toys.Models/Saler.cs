namespace Toys.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Saler
    {
        private ICollection<Sale> saleses;

        public Saler()
        {
            this.saleses = new HashSet<Sale>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public virtual ICollection<Sale> Saleses
        {
            get { return this.saleses; }
            set { this.saleses = value; }
        }
    }
}
