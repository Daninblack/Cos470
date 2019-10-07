using System;

namespace HaveWeMet
{
    public class LocationHistoryAnalysis
    {
        /*
        * Checks where you were on a particular day
        * Returns first instance found
        */
        public static bool CheckAlibi(DateTime date, LocationHistory locationHistory)
        {
            foreach (var location in locationHistory.locations)
            {
                var day = LocationHistoryHelperMethods.UnixTimeStampToDateTime(location.timestampMs);
                if (DateTime.Compare(date, day) == 0)
                {
                    Console.WriteLine("Location:\n\nLatitude: " + location.latitudeE7 + "\tLongitude: " + location.longitudeE7);
                    return true;
                }
            }
            return false;
        }

        public static bool HaveWeMet(LocationHistory locationHistory1, LocationHistory locationHistory2)
        {
            for (int i = 0; i < locationHistory1.locations.Count; i++)
            {
                DateTime date1 = LocationHistoryHelperMethods.UnixTimeStampToDateTime(locationHistory1.locations[i].timestampMs);
                for (int j = 0; j < locationHistory2.locations.Count; j++)
                {
                    DateTime date2 = LocationHistoryHelperMethods.UnixTimeStampToDateTime(locationHistory2.locations[j].timestampMs);
                    //Checks if the time is relatively close
                    if (LocationHistoryHelperMethods.DateTimesCoincide(date1, date2) == true)
                    {
                        var lat1 = locationHistory1.locations[i].latitudeE7;
                        var lon1 = locationHistory1.locations[i].longitudeE7;
                        var lat2 = locationHistory2.locations[j].latitudeE7;
                        var lon2 = locationHistory2.locations[j].longitudeE7;
                        var HaveWeMet = LocationHistoryHelperMethods.LocationsCoincide(lat1, lon1, lat2, lon2);
                        //Checks if the coordinates are relatively close
                        if (HaveWeMet == true)
                        {
                            Console.WriteLine("We have met!\n\nLatitude: " + lat1 + "\tLongitude: " + lon1);
                            Console.WriteLine("\nDate: " + date1 + "\tUnix Time: " + locationHistory1.locations[i].timestampMs);
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
