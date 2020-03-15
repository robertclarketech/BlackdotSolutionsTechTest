using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlackdotTechTest.WebApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchEngineQueries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateEdited = table.Column<DateTime>(nullable: true),
                    Query = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchEngineQueries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SearchEngineQueryResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateEdited = table.Column<DateTime>(nullable: true),
                    ResultText = table.Column<string>(nullable: false),
                    ResultLink = table.Column<string>(nullable: false),
                    SearchEngineType = table.Column<string>(nullable: false),
                    SearchEngineQueryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchEngineQueryResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchEngineQueryResults_SearchEngineQueries_SearchEngineQueryId",
                        column: x => x.SearchEngineQueryId,
                        principalTable: "SearchEngineQueries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchEngineQueryResults_SearchEngineQueryId",
                table: "SearchEngineQueryResults",
                column: "SearchEngineQueryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchEngineQueryResults");

            migrationBuilder.DropTable(
                name: "SearchEngineQueries");
        }
    }
}
