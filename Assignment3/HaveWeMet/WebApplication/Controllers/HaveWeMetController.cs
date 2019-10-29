using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaveWeMet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaveWeMetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HaveWeMetController : ControllerBase
    {
        static Dictionary<int, LocationHistory> LocationHistories = new Dictionary<int, LocationHistory>();

        public HaveWeMetController()
        {
            var filePath = @"C:\Users\Daniel\Desktop\Cos470 folder\Assignment3\HaveWeMet\WebApplication\LocationHistoryVeryShort.json";
            var json = LocationHistoryHelperMethods.BuildJSONFromFile(filePath);
            var locationHistory = LocationHistoryHelperMethods.DeserializeJSON(json);
            if (LocationHistories.Count == 0)
            {
                LocationHistories.Add(0, locationHistory);
            }
            else
            {
                LocationHistories.Add(LocationHistories.Count, locationHistory);
            }
        }

        // GET api/HaveWeMet
        [HttpGet]
        public ActionResult<Dictionary<int, LocationHistory>> Get()
        {
            return LocationHistories;
        }

        // GET api/HaveWeMet/LocationHistoryIDNumber
        [HttpGet("{LocationHistoryID}")]
        public ActionResult<LocationHistory> Get(int LocationHistoryID)
        {
            if (LocationHistories.ContainsKey(LocationHistoryID))
            {
                return LocationHistories[LocationHistoryID];
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/HaveWeMet/LocationHistoryIDNumber/date
        [HttpGet("{LocationHistoryID}/{date}")]
        public ActionResult<LocationHistory.Location> Get(int LocationHistoryID, String date)
        {
            if (LocationHistories.ContainsKey(LocationHistoryID))
            {
                DateTime newDate = LocationHistoryHelperMethods.StringToDateTime(date);
                LocationHistory.Location locations = LocationHistoryAnalysis.CheckAlibi(newDate, LocationHistories[LocationHistoryID]);
                return locations;
            }
            else
            {
                return NotFound();
            }
        }

    }
}