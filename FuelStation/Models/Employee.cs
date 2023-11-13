using System;
using System.Collections.Generic;

namespace TaxiGomel.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Calls = new HashSet<Call>();
            CarsDrivers = new HashSet<CarsDriver>();
            CarsMechanics = new HashSet<CarsMechanic>();
        }

        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public int? PositionId { get; set; }
        public int? Experience { get; set; }

        public virtual Position Position { get; set; }
        public virtual ICollection<Call> Calls { get; set; }
        public virtual ICollection<CarsDriver> CarsDrivers { get; set; }
        public virtual ICollection<CarsMechanic> CarsMechanics { get; set; }
    }
}
