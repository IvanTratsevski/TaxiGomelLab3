using System;
using System.Collections.Generic;

namespace TaxiGomel.Models
{
    public partial class Car
    {
        public Car()
        {
            Calls = new HashSet<Call>();
            CarsDrivers = new HashSet<CarsDriver>();
            CarsMechanics = new HashSet<CarsMechanic>();
        }

        public int CarId { get; set; }
        public string RegistrationNumber { get; set; }
        public int? CarModelID { get; set; }
        public string CarcaseNumber { get; set; }
        public string EngineNumber { get; set; }
        public DateTime? ReleaseYear { get; set; }
        public int? Mileage { get; set; }
        public DateTime? LastTi { get; set; }
        public string SpecialMarks { get; set; }

        public virtual CarModel Model { get; set; }
        public virtual ICollection<Call> Calls { get; set; }
        public virtual ICollection<CarsDriver> CarsDrivers { get; set; }
        public virtual ICollection<CarsMechanic> CarsMechanics { get; set; }
    }
}
