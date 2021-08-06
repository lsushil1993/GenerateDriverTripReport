using CalculateDriverTrips.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateDriverTrips.Utilities
{
    public class FileProcessor : IFileProcessor
    {
        public TripDetails ProcessLines(string record)
        {
            if (string.IsNullOrEmpty(record))
                return null;

            string[] details = record.Split(" ");
            if(details.Count() == 5)
            {
                TripDetails trip = new TripDetails();
                trip.Driver = details[1];
                trip.StartTime = Convert.ToDateTime(details[2]);
                trip.StopTime = Convert.ToDateTime(details[3]);
                trip.Miles = Convert.ToDecimal(details[4]);
                return trip;
            }

            return null;
        }
    }
}
