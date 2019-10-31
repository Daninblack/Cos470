using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaveWeMet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HaveWeMetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HaveWeMetController : ControllerBase
    {
        static Dictionary<int, LocationHistory> LocationHistories = new Dictionary<int, LocationHistory>();
        IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

        public HaveWeMetController()
        {
            var filePath = config["filePath"];
            var json = LocationHistoryHelperMethods.BuildJSONFromFile(filePath);
            var locationHistory = LocationHistoryHelperMethods.DeserializeJSON(json);

            var filePath2 = config["filePath2"];
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
        [HttpGet("{id}")]
        public ActionResult<LocationHistory> Get(int id)
        {
            if (LocationHistories.ContainsKey(id))
            {
                return LocationHistories[id];
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/HaveWeMet/id/date
        [HttpGet("{id}/{date}")]
        public ActionResult<LocationHistory.Location> Get(int id, String date)
        {
            if (LocationHistories.ContainsKey(id))
            {
                DateTime newDate = LocationHistoryHelperMethods.StringToDateTime(date);
                LocationHistory.Location locations = LocationHistoryAnalysis.CheckAlibi(newDate, LocationHistories[id]);
                return locations;
            }
            else
            {
                return new NotFoundResult(); 
            }
        }

        // GET api/HaveWeMet/id/MetDate/id2
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
            else
            {
                return new NotFoundResult();
            }
        }

        // POST api/HaveWeMet/post
        [HttpPost("post")]
        public ActionResult<bool> Post([FromBody] string locHistory)
        {
            if (LocationHistories.Count() > 0)
            {
                LocationHistory locationHistory = LocationHistoryHelperMethods.DeserializeJSON(locHistory);
                LocationHistories.Add(LocationHistories.Count, locationHistory);
                return true;
            }
            else
            {
                return false;
            }
        }

        // DELETE api/HaveWeMet/delete/id
        [HttpDelete("delete/{id}")]
        public ActionResult<bool> Delete(int id)
        {
            if (LocationHistories.ContainsKey(id))
            {
                LocationHistories.Remove(id);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}