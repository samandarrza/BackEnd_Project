using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quarter.Migrations
{
    public partial class ChangeAminityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousesAmenities_Amenities_AmenityId",
                table: "HousesAmenities");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.CreateTable(
                name: "Aminities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aminities", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_HousesAmenities_Aminities_AmenityId",
                table: "HousesAmenities",
                column: "AmenityId",
                principalTable: "Aminities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousesAmenities_Aminities_AmenityId",
                table: "HousesAmenities");

            migrationBuilder.DropTable(
                name: "Aminities");

            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_HousesAmenities_Amenities_AmenityId",
                table: "HousesAmenities",
                column: "AmenityId",
                principalTable: "Amenities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
