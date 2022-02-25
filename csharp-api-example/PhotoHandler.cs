using System;
using System.Net;
using System.IO;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace csharp_api_example
{
    public class PhotoHandler
    {
        private readonly ILogger<BackgroundService> _logger;

        public PhotoHandler(ILogger<BackgroundService> logger)
        {
            _logger = logger;
        }

        // Saves a listing photo if needed
        public bool SavePhoto(String remoteFileUrl, String localFileName, DateTime previousSync, String mediaMod)
        {
            var lastUpdate = DateTime.ParseExact
            (
                mediaMod,
                "MM/dd/yyyy HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal
            );

            var localFolder = localFileName.Split("/")[0];
            Directory.CreateDirectory("./photos/" + localFolder);
            var localFile = "./photos/" + localFileName + ".jpg";

            // photo updated after the most recent pull, or the photo does not exist
            var update = lastUpdate > previousSync;
            var exists = !File.Exists(localFile);
            if (update || exists)
            {
                WebClient webClient = new WebClient();

                webClient.DownloadFileTaskAsync(remoteFileUrl, localFile);

                return true;
            }

            return false;
        }

        // Deletes photos associated with a listing
        public bool DeletePhotos(String listingkey)
        {
            var localFolder = "./photos/" + listingkey;
            try
            {
                Directory.Delete(localFolder, true);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }
        }

        // Deletes a single photo from a listing
        public bool DeletePhoto(String listingkey, String mediakey)
        {
            try
            {
                var localFile = "./photos/" + listingkey + "/" + mediakey + ".jpg";
                File.Delete(localFile);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }
        }
    }
}
