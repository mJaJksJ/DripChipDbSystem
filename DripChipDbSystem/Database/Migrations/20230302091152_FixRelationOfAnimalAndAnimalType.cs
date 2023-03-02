using Microsoft.EntityFrameworkCore.Migrations;

namespace DripChipDbSystem.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationOfAnimalAndAnimalType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_animal_type_animal_animal_id",
                table: "animal_type");

            migrationBuilder.DropIndex(
                name: "IX_animal_type_animal_id",
                table: "animal_type");

            migrationBuilder.DropColumn(
                name: "animal_id",
                table: "animal_type");

            migrationBuilder.CreateTable(
                name: "AnimalAnimalType",
                columns: table => new
                {
                    animaltypesid = table.Column<long>(name: "animal_types_id", type: "bigint", nullable: false),
                    relatedanimalsid = table.Column<long>(name: "related_animals_id", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalAnimalType", x => new { x.animaltypesid, x.relatedanimalsid });
                    table.ForeignKey(
                        name: "FK_AnimalAnimalType_animal_related_animals_id",
                        column: x => x.relatedanimalsid,
                        principalTable: "animal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalAnimalType_animal_type_animal_types_id",
                        column: x => x.animaltypesid,
                        principalTable: "animal_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAnimalType_related_animals_id",
                table: "AnimalAnimalType",
                column: "related_animals_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalAnimalType");

            migrationBuilder.AddColumn<long>(
                name: "animal_id",
                table: "animal_type",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_animal_type_animal_id",
                table: "animal_type",
                column: "animal_id");

            migrationBuilder.AddForeignKey(
                name: "FK_animal_type_animal_animal_id",
                table: "animal_type",
                column: "animal_id",
                principalTable: "animal",
                principalColumn: "id");
        }
    }
}
