using Microsoft.EntityFrameworkCore.Migrations;

namespace WalletTransaction.Migrations
{
    public partial class Changedwallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PinSalt",
                table: "Wallets",
                newName: "PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "PinHash",
                table: "Wallets",
                newName: "PasswordHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Wallets",
                newName: "PinSalt");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Wallets",
                newName: "PinHash");
        }
    }
}
