using System;
using System.Collections.Generic;

namespace HaveWeMet
{
    public class LocationHistory
    {
        public List<Location> locations { get; set; }

        public class Location
        {
            public string timestampMs { get; set; }
            public int latitudeE7 { get; set; }
            public int longitudeE7 { get; set; }
            public int accuracy { get; set; }
            public List<Activity> activity { get; set; }
        }

        public class Activity
        {
            public string timestampMs { get; set; }
            public List<ActivityType> activity { get; set; }
        }

        public class ActivityType
        {
            public string type { get; set; }
            public int confidence { get; set; }
        }
    }
}
