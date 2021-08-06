using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateDriverTrips.Models
{
    public class ReportDetails
    {
        public ReportDetails()
        {
            Miles = 0;
            Speed = 0;
        }
        public string Driver { get; set; }
        public double Miles { get; set; }
        public double Speed { get; set; }
    }
}
