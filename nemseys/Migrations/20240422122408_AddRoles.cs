using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nemesys.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
            { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "Reporters", "REPORTERS" },
            { Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "Investigators", "INVESTIGATORS" }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "134c1566-3f64-4ab4-b1e7-2ffe11f43e32",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "44ba4d8e-8172-4005-8f58-be4456eecd02", "AQAAAAEAACcQAAAAEOksLsdskx0I9ngTKkVhphCBxI7jPrMqJJtopGzUcOqsWHn+MvsqOPg5OIGuIkVGbg==", "f98b62d4-9a79-4f16-a059-a9bae32aeae1" });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 1,
                columns: new[] { "DateAndTimeSpotted", "DateOfReport" },
                values: new object[] { new DateTime(2024, 4, 21, 15, 2, 16, 693, DateTimeKind.Utc).AddTicks(7281), new DateTime(2024, 4, 21, 15, 2, 16, 693, DateTimeKind.Utc).AddTicks(7281) });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 2,
                columns: new[] { "DateAndTimeSpotted", "DateOfReport" },
                values: new object[] { new DateTime(2024, 4, 20, 15, 2, 16, 693, DateTimeKind.Utc).AddTicks(7287), new DateTime(2024, 4, 20, 15, 2, 16, 693, DateTimeKind.Utc).AddTicks(7283) });

            migrationBuilder.UpdateData(
                table: "Reports",
                keyColumn: "ReportId",
                keyValue: 3,
                columns: new[] { "DateAndTimeSpotted", "DateOfReport" },
                values: new object[] { new DateTime(2024, 4, 19, 15, 2, 16, 693, DateTimeKind.Utc).AddTicks(7288), new DateTime(2024, 4, 19, 15, 2, 16, 693, DateTimeKind.Utc).AddTicks(7288) });
        }
    }
}
