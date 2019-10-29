using System;

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


        public static bool HaveWeMet(LocationHistory locationHistory1, LocationHistory locationHistory2)
        {
            foreach(var location1 in  locationHistory1.locations)
            {
                DateTime date1 = LocationHistoryHelperMethods.UnixTimeStampToDateTime(location1.timestampMs);
                foreach(var location2 in locationHistory2.locations)
                {
                    DateTime date2 = LocationHistoryHelperMethods.UnixTimeStampToDateTime(location2.timestampMs);
                    //Checks if the time is relatively close
                    if (LocationHistoryHelperMethods.DateTimesCoincide(date1, date2))
                    {
                        var lat1 = location1.latitudeE7;
                        var lon1 = location1.longitudeE7;
                        var lat2 = location2.latitudeE7;
                        var lon2 = location2.longitudeE7;
                        var HaveWeMet = LocationHistoryHelperMethods.LocationsCoincide(lat1, lon1, lat2, lon2);
                        //Checks if the coordinates are relatively close
                        if (HaveWeMet)
                        {
                            return true;
                        }
                    }
                }
            }
            Console.WriteLine("We have not met before.");
            return false;
        }

    }
}
