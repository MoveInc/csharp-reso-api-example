using System;
using Microsoft.EntityFrameworkCore;

namespace csharp_api_example.data
{
    public class PropertyContext : DbContext
    {
        public DbSet<Property> Property { get; set; }

        public PropertyContext()
        {
        }

        public PropertyContext(DbContextOptions<PropertyContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Should use a config file and load password from somewhere else.
            optionsBuilder.UseSqlServer(@"Server=db;Database=master;User=sa;Password=Your_password123;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>(b =>
            {
                // Some possibly useful indexes from what we are pulling from the API.
                b.HasIndex(x => x.ListingKey);
                b.HasIndex(x => x.PostalCode);
                b.HasIndex(x => x.ListPrice);
                b.HasIndex(x => x.PostalCity);
                b.HasKey(x => x.ListingKey);
            });
        }
    }

    // The Property represents a listing from the API, you can create more migrations to add
    // values to the database and then add the values here to use them.
    public class Property
    {
        public string ListingKey { get; set; }
        public DateTimeOffset ModificationTimestamp { get; set; }
        public string ListingId { get; set; }
        public string PropertyType { get; set; } = null!;
        public string PropertySubType { get; set; } = null!;
        public decimal? ListPrice { get; set; } = null!;
        public string UnparsedAddress { get; set; } = null!;
        public string PostalCity { get; set; } = null!;
        public string StateOrProvince { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public Int32? BathroomsTotalInteger { get; set; } = null!;
        public Int32? BedroomsTotal { get; set; } = null!;
        public string StandardStatus { get; set; } = null!;
        public decimal? Latitude { get; set; } = null!;
        public decimal? Longitude { get; set; } = null!;
        public Int32? PhotosCount { get; set; } = null!;
        public DateTime? ListingContractDate { get; set; } = null!;
        public string Cooling { get; set; } = null!;
        public string Heating { get; set; } = null!;
        public bool FireplaceYN { get; set; }
        public bool WaterfrontYN { get; set; }
        public string Media { get; set; } = null!;
        public string LeadRoutingEmail { get; set; } = null!;
    }
}