using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrontAPI.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RestaurantName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    Cuisine = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name",
                table: "Locations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_StateId",
                table: "Locations",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_StreetAddress",
                table: "Locations",
                column: "StreetAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ZipCode",
                table: "Locations",
                column: "ZipCode");

            migrationBuilder.CreateIndex(
                name: "IX_States_Cuisine",
                table: "States",
                column: "Cuisine");

            migrationBuilder.CreateIndex(
                name: "IX_States_Name",
                table: "States",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_States_PhoneNumber",
                table: "States",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_States_RestaurantName",
                table: "States",
                column: "RestaurantName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
