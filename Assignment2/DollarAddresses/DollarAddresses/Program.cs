using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
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
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var json = "";
            var municipality = config["municipality"];
            var outFields = config["outFields"];
            var resultCount = config["resultCount"];
            var f = config["f"];

            string url = "https://gis.maine.gov/arcgis/rest/services/Location/Maine_E911_Addresses_Roads_PSAP/MapServer/1/query?where=MUNICIPALITY%3D%27" 
                         + municipality + "%27&outFields=" + outFields + "&resultRecordCount="+ resultCount + "&f=" + f;

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
                FilterDollarAddresses(jAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine("We had a problem: " + e.Message.ToString());
            }
        }

        public static void DisplayDeserializedJSON(Object jAddress)
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

        public static int LetterValue(char c)
        {
            //Converts char to lower case
            char letter = char.ToLower(c);

            //Gets the character's ASCII value
            int number = (int)letter;

            //Only returns values for characters A-Z
            if (number >= 97 && number <= 122)
            {
                return number - 96;
            }
            return 0;
        }

        public static int getWordValue(string word)
        {
            int value = 0;
            foreach (char c in word)
            {
                //Adds the value of each character
                value += LetterValue(c);
            }
            return value;
        }

        public static Boolean IsItDollarAddress(int streetNameValue, int streetSuffixValue,  int addressNumber)
        {
            if((streetNameValue + streetSuffixValue) == addressNumber) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void FilterDollarAddresses(Object jAddress)
        {
            List<Object.Features> dollarAddresses = new List<Object.Features>();
            int streetNameValue;
            int streetSuffixValue;
            int addressNumber;

            foreach(var address in jAddress.features)
            {
                streetNameValue = getWordValue(address.attributes.STREETNAME);
                streetSuffixValue = getWordValue(address.attributes.SUFFIX);
                addressNumber = address.attributes.ADDRESS_NUMBER;

                if(IsItDollarAddress(streetNameValue, streetSuffixValue, addressNumber))
                {
                    dollarAddresses.Add(address);
                }
            }

            DisplayDollarAddresses(dollarAddresses);
        }

        public static void DisplayDollarAddresses(List<Object.Features> addresses)
        {
            if(addresses.Count == 0)
            {
                Console.WriteLine("There were no dollar addresses found");
            }
            else
            {
                Console.WriteLine("Dollar addresses from " + addresses[0].attributes.MUNICIPALITY + ":\n");
                foreach (var address in addresses)
                {
                    Console.WriteLine(address.attributes.ADDRESS_NUMBER + " " + address.attributes.STREETNAME + " " + address.attributes.SUFFIX);
                }
            }           
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
