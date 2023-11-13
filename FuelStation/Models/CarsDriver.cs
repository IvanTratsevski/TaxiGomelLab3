using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaxiGomel.Models
{
    public partial class CarsDriver
    {
        [Key]
        public int CarDriverId { get; set; }
        public int? CarId { get; set; }
        public int? DriverId { get; set; }

        public virtual Car Car { get; set; }
        public virtual Employee Driver { get; set; }
    }
}
