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
        static Dictionary<string, LocationHistory> LocationHistories = new Dictionary<string, LocationHistory>();
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
                LocationHistories.Add("Donnie", locationHistory);
                LocationHistories.Add("Frank", locationHistory2);
            }

        }

        // GET api/HaveWeMet
        [HttpGet]
        public ActionResult<Dictionary<string, LocationHistory>> Get()
        {
            return LocationHistories;
        }

        // GET api/HaveWeMet/name
        [HttpGet("{name}")]
        public ActionResult<LocationHistory> Get(string name)
        {
            if (LocationHistories.ContainsKey(name))
            {
                return LocationHistories[name];
            }
            else
            {
                return NotFound("No location history was found for " + name);
            }
        }

        // GET api/HaveWeMet/id/date
        [HttpGet("{name}/{date}")]
        public ActionResult<string> Get(string name, DateTime date)
        {
            string location = "";

            if (LocationHistories.ContainsKey(name))
            {
                LocationHistory.Location locations = LocationHistoryAnalysis.CheckAlibi(date, LocationHistories[name]);
                if(locations == null)
                {
                    location = "No location found for this given date: " + date;
                    return NotFound(location);
                }
                else
                {
                    var dateTime = LocationHistoryHelperMethods.UnixTimeStampToDateTime(locations.timestampMs);
                    location = name + " was located at:\n\nlatitude: " + locations.latitudeE7 + " longitude: " + locations.longitudeE7 +
                               "\non " + dateTime;
                    return location;
                }
            }
            else
            {
                return NotFound("No location history found for " + name); 
            }
        }

        // GET api/HaveWeMet/name/MetDate/name2
        [HttpGet("{name}/{name2}/MetEachOther")]
        public ActionResult<string> Get(string name, string name2)
        {
            string HaveWeMetInfo = "";
            if (LocationHistories.ContainsKey(name) && LocationHistories.ContainsKey(name2))
            {
                LocationHistory locationHistory = LocationHistories[name];
                LocationHistory locationHistory2 = LocationHistories[name2];
                var result = LocationHistoryAnalysis.HaveWeMet(locationHistory, locationHistory2);
                var date = LocationHistoryHelperMethods.UnixTimeStampToDateTime(result.timestampMs);
                HaveWeMetInfo = name + " and " + name2 + " first met each other on:\n\n" +
                                "Time: " + date + "\nlatitude: " + result.latitudeE7 + " longitude: " + result.longitudeE7;
            }
            else if (!LocationHistories.ContainsKey(name) && LocationHistories.ContainsKey(name2))
            {
                HaveWeMetInfo = "No location history found for " + name;
                return NotFound(HaveWeMetInfo);
            }
            else if (LocationHistories.ContainsKey(name) && !LocationHistories.ContainsKey(name2))
            { 

                HaveWeMetInfo = "No location history found for " + name2;
                return NotFound(HaveWeMetInfo);
            }
            else
            {
                HaveWeMetInfo = "No location histories found for both " + name + " and " + name2;
                return NotFound(HaveWeMetInfo);
            }

            return HaveWeMetInfo;
        }

        // POST api/HaveWeMet/post
        [HttpPost("post/{name}")]
        public ActionResult<bool> Post(string name, [FromBody] string locHistory)
        {
            if (!LocationHistories.ContainsKey(name))
            {
                LocationHistory locationHistory = LocationHistoryHelperMethods.DeserializeJSON(locHistory);
                LocationHistories.Add(name, locationHistory);
                return true;
            }
            else
            {
                return false;
            }
        }

        // DELETE api/HaveWeMet/delete/name
        [HttpDelete("delete/{name}")]
        public ActionResult<bool> Delete(string name)
        {
            if (LocationHistories.ContainsKey(name))
            {
                LocationHistories.Remove(name);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}