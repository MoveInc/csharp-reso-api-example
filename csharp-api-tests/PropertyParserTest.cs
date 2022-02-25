using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using csharp_api_example;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using csharp_api_example.data;
using System.Collections.Generic;

namespace csharp_api_tests
{
    [TestClass]
    public class PropertyParserTest
    {
        ILogger<BackgroundService> logger;
        Handler handler;
        PropertyParser parser;
        JObject result;
        List<JToken> properties;
        JObject firstProp;

        [TestInitialize]
        public void TestInitialize()
        {
            logger = TestLogging.CreateLogger<BackgroundService>();
            handler = new Handler(logger);
            result = handler.GetResults("https://api.listhub.com/public_sandbox/odata/Property?$top=1");
            parser = new PropertyParser();
            properties = result.Property("value").Value.ToList<JToken>();
            firstProp = JObject.Parse(properties.First().ToString());
        }

        [TestMethod]
        public void TestParseStringsFromProperty()
        {
            var prop = new Property();
            var parsed = parser.ParseStringsFromProperty(firstProp, prop);

            Assert.IsNotNull(parsed.ListingKey, "ListingKey has no value.");
            Assert.AreNotEqual("", parsed.ListingKey, "ListingKey is blank.");
            Assert.IsNotNull(parsed.ListingId, "ListingId has no value.");
            Assert.AreNotEqual("", parsed.ListingId, "ListingId is blank.");
        }

        [TestMethod]
        public void TestParseDatesFromProperty()
        {
            var prop = new Property();
            var parsed = parser.ParseDatesFromProperty(firstProp, prop);

            Assert.IsNotNull(parsed.ModificationTimestamp, "ModificationTimestamp is Null");
        }

        [TestMethod]
        public void TestParseDecimalFromProperty()
        {
            var prop = new Property();
            var parsed = parser.ParseDecimalFromProperty(firstProp, prop);

            Assert.IsNotNull(parsed.ListPrice, "ListPrice is Null");
            Assert.IsNotNull(parsed.Latitude, "Latitude is Null");
            Assert.IsNotNull(parsed.Longitude, "Longitude is Null");
        }

        [TestMethod]
        public void TestParseBoolFromProperty()
        {
            var prop = new Property();
            var parsed = parser.ParseBoolFromProperty(firstProp, prop);

            Assert.IsNotNull(parsed.FireplaceYN, "FireplaceYN is Null");
            Assert.IsNotNull(parsed.WaterfrontYN, "WaterfrontYN is Null");
        }

        [TestMethod]
        public void TestParseInt32FromProperty()
        {
            var prop = new Property();
            var parsed = parser.ParseInt32FromProperty(firstProp, prop);

            Assert.IsNotNull(parsed.BathroomsTotalInteger, "BathroomsTotalInteger is Null");
            Assert.IsNotNull(parsed.BedroomsTotal, "BedroomsTotal is Null");
        }
    }
}
