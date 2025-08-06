using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LRA.Account.Infrasturcture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class VerificatinonIsUsedAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "VerificationModels",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { new Guid("39c39491-7a96-4e2d-831e-d38def097117"), "Paramedic" },
                    { new Guid("7f2e843e-1ffb-4ef5-b13c-1eecf50df577"), "SuperAdmin" },
                    { new Guid("9d773595-f07e-4c44-a00e-b5bdc388ba11"), "Client" },
                    { new Guid("f496959f-8e1a-4a4f-b5d2-49f32c6e82af"), "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("39c39491-7a96-4e2d-831e-d38def097117"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7f2e843e-1ffb-4ef5-b13c-1eecf50df577"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("9d773595-f07e-4c44-a00e-b5bdc388ba11"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f496959f-8e1a-4a4f-b5d2-49f32c6e82af"));

            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "VerificationModels");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "SuperAdmin" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Admin" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Client" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Paramedic" }
                });
        }
    }
}
