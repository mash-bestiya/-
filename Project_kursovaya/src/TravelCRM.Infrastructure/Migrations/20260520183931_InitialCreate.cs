using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tourists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    MiddleName = table.Column<string>(type: "TEXT", nullable: false),
                    EngFirstName = table.Column<string>(type: "TEXT", nullable: false),
                    EngLastName = table.Column<string>(type: "TEXT", nullable: false),
                    PassportInfo = table.Column<string>(type: "TEXT", nullable: false),
                    IntPassportInfo = table.Column<string>(type: "TEXT", nullable: false),
                    VisaInfo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tourists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinkedTourists",
                columns: table => new
                {
                    MainTouristId = table.Column<int>(type: "INTEGER", nullable: false),
                    LinkedTouristId = table.Column<int>(type: "INTEGER", nullable: false),
                    RelationType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedTourists", x => new { x.MainTouristId, x.LinkedTouristId });
                    table.ForeignKey(
                        name: "FK_LinkedTourists_Tourists_LinkedTouristId",
                        column: x => x.LinkedTouristId,
                        principalTable: "Tourists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LinkedTourists_Tourists_MainTouristId",
                        column: x => x.MainTouristId,
                        principalTable: "Tourists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DepartureCity = table.Column<string>(type: "TEXT", nullable: false),
                    ArrivalCity = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HotelName = table.Column<string>(type: "TEXT", nullable: false),
                    PaymentStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    TouristId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Tourists_TouristId",
                        column: x => x.TouristId,
                        principalTable: "Tourists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TripId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TripId",
                table: "Documents",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedTourists_LinkedTouristId",
                table: "LinkedTourists",
                column: "LinkedTouristId");

            migrationBuilder.CreateIndex(
                name: "IX_Tourists_Phone",
                table: "Tourists",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TouristId",
                table: "Trips",
                column: "TouristId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "LinkedTourists");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Tourists");
        }
    }
}
