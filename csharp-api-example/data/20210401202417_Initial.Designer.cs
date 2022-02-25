﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace csharp_api_example.data
{
    [DbContext(typeof(PropertyContext))]
    [Migration("20210401202417_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("csharp_api_pull.data.Property", b =>
                {
                    b.Property<string>("ListingKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("BathroomsTotalInteger")
                        .HasColumnType("int");

                    b.Property<int?>("BedroomsTotal")
                        .HasColumnType("int");

                    b.Property<string>("Cooling")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("FireplaceYN")
                        .HasColumnType("bit");

                    b.Property<string>("Heating")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("LeadRoutingEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("ListPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("ListingContractDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ListingId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Media")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModificationTimestamp")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("PhotosCount")
                        .HasColumnType("int");

                    b.Property<string>("PostalCity")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PropertySubType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PropertyType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StandardStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StateOrProvince")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnparsedAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("WaterfrontYN")
                        .HasColumnType("bit");

                    b.HasKey("ListingKey");

                    b.HasIndex("ListPrice");

                    b.HasIndex("ListingKey");

                    b.HasIndex("PostalCity");

                    b.HasIndex("PostalCode");

                    b.ToTable("Property");
                });
#pragma warning restore 612, 618
        }
    }
}
