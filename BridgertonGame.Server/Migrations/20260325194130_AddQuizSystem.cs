using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BridgertonGame.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminCredentials",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$fd4VKSrWf1o25Zm7lAzeC.p7SX9Mj4vDPk5xahFCbcKdqPUuaSkgq");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "1",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 25, 9, 41, 29, 635, DateTimeKind.Utc).AddTicks(9676));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "2",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 25, 11, 41, 29, 635, DateTimeKind.Utc).AddTicks(9676));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "3",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 25, 13, 41, 29, 635, DateTimeKind.Utc).AddTicks(9676));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "4",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 25, 15, 41, 29, 635, DateTimeKind.Utc).AddTicks(9676));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "5",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 25, 17, 41, 29, 635, DateTimeKind.Utc).AddTicks(9676));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminCredentials",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$AxKjwiqhEjgx8ngYgwrxPupI0CLjBFPWnl5FY0FGsxulntnZ11AeO");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "1",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 25, 9, 28, 54, 767, DateTimeKind.Utc).AddTicks(2413));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "2",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 25, 11, 28, 54, 767, DateTimeKind.Utc).AddTicks(2413));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "3",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 25, 13, 28, 54, 767, DateTimeKind.Utc).AddTicks(2413));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "4",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 25, 15, 28, 54, 767, DateTimeKind.Utc).AddTicks(2413));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "5",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 25, 17, 28, 54, 767, DateTimeKind.Utc).AddTicks(2413));
        }
    }
}
