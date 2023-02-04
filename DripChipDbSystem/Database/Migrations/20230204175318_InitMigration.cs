using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DripChipDbSystem.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(name: "first_name", type: "text", nullable: false),
                    lastname = table.Column<string>(name: "last_name", type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    passwordhash = table.Column<string>(name: "password_hash", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "location_point",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location_point", x => x.id);
                    table.CheckConstraint("CK_Latitude", "Latitude >= -90 AND Latitude <= 90");
                    table.CheckConstraint("CK_Longitude", "Latitude >= -180 AND Latitude <= 180");
                });

            migrationBuilder.CreateTable(
                name: "animal",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    weight = table.Column<float>(type: "real", nullable: false),
                    length = table.Column<float>(type: "real", nullable: false),
                    height = table.Column<float>(type: "real", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    lifestatus = table.Column<int>(name: "life_status", type: "integer", nullable: false, defaultValue: 0),
                    chippingdatetime = table.Column<DateTime>(name: "chipping_date_time", type: "timestamp without time zone", nullable: false),
                    chipperid = table.Column<int>(name: "chipper_id", type: "integer", nullable: false),
                    chippinglocationpointid = table.Column<long>(name: "chipping_location_point_id", type: "bigint", nullable: false),
                    deathdatetime = table.Column<DateTime>(name: "death_date_time", type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_animal", x => x.id);
                    table.ForeignKey(
                        name: "FK_animal_account_chipper_id",
                        column: x => x.chipperid,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_animal_location_point_chipping_location_point_id",
                        column: x => x.chippinglocationpointid,
                        principalTable: "location_point",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "animal_type",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: false),
                    animalid = table.Column<long>(name: "animal_id", type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_animal_type", x => x.id);
                    table.ForeignKey(
                        name: "FK_animal_type_animal_animal_id",
                        column: x => x.animalid,
                        principalTable: "animal",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "animal_visited_location",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    animalid = table.Column<long>(name: "animal_id", type: "bigint", nullable: false),
                    locationpointid = table.Column<long>(name: "location_point_id", type: "bigint", nullable: false),
                    visiteddatetime = table.Column<DateTime>(name: "visited_date_time", type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_animal_visited_location", x => x.id);
                    table.ForeignKey(
                        name: "FK_animal_visited_location_animal_animal_id",
                        column: x => x.animalid,
                        principalTable: "animal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_animal_visited_location_location_point_location_point_id",
                        column: x => x.locationpointid,
                        principalTable: "location_point",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_animal_chipper_id",
                table: "animal",
                column: "chipper_id");

            migrationBuilder.CreateIndex(
                name: "IX_animal_chipping_location_point_id",
                table: "animal",
                column: "chipping_location_point_id");

            migrationBuilder.CreateIndex(
                name: "IX_animal_type_animal_id",
                table: "animal_type",
                column: "animal_id");

            migrationBuilder.CreateIndex(
                name: "IX_animal_visited_location_animal_id",
                table: "animal_visited_location",
                column: "animal_id");

            migrationBuilder.CreateIndex(
                name: "IX_animal_visited_location_location_point_id",
                table: "animal_visited_location",
                column: "location_point_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "animal_type");

            migrationBuilder.DropTable(
                name: "animal_visited_location");

            migrationBuilder.DropTable(
                name: "animal");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "location_point");
        }
    }
}
