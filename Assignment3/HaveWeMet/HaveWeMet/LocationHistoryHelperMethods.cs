using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;

namespace HaveWeMet
{
    public class LocationHistoryHelperMethods
    {
        /*
         * Constructs the JSON given a text file
         */
        public static string BuildJSONFromFile(string filePath)
        {
            List<string> lines = File.ReadAllLines(filePath).ToList();
            StringBuilder json = new StringBuilder();
            foreach (string line in lines)
            {
                json.AppendLine(line);
            }

            return json.ToString();
        }

        /*
         * Deserializes a json file
         * Returns a LocationHistory
         */
        public static LocationHistory DeserializeJSON(string json)
        {
            try
            {
                var locationHistory = JsonConvert.DeserializeObject<LocationHistory>(json);
                return locationHistory;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /*
         * Takes a timestampMs string
         * Returns a DateTime value that's comprehensible for humans
         */
        public static DateTime UnixTimeStampToDateTime(string timestampMs)
        {
            var date = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds(double.Parse(timestampMs));
            var newDate = date.AddMilliseconds(-date.Millisecond);
            return newDate;
        }

        /*
         * Compares two DateTime instances
         * Returns whether or not the times are close enough
         * Times are close enough if they are 20 minutes apart or less
         */
        public static bool DateTimesCoincide(DateTime date1, DateTime date2)
        {

            double timeSpanLimit = 30.0;
            TimeSpan timeSpan = date1 - date2;
            double duration = Math.Abs(timeSpan.TotalMinutes);

            if (duration <= timeSpanLimit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
         * Given a string with the following format: "YYYY-MM-DD-HH-mm-sss"
         * Returns DateTime
         */
         public static DateTime StringToDateTime(string date)
        {
            DateTime newDate = DateTime.ParseExact(date, "yyyy-MM-dd-HH-mm-ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
            return newDate;
        }

        /*
         * Converts latitudeE7 and longitudeE7 into actual coordinates
         */
        public static double FormatCoordinate(double coordinate)
        {
            return (coordinate / 1e7);
        }

        /*
         * Compares two coordinates and determines whether they are close enough
         * Coordinates are close enough if they are 10 meters apart or less
         */
        public static bool LocationsCoincide(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double distanceLimit = 10.0;
            double long1 = FormatCoordinate(longitude1);
            double lat1 = FormatCoordinate(latitude1);
            double long2 = FormatCoordinate(longitude2);
            double lat2 = FormatCoordinate(latitude2);

            var coordinate1 = new GeoCoordinate(lat1, long1);
            var coordinate2 = new GeoCoordinate(lat2, long2);
            double distance = coordinate1.GetDistanceTo(coordinate2);

            return distance <= distanceLimit;
        }

    }
}
