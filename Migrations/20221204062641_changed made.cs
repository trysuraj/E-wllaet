using Microsoft.EntityFrameworkCore.Migrations;

namespace WalletTransaction.Migrations
{
    public partial class changedmade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionParticulars",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionParticulars",
                table: "Transactions");
        }
    }
}
