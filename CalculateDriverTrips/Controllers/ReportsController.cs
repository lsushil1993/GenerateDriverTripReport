using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculateDriverTrips.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalculateDriverTrips.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private IFileActionsProvider _FileActionProvider { get; set; }
        public ReportsController(IFileActionsProvider FileActionsProvider)
        {
            _FileActionProvider = FileActionsProvider;
        }

        [HttpGet("generateReport")]
        public JsonResult GenerateReport()
        {
            var records = _FileActionProvider.ReadFile();
            return new JsonResult(records);
        }
    }
}
