using System;
using System.Collections.Generic;

namespace TaxiGomel.Models
{
    public partial class CarSession
    {
        public int MinPrice { get; set; }
        public int CarModelID { get; set; }
        public CarSession(int MinPrice, int CarModelID)
        {
            this.MinPrice = MinPrice;
            this.CarModelID = CarModelID;
        }
    }
}
