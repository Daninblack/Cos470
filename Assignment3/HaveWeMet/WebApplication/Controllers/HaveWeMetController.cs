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
        string json;

        public HaveWeMetController()
        {
            var filePath = @"C:\Users\Daniel\Desktop\Cos470 folder\Assignment3\HaveWeMet\WebApplication\LocationHistoryVeryShort.json";
            json = LocationHistoryHelperMethods.BuildJSONFromFile(filePath);
            var locationHistory = LocationHistoryHelperMethods.DeserializeJSON(json);
            LocationHistories.Add(1, locationHistory);
        }

        // GET api/HaveWeMet
        [HttpGet]
        public ActionResult<string> Get()
        {
            return json;
        }
    }
}