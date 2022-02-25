using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using csharp_api_example;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace csharp_api_tests
{
    [TestClass]
    public class HandlerTest
    {
        [TestMethod]
        public void TestGetResults()
        {
            ILogger<BackgroundService> logger = TestLogging.CreateLogger<BackgroundService>();
            Handler handler = new Handler(logger);
            var result = handler.GetResults("https://api.listhub.com/public_sandbox/odata/Property?$top=1");
            Assert.AreEqual(1, result.Property("value").Value.ToList<JToken>().Count, "GetResult count is not correct.");
        }
    }
}
