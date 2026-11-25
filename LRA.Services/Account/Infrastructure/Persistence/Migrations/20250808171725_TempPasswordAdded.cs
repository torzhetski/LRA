using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LRA.Account.Infrasturcture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TempPasswordAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CredentialsId",
                table: "Accounts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CredentialsDetailsEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HasTempPassword = table.Column<bool>(type: "boolean", nullable: false),
                    IsPasswordUsed = table.Column<bool>(type: "boolean", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialsDetailsEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CredentialsId",
                table: "Accounts",
                column: "CredentialsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_CredentialsDetailsEntity_CredentialsId",
                table: "Accounts",
                column: "CredentialsId",
                principalTable: "CredentialsDetailsEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_CredentialsDetailsEntity_CredentialsId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "CredentialsDetailsEntity");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_CredentialsId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CredentialsId",
                table: "Accounts");
        }
    }
}
