using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePrestige.Data.Migrations
{
    public partial class AddPaymentInfoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorizationCode",
                table: "PedidoModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gateway",
                table: "PedidoModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tid",
                table: "PedidoModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionStatus",
                table: "PedidoModel",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorizationCode",
                table: "PedidoModel");

            migrationBuilder.DropColumn(
                name: "Gateway",
                table: "PedidoModel");

            migrationBuilder.DropColumn(
                name: "Tid",
                table: "PedidoModel");

            migrationBuilder.DropColumn(
                name: "TransactionStatus",
                table: "PedidoModel");
        }
    }
}
