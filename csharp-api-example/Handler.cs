using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using csharp_api_example.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace csharp_api_example
{
    // Handles the requests to the api and adding/updating Properties in the DB
    public class Handler
    {
        Stopwatch Parserwatch = new Stopwatch();
        private readonly ILogger<BackgroundService> _logger;
        Stopwatch APIwatch = new Stopwatch();
        Stopwatch DBwatch = new Stopwatch();
        private List<String> APIPropertyKeys = new List<string>();

        public Handler(ILogger<BackgroundService> logger)
        {
            _logger = logger;
        }

        // Start loop to get results from url, parse them and add to db/delete from db
        internal void BeginProcessing(string url, DateTime previousSync, PropertyContext dbContext, bool delete = false)
        {
            var nextLink = true;
            var count = 0;

            // Api does not supply a nextLink when the end of the listings has been reached,
            // so we can use the nextLink to tell us whether or not we need to query the API more.
            while (nextLink)
            {
                try
                {
                    if (url != null)
                    {
                        var results = GetResults(url);
                        var properties = results.Property("value").Value.ToList<JToken>();
                        count += properties.Count;
                        _logger.LogWarning("Properties downloaded from API: " + count);
                        ParseProperties(properties, previousSync, dbContext);

                        if (results.Property("@odata.nextLink") != null)
                        {
                            var newUrl = results.Property("@odata.nextLink").Value.ToString();
                            _logger.LogWarning("NEXT LINK = " + newUrl);
                            if (newUrl != null)
                            {
                                url = newUrl;
                            }
                            else
                            {
                                _logger.LogWarning("URL IS NULL");
                                break;
                            }
                        }
                        else
                        {
                            nextLink = false;
                        }
                    }
                } catch (Exception ex)
                {
                    _logger.LogCritical(ex.StackTrace);
                    _logger.LogWarning("Failed in API processing loop, trying again in 10 seconds.");
                    Thread.Sleep(10000);
                }
            }

            if (delete)
            {
                DeleteProperties(dbContext);
            }
        }

        // Get the JSON results from the API
        public JObject GetResults(string url, int fails = 1)
        {
            string result;
            JObject jsonResult;

            try
            {
                using (var webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(url))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            APIwatch.Start();
                            result = streamReader.ReadToEnd();
                            APIwatch.Stop();
                            _logger.LogCritical("API call timer: " + APIwatch.Elapsed.ToString());
                            jsonResult = JObject.Parse(result);

                            _logger.LogWarning("Worker running at: {time} " + url, DateTimeOffset.Now);

                            return jsonResult;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (fails == 5)
                {
                    _logger.LogError("Failed to get results from API 5 times.");
                    throw new Exception(ex.Message);
                }

                _logger.LogWarning("Failed to get data, trying again in 10 seconds");
                Thread.Sleep(10000);
                GetResults(url, ++fails);
            }

            return null;
        }

        // Parses properties from the API and adds them to the DB
        public void ParseProperties(List<JToken> properties, DateTime previousSync, PropertyContext dbContext)
        {
            var props = new List<Property>();
            var parser = new PropertyParser();

            // Each property is parsed into the Property model and added to the list of properties
            Parserwatch.Start();
            properties.ForEach(x =>
            {
                var prop = JObject.Parse(x.ToString());
                var parsedProperty = parser.ParseProperty(prop);
                props.Add(parsedProperty);
                APIPropertyKeys.Add(parsedProperty.ListingKey);
            });
            Parserwatch.Stop();
            _logger.LogCritical("Property parser timer: " + Parserwatch.Elapsed.ToString());

            _logger.LogWarning("Beginning DB write");
            WriteToDB(props, dbContext, previousSync);
            _logger.LogWarning("DB write complete");
        }

        // Gets a List of all Properties from DB
        public static List<Property> GetPropertiesFromDB(PropertyContext c, int fails = 0)
        {
            List<Property> properties;
            properties = c.Property.AsNoTracking().ToList();
            return properties;
        }

        public static void DeletePropertiesFromDB(PropertyContext c, List<String> props, int fails = 0)
        {
            foreach (var prop in props)
            {
                c.Property.Remove(c.Property.FirstOrDefault(x => x.ListingKey.Equals(prop)));
            }
            c.SaveChanges();
            c.ChangeTracker.Clear();
        }

        // Writes Properties to the DB and updates/deletes/adds photos.
        public void WriteToDB(List<Property> props, PropertyContext c, DateTime previousSync, int fails = 0, bool noPhotos = true)
        {
            foreach (Property prop in props)
            {
                if (c.Property.Any(p => p.ListingKey.Equals(prop.ListingKey)))
                {
                    DBwatch.Start();
                    c.Property.Update(prop);
                    DBwatch.Stop();
                }
                else
                {
                    DBwatch.Start();
                    c.Property.Add(prop);
                    DBwatch.Stop();
                }
            }
            DBwatch.Start();
            c.SaveChanges();
            c.ChangeTracker.Clear();
            DBwatch.Stop();
            _logger.LogCritical("DB transaction timer: " + DBwatch.Elapsed.ToString());
        }

        public void DeleteProperties(PropertyContext dbContext)
        {
            // Remove properties no longer in the API
            _logger.LogWarning("Beginning DB deletes");
            var propsToDelete = new List<String>();
            var propsInDB = GetPropertiesFromDB(dbContext);
            _logger.LogWarning("Retrieved props from db.");
            var DBpropsKeys = new List<string>();

            foreach (var prop in propsInDB)
            {
                DBpropsKeys.Add(prop.ListingKey);
            }

            _logger.LogWarning("Created list of listing keys, checking for props that need deleted.");

            foreach (var DBpropKey in DBpropsKeys)
            {
                // DBpropsKeys are the keys of Database Property records
                // APIPropertyKeys is list of property keys currently in the API,
                // this means if the DB contains a listing key not in the API
                // it can be deleted from the DB
                if (!APIPropertyKeys.Contains(DBpropKey))
                {
                    _logger.LogWarning("Deleting " + DBpropKey);
                    propsToDelete.Add(DBpropKey);
                }
            }
            DeletePropertiesFromDB(dbContext, propsToDelete);
            _logger.LogWarning("Deletes complete");
        }
    }
}
