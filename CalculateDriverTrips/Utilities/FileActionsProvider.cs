using CalculateDriverTrips.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateDriverTrips.Utilities
{
    public class FileActionsProvider : IFileActionsProvider
    {
        const int MAX = 10000;
        private IFileProcessor _FileProcessor { get; set; }

        public FileActionsProvider(IFileProcessor FileProcessor)
        {
            _FileProcessor = FileProcessor;
        }

        public List<ReportDetails> ReadFile()
        {
            var AllLines = new string[MAX]; //only allocate memory here
            var RecordTrips = new List<TripDetails>();
            var ReportTrips = new List<ReportDetails>();
            var RecordDrivers = new List<DriverDetails>();
            AllLines = File.ReadAllLines(Startup.StaticConfig["FilePath"]);
            var DriverRecords = AllLines.Where(x => !string.IsNullOrEmpty(x) && x.ToLower().Contains("driver")).ToList();
            var TripRecords = AllLines.Where(x => !string.IsNullOrEmpty(x) && x.ToLower().Contains("trip")).ToList();
            var Drivers = DriverRecords.Select(x => x.Split()[1]).ToList();
            var sync = new object();
            Parallel.ForEach(TripRecords, trip =>
                {
                    lock (sync)
                    {
                        var record = _FileProcessor.ProcessLines(trip);
                        if (record != null)
                            RecordTrips.Add(record);
                    }
                }
            );

            foreach (string dd in Drivers)
            {
                ReportDetails reportDetail = new ReportDetails();
                if (RecordTrips.Where(trip => trip.Driver == dd).Count() > 0)
                {
                    var c = RecordTrips.Where(
                        trip => trip.Driver == dd
                        && Math.Round((double)trip.Miles / (trip.StopTime - trip.StartTime).TotalHours) > 5 && Math.Round((double)trip.Miles / (trip.StopTime - trip.StartTime).TotalHours) < 100)
                        .Select(t => new TripDetails()
                        {
                            Driver = t.Driver,
                            StartTime = t.StartTime,
                            StopTime = t.StopTime,
                            Miles = t.Miles
                        }).ToList();
                    var miles = Math.Round(c
                        .Where(x => x.Driver == dd)
                        .GroupBy(x => x.Driver)
                        .Select(x => x.Sum(y => (double)y.Miles)).FirstOrDefault());

                    var time = c
                        .Where(x => x.Driver == dd)
                        .GroupBy(x => x.Driver)
                        .Select(x => x.Sum(y => (y.StopTime - y.StartTime).TotalHours)).FirstOrDefault();

                    reportDetail.Driver = dd;
                    reportDetail.Miles = miles;
                    reportDetail.Speed = Math.Round(miles / time);
                    ReportTrips.Add(reportDetail);
                    continue;
                }
                reportDetail.Driver = dd;
                ReportTrips.Add(reportDetail);
            }

            return ReportTrips.OrderByDescending(x => x.Miles).ToList();

        }
    }
}
