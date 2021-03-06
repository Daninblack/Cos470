﻿using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;


namespace DollarAddresses
{
    public class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        public static void Run()
        {
            string address = SetParameters();
            string json = FetchAddresses(address);
            DesirializeJSON(json);
        }

        public static string SetParameters()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var parameters = System.Web.HttpUtility.ParseQueryString(string.Empty);
            parameters["where"] = config["where"];
            parameters["outFields"] = config["outFields"];
            parameters["f"] = config["f"];
            parameters["resultCount"] = config["resultCount"];

            var address = @"https://gis.maine.gov/arcgis/rest/services/Location/Maine_E911_Addresses_Roads_PSAP/MapServer/1/query?"
                + parameters;

            return address;
        }

        public static string FetchAddresses(string address)
        {
            using (var client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, address))
                {
                    var response = client.SendAsync(request).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"{content}: {response.StatusCode}");
                    }
                    return content;
                }
            }
        }

        public static void DesirializeJSON(string json)
        {
            try
            {
                var jAddress = JsonConvert.DeserializeObject<Object>(json);
                var dollarAddresses = FilterDollarAddresses(jAddress);
                DisplayDollarAddresses(dollarAddresses);
            }
            catch (Exception e)
            {
                Console.WriteLine("We had a problem: " + e.Message.ToString());
            }
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

        public static List<Object.Features> FilterDollarAddresses(Object jAddress)
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

            return dollarAddresses;
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
        public List<Features> features { get; set; }

        public class Address
        {
            public int ADDRESS_NUMBER { get; set; }
            public string STREETNAME { get; set; }
            public string SUFFIX { get; set; }
            public string MUNICIPALITY { get; set; }
        }

        public class Features
        {
            public Address attributes { get; set; }
        }      
    }

}
