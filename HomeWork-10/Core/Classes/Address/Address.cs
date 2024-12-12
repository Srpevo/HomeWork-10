using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeWork_10.Core.Classes.Address
{
    internal class Address
    {
        [JsonConstructor]
        public Address(string country, string city, string region, int postalCode)
        {
            Country = country;
            City = city;
            Region = region;
            PostalCode = postalCode;
        }
        public Address() { }

        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public int PostalCode { get; set; }
    }
}
