using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VirtualDungeonMaster.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdventureSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CurrentTurnNumber = table.Column<int>(type: "integer", nullable: false),
                    CurrentPrompt = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdventureSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Class = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Race = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Background = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NarrativeEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TurnNumber = table.Column<int>(type: "integer", nullable: false),
                    PlayerInput = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    AIResponse = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AdventureSessionEntityId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NarrativeEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NarrativeEvents_AdventureSessions_AdventureSessionEntityId",
                        column: x => x.AdventureSessionEntityId,
                        principalTable: "AdventureSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NarrativeEvents_AdventureSessionEntityId",
                table: "NarrativeEvents",
                column: "AdventureSessionEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "NarrativeEvents");

            migrationBuilder.DropTable(
                name: "AdventureSessions");
        }
    }
}
