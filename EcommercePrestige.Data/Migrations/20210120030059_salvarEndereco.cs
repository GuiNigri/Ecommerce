using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePrestige.Data.Migrations
{
    public partial class salvarEndereco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "PedidoModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "PedidoModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "PedidoModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "PedidoModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "PedidoModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Frete",
                table: "PedidoModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "PedidoModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rua",
                table: "PedidoModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "PedidoModel");

            migrationBuilder.DropColumn(
                name: "Cep",
                table: "PedidoModel");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "PedidoModel");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "PedidoModel");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "PedidoModel");

            migrationBuilder.DropColumn(
                name: "Frete",
                table: "PedidoModel");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "PedidoModel");

            migrationBuilder.DropColumn(
                name: "Rua",
                table: "PedidoModel");
        }
    }
}
