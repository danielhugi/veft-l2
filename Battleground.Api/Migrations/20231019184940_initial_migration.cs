using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Battleground.Api.Migrations
{
    /// <inheritdoc />
    public partial class initial_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Battles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WinnerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Battles_Players_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerInventories",
                columns: table => new
                {
                    PokemonIdentifier = table.Column<string>(type: "text", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    AcquiredDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerInventories", x => new { x.PokemonIdentifier, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_PlayerInventories_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BattlePlayers",
                columns: table => new
                {
                    BattleId = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattlePlayers", x => new { x.BattleId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_BattlePlayers_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BattlePlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BattlePokemons",
                columns: table => new
                {
                    BattleId = table.Column<int>(type: "integer", nullable: false),
                    PokemonIdentifier = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattlePokemons", x => new { x.BattleId, x.PokemonIdentifier });
                    table.ForeignKey(
                        name: "FK_BattlePokemons_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    CriticalHit = table.Column<bool>(type: "boolean", nullable: false),
                    Damage = table.Column<int>(type: "integer", nullable: false),
                    BattlePokemonId = table.Column<int>(type: "integer", nullable: false),
                    BattlePokemonBattleId = table.Column<int>(type: "integer", nullable: false),
                    BattlePokemonPokemonIdentifier = table.Column<string>(type: "text", nullable: false),
                    PokemonIdentifier = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attacks_BattlePokemons_BattlePokemonBattleId_BattlePokemonP~",
                        columns: x => new { x.BattlePokemonBattleId, x.BattlePokemonPokemonIdentifier },
                        principalTable: "BattlePokemons",
                        principalColumns: new[] { "BattleId", "PokemonIdentifier" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attacks_BattlePokemonBattleId_BattlePokemonPokemonIdentifier",
                table: "Attacks",
                columns: new[] { "BattlePokemonBattleId", "BattlePokemonPokemonIdentifier" });

            migrationBuilder.CreateIndex(
                name: "IX_BattlePlayers_PlayerId",
                table: "BattlePlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Battles_WinnerId",
                table: "Battles",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerInventories_PlayerId",
                table: "PlayerInventories",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attacks");

            migrationBuilder.DropTable(
                name: "BattlePlayers");

            migrationBuilder.DropTable(
                name: "PlayerInventories");

            migrationBuilder.DropTable(
                name: "BattlePokemons");

            migrationBuilder.DropTable(
                name: "Battles");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
