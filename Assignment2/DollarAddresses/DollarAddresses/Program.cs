using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;


namespace DollarAddresses
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        public static void Run()
        {
            string json = FetchAddresses();
            DesirializeJSON(json);
        }

        public static string FetchAddresses()
        {
            var json = "";
            string url = "https://gis.maine.gov/arcgis/rest/services/Location/Maine_E911_Addresses_Roads_PSAP/MapServer/1/query?where=MUNICIPALITY%3D%27Portland%27&text=&objectIds=&time=&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&relationParam=&outFields=ADDRESS_NUMBER%2CSTREETNAME%2CSUFFIX%2CMUNICIPALITY&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&returnIdsOnly=false&returnCountOnly=false&orderByFields=address_number&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&returnDistinctValues=false&resultOffset=0&resultRecordCount=&f=pjson";            
            using (WebClient wc = new WebClient())
            {
                json = wc.DownloadString(url);
            }
            return json;
        }

        public static void DesirializeJSON(string json)
        {
            try
            {
                var jAddress = JsonConvert.DeserializeObject<Object>(json);
                PrintDeserializedJSON(jAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine("We had a problem: " + e.Message.ToString());
            }

        }

        public static void PrintDeserializedJSON(Object jAddress)
        {
            Console.WriteLine("displayFieldName : " + jAddress.displayFieldName);

            Console.WriteLine("\nFieldAliases: ");
            Console.WriteLine("ADDRESS_NUMBER : " + jAddress.fieldAliases.ADDRESS_NUMBER);
            Console.WriteLine("STREETNAME : " + jAddress.fieldAliases.STREETNAME);
            Console.WriteLine("SUFFIX : " + jAddress.fieldAliases.SUFFIX);
            Console.WriteLine("MUNICIPALITY : " + jAddress.fieldAliases.MUNICIPALITY);

            Console.WriteLine("\nFields: ");
            foreach (var item in jAddress.fields)
            {
                Console.WriteLine(item.name);
                Console.WriteLine(item.type);
                Console.WriteLine(item.alias);
                Console.WriteLine(item.length);
            }

            Console.WriteLine("\nFeatures: ");
            foreach (var item in jAddress.features)
            {
                Console.WriteLine("\nAttributes:");
                Console.WriteLine("ADDRESS_NUMBER : " + item.attributes.ADDRESS_NUMBER);
                Console.WriteLine("STREETNAME : " + item.attributes.STREETNAME);
                Console.WriteLine("SUFFIX : " + item.attributes.SUFFIX);
                Console.WriteLine("MUNICIPALITY : " + item.attributes.MUNICIPALITY);
            }

            Console.WriteLine("\nexceededTransferLimit : " + jAddress.exceededTransferLimit);
        }
    }


    public class Object
    {
        public string displayFieldName { get; set; }
        public Aliases fieldAliases { get; set; }
        public List<FieldInfo> fields { get; set; }
        public List<Features> features { get; set; }
        public bool exceededTransferLimit { get; set; }


        public class Aliases
        {
            public string ADDRESS_NUMBER { get; set; }
            public string STREETNAME { get; set; }
            public string SUFFIX { get; set; }
            public string MUNICIPALITY { get; set; }
        }

        public class Address
        {
            public int ADDRESS_NUMBER { get; set; }
            public string STREETNAME { get; set; }
            public string SUFFIX { get; set; }
            public string MUNICIPALITY { get; set; }
        }

        public class FieldInfo
        {
            public string name { get; set; }
            public string type { get; set; }
            public string alias { get; set; }
            public int length { get; set; }
        }

        public class Features
        {
            public Address attributes { get; set; }
        }
       
    }

}
