using System;
using System.Collections.Generic;

namespace TaxiGomel.Models
{
    public partial class EmployeeCookie
    { 
        public int Age { get; set; }
        public int PositionID { get; set; }
        public EmployeeCookie(int Age, int PositionID)
        {
            this.Age = Age;
            this.PositionID = PositionID;
        }
    }
}
