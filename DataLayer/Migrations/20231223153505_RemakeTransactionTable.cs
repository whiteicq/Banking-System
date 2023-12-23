using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class RemakeTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientBankAccount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SenderBankAccount",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "TransactionType",
                table: "Transactions",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "BankAccounts",
                type: "nvarchar(28)",
                maxLength: 28,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "DateBirth", "Email", "HashPassword", "PhoneNumber", "Role", "UserName" },
                values: new object[] { 40, new DateTime(2002, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "manager@mail.ru", "5994471ABB01112AFCC18159F6CC74B4F511B99806DA59B3CAF5A9C173CACFC5", "+375290000000", "Manager", "manager" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "BankAccounts");

            migrationBuilder.AddColumn<int>(
                name: "RecipientBankAccount",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SenderBankAccount",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
