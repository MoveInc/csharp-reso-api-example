using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace csharp_api_example.data
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    ListingKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModificationTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ListingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertySubType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UnparsedAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCity = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StateOrProvince = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BathroomsTotalInteger = table.Column<int>(type: "int", nullable: true),
                    BedroomsTotal = table.Column<int>(type: "int", nullable: true),
                    StandardStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhotosCount = table.Column<int>(type: "int", nullable: true),
                    ListingContractDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cooling = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Heating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FireplaceYN = table.Column<bool>(type: "bit", nullable: false),
                    WaterfrontYN = table.Column<bool>(type: "bit", nullable: false),
                    Media = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeadRoutingEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.ListingKey);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Property_ListingKey",
                table: "Property",
                column: "ListingKey");

            migrationBuilder.CreateIndex(
                name: "IX_Property_ListPrice",
                table: "Property",
                column: "ListPrice");

            migrationBuilder.CreateIndex(
                name: "IX_Property_PostalCity",
                table: "Property",
                column: "PostalCity");

            migrationBuilder.CreateIndex(
                name: "IX_Property_PostalCode",
                table: "Property",
                column: "PostalCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Property");
        }
    }
}
