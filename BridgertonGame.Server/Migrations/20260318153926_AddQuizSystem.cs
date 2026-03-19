using System;
using Microsoft.EntityFrameworkCore.Metadata;
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
            migrationBuilder.CreateTable(
                name: "QuizAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QuestionNumber = table.Column<int>(type: "int", nullable: false),
                    SelectedAnswer = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsCorrect = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AnsweredAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAnswers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QuizStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CurrentQuestionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizStates", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuestionNumber = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OptionA = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OptionB = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OptionC = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OptionD = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CorrectAnswer = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AdminCredentials",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$.xWcpRppD8w3HclskFBxK.lsb3oPqKPXziJn/3yEpSG0pW65v.n2K");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "1",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 18, 5, 39, 25, 940, DateTimeKind.Utc).AddTicks(815));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "2",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 18, 7, 39, 25, 940, DateTimeKind.Utc).AddTicks(815));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "3",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 18, 9, 39, 25, 940, DateTimeKind.Utc).AddTicks(815));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "4",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 18, 11, 39, 25, 940, DateTimeKind.Utc).AddTicks(815));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "5",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 18, 13, 39, 25, 940, DateTimeKind.Utc).AddTicks(815));

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswers_PlayerId_QuestionNumber",
                table: "QuizAnswers",
                columns: new[] { "PlayerId", "QuestionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_QuestionNumber",
                table: "Quizzes",
                column: "QuestionNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizAnswers");

            migrationBuilder.DropTable(
                name: "QuizStates");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.UpdateData(
                table: "AdminCredentials",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$xAbdCwTiPUumC.pcowKUcepYaNpDva13YT2Kru8kIOSQ5h8QebVHa");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "1",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 11, 0, 5, 25, 841, DateTimeKind.Utc).AddTicks(2712));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "2",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 11, 2, 5, 25, 841, DateTimeKind.Utc).AddTicks(2712));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "3",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 11, 4, 5, 25, 841, DateTimeKind.Utc).AddTicks(2712));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "4",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 11, 6, 5, 25, 841, DateTimeKind.Utc).AddTicks(2712));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: "5",
                column: "PublishedAt",
                value: new DateTime(2026, 3, 11, 8, 5, 25, 841, DateTimeKind.Utc).AddTicks(2712));
        }
    }
}
