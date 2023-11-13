using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaxiGomel.Models
{
    public partial class CarModel
    {
        public CarModel()
        {
            Cars = new HashSet<Car>();
        }
        [Key]
        public int CarModelID { get; set; }
        public string ModelName { get; set; }
        public string TechStats { get; set; }
        public decimal? Price { get; set; }
        public string Specifications { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
