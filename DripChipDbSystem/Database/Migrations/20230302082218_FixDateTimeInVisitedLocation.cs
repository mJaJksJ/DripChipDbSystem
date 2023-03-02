using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DripChipDbSystem.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixDateTimeInVisitedLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "visited_date_time",
                table: "animal_visited_location",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "visited_date_time",
                table: "animal_visited_location",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");
        }
    }
}
