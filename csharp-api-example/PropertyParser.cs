using System;
using System.Globalization;
using csharp_api_example.data;
using Newtonsoft.Json.Linq;

namespace csharp_api_example
{
    // Parses JSON Property object to the Property model.
    public class PropertyParser
    {
        public PropertyParser()
        {
        }

        public Property ParseProperty(JObject p)
        {
            var r = new Property();
            // The custom parser is recommended to ensure nulls and things are set the way you want them to be.
            // Most values in the api are not guaranteed to be populated, it is best to
            // tryParse or have some form of validation of all fields.

            // Parsing different data types individually
            r = ParseInt32FromProperty(p, r);
            r = ParseBoolFromProperty(p, r);
            r = ParseDecimalFromProperty(p, r);
            r = ParseDatesFromProperty(p, r);
            r = ParseStringsFromProperty(p, r);

            return r;
        }

        public Property ParseStringsFromProperty(JObject p, Property r)
        {
            r.ListingKey = p.Property("ListingKey").Value.ToString();
            r.ListingId = p.Property("ListingId").Value.ToString();
            r.PropertyType = p.Property("PropertyType").Value.ToString() ?? null;
            r.PropertySubType = p.Property("PropertySubType").Value.ToString() ?? null;
            r.UnparsedAddress = p.Property("UnparsedAddress").Value.ToString() ?? null;
            r.PostalCity = p.Property("PostalCity").Value.ToString() ?? null;
            r.StateOrProvince = p.Property("StateOrProvince").Value.ToString() ?? null;
            r.Country = p.Property("Country").Value.ToString() ?? null;
            r.PostalCode = p.Property("PostalCode").Value.ToString() ?? null;
            r.StandardStatus = p.Property("StandardStatus").Value.ToString() ?? null;
            r.Cooling = p.Property("Cooling").Value.ToString() ?? null;
            r.Heating = p.Property("Heating").Value.ToString() ?? null;
            r.Media = p.Property("Media").Value.ToString() ?? null;
            String CustomFields = p.Property("CustomFields").Value.ToString() ?? null;

            if (CustomFields != null)
            {
                JObject Fields = JObject.Parse(CustomFields);
                r.LeadRoutingEmail = Fields.Property("LeadRoutingEmail").Value.ToString() ?? null;
            }

            return r;
        }

        public Property ParseDatesFromProperty(JObject p, Property r)
        {
            DateTime ListingContractDate = DateTime.MinValue;
            String lcd = p.Property("ListingContractDate").Value.ToString();

            DateTime.TryParse(lcd, out ListingContractDate);

            if (ListingContractDate.Equals(DateTime.MinValue))
            {
                r.ListingContractDate = null;
            }
            else
            {
                r.ListingContractDate = ListingContractDate;
            }

            r.ModificationTimestamp = DateTimeOffset.ParseExact(
                p.Property("ModificationTimestamp").Value
                    .ToString(),
                "MM/dd/yyyy HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal);

            return r;
        }

        public Property ParseDecimalFromProperty(JObject p, Property r)
        {
            r.ListPrice = decimal.Parse(p.Property("ListPrice").Value.ToString() ?? decimal.MinValue.ToString());
            r.Latitude = decimal.Parse(p.Property("Latitude").Value.ToString() ?? decimal.MinValue.ToString());
            r.Longitude = decimal.Parse(p.Property("Longitude").Value.ToString() ?? decimal.MinValue.ToString());

            return r;
        }

        public Property ParseBoolFromProperty(JObject p, Property r)
        {
            r.FireplaceYN = bool.Parse(p.Property("FireplaceYN").Value.ToString() ?? false.ToString());
            r.WaterfrontYN = bool.Parse(p.Property("WaterfrontYN").Value.ToString() ?? false.ToString());

            return r;
        }

        public Property ParseInt32FromProperty(JObject p, Property r)
        {
            r.BathroomsTotalInteger = Int32.Parse(p.Property("BathroomsTotalInteger").Value.ToString() ?? Int32.MinValue.ToString());
            r.BedroomsTotal = Int32.Parse(p.Property("BedroomsTotal").Value.ToString() ?? Int32.MinValue.ToString());

            return r;
        }
    }
}
