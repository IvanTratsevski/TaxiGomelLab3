using System;
using System.Collections.Generic;
using TaxiGomel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaxiGomel.Data
{
    public partial class TaxiGomelContext : DbContext
    {
        public TaxiGomelContext()
        {
        }

        public TaxiGomelContext(DbContextOptions<TaxiGomelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Call> Calls { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarModel> CarModels { get; set; }
        public virtual DbSet<CarsDriver> CarsDrivers { get; set; }
        public virtual DbSet<CarsMechanic> CarsMechanics { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }

    }
        
}
