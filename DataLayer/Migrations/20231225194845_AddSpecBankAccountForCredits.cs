using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecBankAccountForCredits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Credits",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "BankAccounts",
                columns: new[] { "Id", "AccountId", "AccountType", "Balance", "DateCreate", "IBAN", "IsFrozen" },
                values: new object[] { 20, 40, "Settlement", 0.0m, new DateTime(2023, 12, 25, 22, 48, 44, 975, DateTimeKind.Local).AddTicks(1842), "1111111111111111111111111111", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Credits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
