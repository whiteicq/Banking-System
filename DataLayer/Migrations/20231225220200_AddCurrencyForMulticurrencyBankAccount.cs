using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyForMulticurrencyBankAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "BankAccounts",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Currency", "DateCreate" },
                values: new object[] { "Dollar", new DateTime(2023, 12, 26, 1, 2, 0, 445, DateTimeKind.Local).AddTicks(9748) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "BankAccounts");

            migrationBuilder.UpdateData(
                table: "BankAccounts",
                keyColumn: "Id",
                keyValue: 20,
                column: "DateCreate",
                value: new DateTime(2023, 12, 25, 22, 48, 44, 975, DateTimeKind.Local).AddTicks(1842));
        }
    }
}
