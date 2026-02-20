using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BridgertonGame.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialMySqlMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdminCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminCredentials", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FamilyId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FamilyName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    VotingEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Revealed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LadyWhistledownId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GameScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GameName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FamilyId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameScores", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FamilyId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsLadyWhistledown = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PublicationCooldowns",
                columns: table => new
                {
                    FamilyId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastPublicationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationCooldowns", x => x.FamilyId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WhistledownPenalties",
                columns: table => new
                {
                    FamilyId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Penalty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhistledownPenalties", x => x.FamilyId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AdminCredentials",
                columns: new[] { "Id", "Password", "Username" },
                values: new object[] { 1, "bridgerton2024", "admin" });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Content", "FamilyId", "FamilyName", "PublishedAt", "Title" },
                values: new object[,]
                {
                    { "1", "La notation que la personne va écrire", "hastings", "Hastings", new DateTime(2026, 2, 20, 1, 10, 56, 362, DateTimeKind.Utc).AddTicks(9840), "Chers amis lecteurs," },
                    { "2", "Un événement des plus intéressants s'est déroulé lors du dernier bal...", "bridgerton", "Bridgerton", new DateTime(2026, 2, 20, 3, 10, 56, 362, DateTimeKind.Utc).AddTicks(9840), "Chers amis lecteurs," },
                    { "3", "Les rumeurs circulent à propos d'une certaine famille...", "featherington", "Featherington", new DateTime(2026, 2, 20, 5, 10, 56, 362, DateTimeKind.Utc).AddTicks(9840), "Chers amis lecteurs," },
                    { "4", "Les secrets de la haute société ne me sont pas étrangers...", "hastings", "Hastings", new DateTime(2026, 2, 20, 7, 10, 56, 362, DateTimeKind.Utc).AddTicks(9840), "Chers amis lecteurs," },
                    { "5", "Une nouvelle intrigue secoue les salons londoniens...", "danbury", "Danbury", new DateTime(2026, 2, 20, 9, 10, 56, 362, DateTimeKind.Utc).AddTicks(9840), "Chers amis lecteurs," }
                });

            migrationBuilder.InsertData(
                table: "Families",
                columns: new[] { "Id", "LadyWhistledownId", "Name", "Points", "Rank", "Revealed", "VotingEnabled" },
                values: new object[,]
                {
                    { "bridgerton", "b1", "Bridgerton", 210, 2, false, false },
                    { "danbury", null, "Danbury", 150, 4, false, false },
                    { "featherington", "f1", "Featherington", 180, 3, false, false },
                    { "hastings", "h1", "Hastings", 230, 1, false, false },
                    { "sharma", "s1", "Sharma", 120, 5, false, false }
                });

            migrationBuilder.InsertData(
                table: "GameScores",
                columns: new[] { "Id", "FamilyId", "GameName", "Score" },
                values: new object[,]
                {
                    { 1, "hastings", "Total", 230 },
                    { 2, "bridgerton", "Total", 230 },
                    { 3, "featherington", "Total", 230 },
                    { 4, "danbury", "Total", 230 },
                    { 5, "sharma", "Total", 230 },
                    { 6, "hastings", "Jeu 1", 230 },
                    { 7, "bridgerton", "Jeu 1", 230 },
                    { 8, "featherington", "Jeu 1", 230 },
                    { 9, "danbury", "Jeu 1", 230 },
                    { 10, "sharma", "Jeu 1", 230 },
                    { 11, "hastings", "Jeu 2", 230 },
                    { 12, "bridgerton", "Jeu 2", 230 },
                    { 13, "featherington", "Jeu 2", 230 },
                    { 14, "danbury", "Jeu 2", 230 },
                    { 15, "sharma", "Jeu 2", 230 },
                    { 16, "hastings", "Jeu 3", 230 },
                    { 17, "bridgerton", "Jeu 3", 230 },
                    { 18, "featherington", "Jeu 3", 230 },
                    { 19, "danbury", "Jeu 3", 230 },
                    { 20, "sharma", "Jeu 3", 230 },
                    { 21, "hastings", "Whistledown", 0 },
                    { 22, "bridgerton", "Whistledown", -10 },
                    { 23, "featherington", "Whistledown", 0 },
                    { 24, "danbury", "Whistledown", -10 },
                    { 25, "sharma", "Whistledown", 0 }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Code", "FamilyId", "ImageUrl", "IsLadyWhistledown", "Name", "Role", "Title" },
                values: new object[,]
                {
                    { "b1", "DAPHNE2024", "bridgerton", "images/AdminAvatar.png", true, "Daphné Bridgerton", "Lady Whistledown", "DUCHESSE" },
                    { "b2", "SIMON2024", "bridgerton", "images/AdminAvatar.png", false, "Simon Bridgerton", "Invité", "DUC" },
                    { "b3", "ELOISE2024", "bridgerton", "images/AdminAvatar.png", false, "Eloïse Bridgerton", "Invitée", "LADY" },
                    { "d1", "AGATHA2024", "danbury", "images/AdminAvatar.png", false, "Agatha Danbury", "Maîtresse de soirée", "LADY" },
                    { "d2", "WILL2024", "danbury", "images/AdminAvatar.png", false, "Will Danbury", "Invité", "LORD" },
                    { "f1", "PENELOPE2024", "featherington", "images/AdminAvatar.png", true, "Penelope Featherington", "Lady Whistledown", "LADY" },
                    { "f2", "PORTIA2024", "featherington", "images/AdminAvatar.png", false, "Portia Featherington", "Invitée", "LADY" },
                    { "h1", "CELIA2024", "hastings", "images/AdminAvatar.png", true, "Célia Hastings", "Lady Whistledown", "DUCHESSE" },
                    { "h2", "FANNY2024", "hastings", "images/AdminAvatar.png", false, "Fanny Hastings", "Invitée", "DUCHESSE" },
                    { "h3", "HUGO2024", "hastings", "images/AdminAvatar.png", false, "Hugo Hastings", "Invité", "DUC" },
                    { "s1", "KATE2024", "sharma", "images/AdminAvatar.png", true, "Kate Sharma", "Lady Whistledown", "LADY" },
                    { "s2", "EDWINA2024", "sharma", "images/AdminAvatar.png", false, "Edwina Sharma", "Invitée", "LADY" }
                });

            migrationBuilder.InsertData(
                table: "WhistledownPenalties",
                columns: new[] { "FamilyId", "Penalty" },
                values: new object[,]
                {
                    { "bridgerton", -10 },
                    { "danbury", -10 },
                    { "featherington", 0 },
                    { "hastings", 0 },
                    { "sharma", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminCredentials");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "GameScores");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "PublicationCooldowns");

            migrationBuilder.DropTable(
                name: "WhistledownPenalties");
        }
    }
}
