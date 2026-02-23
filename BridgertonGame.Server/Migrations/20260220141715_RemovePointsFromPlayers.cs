using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BridgertonGame.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovePointsFromPlayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "1",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 4, 17, 15, 77, DateTimeKind.Utc).AddTicks(4159));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "2",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 6, 17, 15, 77, DateTimeKind.Utc).AddTicks(4159));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "3",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 8, 17, 15, 77, DateTimeKind.Utc).AddTicks(4159));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "4",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 10, 17, 15, 77, DateTimeKind.Utc).AddTicks(4159));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "5",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 12, 17, 15, 77, DateTimeKind.Utc).AddTicks(4159));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "1",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 4, 15, 7, 272, DateTimeKind.Utc).AddTicks(311));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "2",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 6, 15, 7, 272, DateTimeKind.Utc).AddTicks(311));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "3",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 8, 15, 7, 272, DateTimeKind.Utc).AddTicks(311));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "4",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 10, 15, 7, 272, DateTimeKind.Utc).AddTicks(311));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "5",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 12, 15, 7, 272, DateTimeKind.Utc).AddTicks(311));
        }
    }
}
