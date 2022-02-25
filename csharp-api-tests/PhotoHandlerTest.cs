using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using csharp_api_example;
using System;

namespace csharp_api_tests
{
    [TestClass]
    public class PhotoHandlerTest
    {
        [TestMethod]
        public void TestSavePhoto()
        {
            ILogger<BackgroundService> logger = TestLogging.CreateLogger<BackgroundService>();
            PhotoHandler handler = new PhotoHandler(logger);
            var saveFolder = "ListingKey000/MediaKey000";
            var downloaded = handler.SavePhoto("https://www.publicdomainpictures.net/pictures/40000/velka/teapot.jpg", saveFolder, DateTime.Now, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            Assert.AreEqual(true, downloaded, "Photo Not Downloaded correclty");
        }

        [TestMethod]
        public void TestDeletePhoto()
        {
            ILogger<BackgroundService> logger = TestLogging.CreateLogger<BackgroundService>();
            PhotoHandler handler = new PhotoHandler(logger);
            var saveFolder = "ListingKey000/MediaKey000";
            handler.SavePhoto("https://www.publicdomainpictures.net/pictures/40000/velka/teapot.jpg", saveFolder, DateTime.Now, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            var deleted = handler.DeletePhoto("ListingKey000", "MediaKey000");
            Assert.AreEqual(true, deleted, "Photo Not Deleted correclty");
        }

        [TestMethod]
        public void TestDeletePhotos()
        {
            ILogger<BackgroundService> logger = TestLogging.CreateLogger<BackgroundService>();
            PhotoHandler handler = new PhotoHandler(logger);
            var saveFolder = "ListingKey000/MediaKey000";
            handler.SavePhoto("https://www.publicdomainpictures.net/pictures/40000/velka/teapot.jpg", saveFolder, DateTime.Now, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            var deleted = handler.DeletePhotos("ListingKey000");
            Assert.AreEqual(true, deleted, "Photo Directory Not Deleted correclty");
        }
    }
}
