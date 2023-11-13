using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaxiGomel.Models
{
    public partial class CarsMechanic
    {
        [Key]
        public int CarMechanicId { get; set; }
        public int? CarId { get; set; }
        public int? MechanicId { get; set; }

        public virtual Car Car { get; set; }
        public virtual Employee Mechanic { get; set; }
    }
}
