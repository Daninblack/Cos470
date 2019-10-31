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
                return NotFound();
            }
        }

        // GET api/HaveWeMet/id/date
        [HttpGet("{name}/{date}")]
        public ActionResult<LocationHistory.Location> Get(string name, DateTime date)
        {
            if (LocationHistories.ContainsKey(name))
            {
                //DateTime newDate = LocationHistoryHelperMethods.StringToDateTime(date);
                LocationHistory.Location locations = LocationHistoryAnalysis.CheckAlibi(date, LocationHistories[name]);
                return locations;
            }
            else
            {
                return new NotFoundResult(); 
            }
        }

        // GET api/HaveWeMet/name/MetDate/name2
        [HttpGet("{name}/{name2}/FirstMetEachOtherOn")]
        public ActionResult<DateTime> Get(string name, string name2)
        {
            if(LocationHistories.ContainsKey(name) && LocationHistories.ContainsKey(name2))
            {
                LocationHistory locationHistory = LocationHistories[name];
                LocationHistory locationHistory2 = LocationHistories[name2];
                var result = LocationHistoryAnalysis.HaveWeMet(locationHistory, locationHistory2);
                return result;
            }
            else
            {
                return new NotFoundResult();
            }
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