using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateDriverTrips.Models
{
    public class TripDetails
    {
        public string Driver { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        public decimal Miles { get; set; }
    }
}
