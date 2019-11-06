using NUnit.Framework;
using HaveWeMet;
using System;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDeserializeJSON_ReturnsLocationHistory()
        {
            var filePath = @"C:\Users\Daniel\Desktop\Cos470 folder\Assignment3\HaveWeMet\Tests\LocationHistorySample.txt";
            var json = LocationHistoryHelperMethods.BuildJSONFromFile(filePath);
            var locationHistory = LocationHistoryHelperMethods.DeserializeJSON(json);
            Assert.IsInstanceOf(typeof(LocationHistory), locationHistory);
        }

        [Test]
        public void UnixTimeStampToDateTime_ReturnsDateTime()
        {
            DateTime date = new DateTime(2019, 1, 31, 0, 22, 57);
            string timeStamp = "1548894177190";
            var result = LocationHistoryHelperMethods.UnixTimeStampToDateTime(timeStamp);
            Assert.AreEqual(date, result);
        }

        [Test]
        public void DateTimesCoincide_DateTimesAreClose_ReturnsTrue()
        {
            DateTime date1 = new DateTime(2019, 2, 5, 8, 30, 12);
            DateTime date2 = new DateTime(2019, 2, 5, 8, 15, 12);
            var result = LocationHistoryHelperMethods.DateTimesCoincide(date1, date2);
            Assert.IsTrue(result);
        }

        [Test]
        public void DateTimesCoincide_DateTimesAreTheSame_ReturnsTrue()
        {
            DateTime date1 = new DateTime(2019, 2, 5, 8, 30, 12);
            DateTime date2 = new DateTime(2019, 2, 5, 8, 30, 12);
            var result = LocationHistoryHelperMethods.DateTimesCoincide(date1, date2);
            Assert.IsTrue(result);
        }

        [Test]
        public void DateTimesCoincide_DateTimesAreTooFarApart_ReturnsFalse()
        {
            DateTime date1 = new DateTime(2019, 2, 5, 8, 30, 12);
            DateTime date2 = new DateTime(2019, 2, 6, 8, 30, 12);
            var result = LocationHistoryHelperMethods.DateTimesCoincide(date1, date2);
            Assert.IsFalse(result);
        }

        [Test]
        public void LocationsCoincide_CoordinatesAreClose_ReturnsTrue()
        {
            var longitude1 = -703617207;
            var latitude1 = 436872867;
            var longitude2 = -703618346;
            var latitude2 = 436872848;
            var result = LocationHistoryHelperMethods.LocationsCoincide(latitude1, longitude1, latitude2, longitude2);
            Assert.IsTrue(result);
        }

        [Test]
        public void LocationsCoincide_CoordinatessAreTheSame_ReturnsTrue()
        {
            var longitude1 = -703617207;
            var latitude1 = 436872867;
            var longitude2 = -703617207;
            var latitude2 = 436872867;
            var result = LocationHistoryHelperMethods.LocationsCoincide(latitude1, longitude1, latitude2, longitude2);
            Assert.IsTrue(result);
        }

        [Test]
        public void LocationsCoincide_CoordinatessAreTooFarApart_ReturnsFalse()
        {
            var longitude1 = -703617207;
            var latitude1 = 436872867;
            var longitude2 = -713617207;
            var latitude2 = 446872867;
            var result = LocationHistoryHelperMethods.LocationsCoincide(latitude1, longitude1, latitude2, longitude2);
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckAlibi_GivenExistingDate_ReturnsLocation()
        {
            DateTime date = LocationHistoryHelperMethods.UnixTimeStampToDateTime("1548894177190");
            var filePath = @"C:\Users\Daniel\Desktop\Cos470 folder\Assignment3\HaveWeMet\Tests\LocationHistorySample.txt";
            var json = LocationHistoryHelperMethods.BuildJSONFromFile(filePath);
            var locationHistory = LocationHistoryHelperMethods.DeserializeJSON(json);

            var result = LocationHistoryAnalysis.CheckAlibi(date, locationHistory);
            Assert.IsInstanceOf(typeof(LocationHistory.Location), result);
        }

        [Test]
        public void CheckAlibi_GivenNonExistingDate_ReturnsNull()
        {
            DateTime date = LocationHistoryHelperMethods.UnixTimeStampToDateTime("1001000100100");
            var filePath = @"C:\Users\Daniel\Desktop\Cos470 folder\Assignment3\HaveWeMet\Tests\LocationHistorySample.txt";
            var json = LocationHistoryHelperMethods.BuildJSONFromFile(filePath);
            var locationHistory = LocationHistoryHelperMethods.DeserializeJSON(json);

            var result = LocationHistoryAnalysis.CheckAlibi(date, locationHistory);
            Assert.IsNull(result);
        }

        [Test]
        public void HaveWeMet_WeHaveMet_ReturnsLocation()
        {
            var filePath1 = @"C:\Users\Daniel\Desktop\Cos470 folder\Assignment3\HaveWeMet\Tests\LocationHistorySample.txt";
            var json1 = LocationHistoryHelperMethods.BuildJSONFromFile(filePath1);
            var locationHistory1 = LocationHistoryHelperMethods.DeserializeJSON(json1);

            var filePath2 = @"C:\Users\Daniel\Desktop\Cos470 folder\Assignment3\HaveWeMet\Tests\LocationHistorySample2.txt";
            var json2 = LocationHistoryHelperMethods.BuildJSONFromFile(filePath2);
            var locationHistory2 = LocationHistoryHelperMethods.DeserializeJSON(json2);

            var result = LocationHistoryAnalysis.HaveWeMet(locationHistory1, locationHistory2);

            Assert.IsInstanceOf(typeof(LocationHistory.Location), result);
        }

    }
}