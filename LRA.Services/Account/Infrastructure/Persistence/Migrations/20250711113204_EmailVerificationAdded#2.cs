using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LRA.Account.Infrasturcture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EmailVerificationAdded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmailVerification");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "EmailVerification",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "EmailVerification");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "EmailVerification",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
