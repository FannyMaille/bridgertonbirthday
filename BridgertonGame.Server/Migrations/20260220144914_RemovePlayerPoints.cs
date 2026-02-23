using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BridgertonGame.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovePlayerPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerPoints");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Players");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "1",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 4, 49, 14, 425, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "2",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 6, 49, 14, 425, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "3",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 8, 49, 14, 425, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "4",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 10, 49, 14, 425, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "5",
                column: "PublishedAt",
                value: new DateTime(2026, 2, 20, 12, 49, 14, 425, DateTimeKind.Utc).AddTicks(7152));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PlayerPoints",
                columns: table => new
                {
                    PlayerId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPoints", x => x.PlayerId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "b1",
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "b2",
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "b3",
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "d1",
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "d2",
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "f1",
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "f2",
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "h1",
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "h2",
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "h3",
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "s1",
                column: "Points",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: "s2",
                column: "Points",
                value: 0);
        }
    }
}
