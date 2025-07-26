using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LRA.Account.Infrasturcture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TwoFaAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "EmailVerification",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsTwoFaEnabled",
                table: "Accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "EmailVerification");

            migrationBuilder.DropColumn(
                name: "IsTwoFaEnabled",
                table: "Accounts");
        }
    }
}
