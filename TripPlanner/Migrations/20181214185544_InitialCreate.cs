using Microsoft.EntityFrameworkCore.Migrations;

namespace TripPlanner.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryCode = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryCode);
                });

            migrationBuilder.CreateTable(
                name: "FeatureCategories",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCategories", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "TimeZones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeZones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureCodes",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    FeatureCategoryCode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCodes", x => x.Code);
                    table.ForeignKey(
                        name: "FK_FeatureCodes_FeatureCategories_FeatureCategoryCode",
                        column: x => x.FeatureCategoryCode,
                        principalTable: "FeatureCategories",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeoData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    AlternateNames = table.Column<string>(nullable: true),
                    Longitude = table.Column<float>(nullable: false),
                    Lattitude = table.Column<float>(nullable: false),
                    TimeZoneId = table.Column<int>(nullable: false),
                    CountryCode = table.Column<string>(nullable: false),
                    FeatureCodeCode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeoData_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "CountryCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeoData_FeatureCodes_FeatureCodeCode",
                        column: x => x.FeatureCodeCode,
                        principalTable: "FeatureCodes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeoData_TimeZones_TimeZoneId",
                        column: x => x.TimeZoneId,
                        principalTable: "TimeZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCodes_FeatureCategoryCode",
                table: "FeatureCodes",
                column: "FeatureCategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_GeoData_CountryCode",
                table: "GeoData",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_GeoData_FeatureCodeCode",
                table: "GeoData",
                column: "FeatureCodeCode");

            migrationBuilder.CreateIndex(
                name: "IX_GeoData_TimeZoneId",
                table: "GeoData",
                column: "TimeZoneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoData");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "FeatureCodes");

            migrationBuilder.DropTable(
                name: "TimeZones");

            migrationBuilder.DropTable(
                name: "FeatureCategories");
        }
    }
}
