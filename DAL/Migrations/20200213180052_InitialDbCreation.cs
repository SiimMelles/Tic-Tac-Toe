﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    GameStateId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    BoardString = table.Column<string>(nullable: true),
                    BoardWidth = table.Column<int>(nullable: false),
                    PlayerOneMove = table.Column<bool>(nullable: false),
                    MoveCount = table.Column<int>(nullable: false),
                    GameLost = table.Column<bool>(nullable: false),
                    GameWon = table.Column<bool>(nullable: false),
                    GameTied = table.Column<bool>(nullable: false),
                    VsAI = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.GameStateId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameStates");
        }
    }
}
