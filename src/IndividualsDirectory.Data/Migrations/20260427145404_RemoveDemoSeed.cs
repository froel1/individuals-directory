using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IndividualsDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDemoSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "IndividualConnections",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "IndividualConnections",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "IndividualConnections",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "IndividualConnections",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "IndividualConnections",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "IndividualConnections",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "IndividualConnections",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "IndividualConnections",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Individuals",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Individuals",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Individuals",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Individuals",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Individuals",
                keyColumn: "Id",
                keyValue: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Individuals",
                columns: new[] { "Id", "CityId", "DateOfBirth", "FirstName", "Gender", "ImageId", "LastName", "PersonalNumber" },
                values: new object[,]
                {
                    { 1, 1, new DateOnly(1990, 5, 15), "Giorgi", 2, null, "Beridze", "01001011001" },
                    { 2, 1, new DateOnly(1992, 8, 20), "Nino", 1, null, "Kapanadze", "01001011002" },
                    { 3, 2, new DateOnly(1985, 3, 10), "Davit", 2, null, "Tsiklauri", "01001011003" },
                    { 4, 3, new DateOnly(1995, 12, 5), "Mariam", 1, null, "Lomidze", "01001011004" },
                    { 5, 4, new DateOnly(1988, 7, 25), "Levan", 2, null, "Adamia", "01001011005" }
                });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "IndividualId", "Number", "Type" },
                values: new object[,]
                {
                    { 1, 1, "+995555111001", 1 },
                    { 2, 1, "+995322111002", 2 },
                    { 3, 2, "+995555222001", 1 },
                    { 4, 3, "+995555333001", 1 },
                    { 5, 4, "+995431444001", 3 },
                    { 6, 5, "+995555555001", 1 }
                });

            migrationBuilder.InsertData(
                table: "IndividualConnections",
                columns: new[] { "Id", "ConnectedIndividualId", "ConnectionType", "IndividualId" },
                values: new object[,]
                {
                    { 1, 2, 1, 1 },
                    { 2, 1, 1, 2 },
                    { 3, 3, 3, 1 },
                    { 4, 1, 3, 3 },
                    { 5, 4, 2, 2 },
                    { 6, 2, 2, 4 },
                    { 7, 5, 4, 3 },
                    { 8, 3, 4, 5 }
                });
        }
    }
}
