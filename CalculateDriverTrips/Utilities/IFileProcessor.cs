using CalculateDriverTrips.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateDriverTrips.Utilities
{
    public interface IFileProcessor
    {
        public TripDetails ProcessLines(string line);
    }
}
