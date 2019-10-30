using System;
using System.Collections.Generic;

namespace HaveWeMet
{
    public class LocationHistoryAnalysis
    {
        /*
        * Checks where you were on a particular day
        * Returns first instance found
        */
        public static LocationHistory.Location CheckAlibi(DateTime date1, LocationHistory locationHistory)
        {
            foreach (var location in locationHistory.locations)
            {
                var date2 = LocationHistoryHelperMethods.UnixTimeStampToDateTime(location.timestampMs);
                if (LocationHistoryHelperMethods.DateTimesCoincide(date1, date2))
                {
                    Console.WriteLine("Location:\n\nLatitude: " + location.latitudeE7 + "\tLongitude: " + location.longitudeE7);
                    return location;
                }
            }
            return null;
        }

        /*
         * Checks if two location histories share a common time and place
         * Returns the DateTime for such instance
         */
        public static DateTime? HaveWeMet(LocationHistory locationHistory1, LocationHistory locationHistory2)
        {
            int i = 0;
            int j = 0;
            while (i < locationHistory1.locations.Count && j < locationHistory2.locations.Count)
            {
                var date1 = LocationHistoryHelperMethods.UnixTimeStampToDateTime(locationHistory1.locations[i].timestampMs);
                var date2 = LocationHistoryHelperMethods.UnixTimeStampToDateTime(locationHistory2.locations[j].timestampMs);
                var dateDiff = date1 - date2;

                if (dateDiff.Days == 1)
                {
                    j++;
                }
                else if (dateDiff.Days == -1)
                {
                    i++;
                }
                else if (LocationHistoryHelperMethods.DateTimesCoincide(date1, date2))
                {
                    var lat1 = locationHistory1.locations[i].latitudeE7;
                    var lon1 = locationHistory1.locations[i].longitudeE7;
                    var lat2 = locationHistory2.locations[j].latitudeE7;
                    var lon2 = locationHistory2.locations[j].longitudeE7;
                    var HaveWeMet = LocationHistoryHelperMethods.LocationsCoincide(lat1, lon1, lat2, lon2);
                    //Checks if the coordinates are relatively close
                    if (HaveWeMet)
                    {
                        return date1;
                    }
                    i++;
                    j++;
                }
                i++;
                j++;
            }
            return null;
        }

    }
}
