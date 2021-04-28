using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePrestige.Data.Migrations
{
    public partial class AddServicoEnvio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServicoEnvio",
                table: "PedidoModel",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServicoEnvio",
                table: "PedidoModel");
        }
    }
}
