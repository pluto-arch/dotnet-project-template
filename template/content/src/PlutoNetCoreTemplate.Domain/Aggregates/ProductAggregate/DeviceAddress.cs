namespace PlutoNetCoreTemplate.Domain.Aggregates.ProductAggregate
{
    using System.Collections.Generic;
    using Entities;

    /// <summary>
    /// 设备地址值对象
    /// </summary>
    public class DeviceAddress: ValueObject
    {
        public string Street { get; private set; } 

        public string City { get; private set; }

        public string State { get; private set; }

        public string Country { get; private set; }

        public string ZipCode { get; private set; }

        public DeviceAddress() { }

        public DeviceAddress(string street, string city, string state, string country, string zipcode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return ZipCode;
        }
    }
}