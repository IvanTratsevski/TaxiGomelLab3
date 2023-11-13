using System;
using System.Collections.Generic;

namespace TaxiGomel.Models
{
    public partial class Call
    {
        public int CallId { get; set; }
        public DateTime? CallTime { get; set; }
        public string Telephone { get; set; }
        public string StartPosition { get; set; }
        public string EndPosition { get; set; }
        public int? RateId { get; set; }
        public int? CarId { get; set; }
        public int? DispatcherId { get; set; }

        public virtual Car Car { get; set; }
        public virtual Employee Dispatcher { get; set; }
        public virtual Rate Rate { get; set; }
    }
}
