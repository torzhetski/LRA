using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LRA.Account.Infrasturcture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "TestModels",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TestModels",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "TestModels");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TestModels");
        }
    }
}
