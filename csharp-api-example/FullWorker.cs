using System;
using System.Threading;
using csharp_api_example.data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace csharp_api_example
{
    public class FullWorker
    {
        private readonly ILogger<BackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FullWorker(ILogger<BackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Execute()
        {
            var prevDay = DateTime.Now.AddDays(-1).AddHours(-1);

            Execute(prevDay);
        }

        // Gets listings from api and parses them to models for entity framework to add to the DB
        public void Execute(DateTime previousSync, int fails = 0)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PropertyContext>();

            if (dbContext.Database.CanConnect() && fails != 3)
            {
                // /public_sandbox will need removed for normal querying.
                // This process should also be modified to first retrieve
                // a token from https://api.listhub.com/oauth2/token as shown
                // in the documentation at https://api.listhub.com
                var url = "https://api.listhub.com/public_sandbox/odata/Property?" +
                         Uri.EscapeUriString("$select=ListingKey,ListingId,ModificationTimestamp,PropertyType," +
                              "PropertySubType,UnparsedAddress,PostalCity,StateOrProvince,Country,ListPrice,PostalCode," +
                              "BedroomsTotal,BathroomsTotalInteger,StandardStatus,PhotosCount,Cooling,Heating,Latitude," +
                              "Longitude,FireplaceYN,WaterfrontYN,ListingContractDate,CustomFields,Media");
                var handler = new Handler(_logger);
                handler.BeginProcessing(url, previousSync, dbContext, true);
            }
            else if (fails == 3)
            {
                _logger.LogError("Can't get a good connection to the DB, something is wrong.");
            }
            else
            {
                _logger.LogCritical("Cannot connect to db, pausing and trying again");
                // Sleep for 5 seconds and retry connection.
                Thread.Sleep(5000);
                Execute(previousSync, ++fails);
            }
        }
    }
}
