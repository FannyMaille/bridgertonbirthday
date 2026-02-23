using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BridgertonGame.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPasswordToBCrypt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminCredentials",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$Po5fDepKNZ2z4i7j.rtOXevce5nbBeU88cXQMJUvlxismBqjlyBIO");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "1",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 10, 55, 35, 278, DateTimeKind.Utc).AddTicks(3163));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "2",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 12, 55, 35, 278, DateTimeKind.Utc).AddTicks(3163));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "3",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 14, 55, 35, 278, DateTimeKind.Utc).AddTicks(3163));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "4",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 16, 55, 35, 278, DateTimeKind.Utc).AddTicks(3163));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "5",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 18, 55, 35, 278, DateTimeKind.Utc).AddTicks(3163));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminCredentials",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "bridgerton2024");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "1",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 7, 31, 21, 546, DateTimeKind.Utc).AddTicks(4691));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "2",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 9, 31, 21, 546, DateTimeKind.Utc).AddTicks(4691));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "3",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 11, 31, 21, 546, DateTimeKind.Utc).AddTicks(4691));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "4",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 13, 31, 21, 546, DateTimeKind.Utc).AddTicks(4691));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "5",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 15, 31, 21, 546, DateTimeKind.Utc).AddTicks(4691));
        }
    }
}
