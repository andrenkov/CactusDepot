using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CactusDepot.Common.DataContext.Migrations
{
    public partial class InitialMigrationSeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CactusSeeds",
                columns: table => new
                {
                    SeedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SeedName = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Parent1CatalogNum = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Parent2CatalogNum = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    SeedNote = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    SeedCollectedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SeedSource = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    SeedSeedsQty = table.Column<int>(type: "int", nullable: false),
                    SeedYear = table.Column<int>(type: "int", nullable: true),
                    SeedCatalogNum = table.Column<int>(type: "int", nullable: true),
                    SeedLastSowedYear = table.Column<int>(type: "int", nullable: true),
                    SeedAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SeedRating = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CactusSeeds", x => x.SeedId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CactusSeeds");
        }
    }
}
