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

            var filePath2 = @"C:\Users\Daniel\Desktop\Cos470 folder\Assignment3\HaveWeMet\WebApplication\LocationHistoryVeryShort2.json";
            var json2 = LocationHistoryHelperMethods.BuildJSONFromFile(filePath2);
            var locationHistory2 = LocationHistoryHelperMethods.DeserializeJSON(json2);

            if (LocationHistories.Count == 0)
            {
                LocationHistories.Add(0, locationHistory);
                LocationHistories.Add(1, locationHistory2);
            }

        }

        // GET api/HaveWeMet
        [HttpGet]
        public ActionResult<Dictionary<int, LocationHistory>> Get()
        {
            return LocationHistories;
        }

        // GET api/HaveWeMet/LocationHistoryID
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

        // GET api/HaveWeMet/LocationHistoryID/date
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

        // GET api/HaveWeMet/LocationHistoryID/MetDate/LocationHistoryID2
        [HttpGet("{id}/MetDate/{id2}")]
        public ActionResult<DateTime> Get(int id, int id2)
        {
            if(LocationHistories.ContainsKey(id) && LocationHistories.ContainsKey(id2))
            {
                LocationHistory locationHistory = LocationHistories[id];
                LocationHistory locationHistory2 = LocationHistories[id2];
                var result = LocationHistoryAnalysis.HaveWeMet(locationHistory, locationHistory2);
                return result;
            }
            return null;
        }

        // POST api/HaveWeMet/post
        //[HttpPost("post")]
        //public ActionResult<bool> Post([FromBody] string locHistory)
        //{
        //    if (LocationHistories.Count() > 0)
        //    {
        //        var locationHistory = LocationHistoryHelperMethods.DeserializeJSON(locHistory);
        //        LocationHistories.Add(LocationHistories.Count, locationHistory);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

    }
}