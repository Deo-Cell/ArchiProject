using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Archi.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseModelForModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Tacos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Tacos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tacos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tacos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Pizzas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Pizzas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Pizzas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Pizzas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tacos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Tacos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tacos");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Tacos");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Pizzas");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Pizzas");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Pizzas");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Pizzas");
        }
    }
}
