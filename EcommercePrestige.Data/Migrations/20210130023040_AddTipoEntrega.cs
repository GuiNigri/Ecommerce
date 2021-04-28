using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePrestige.Data.Migrations
{
    public partial class AddTipoEntrega : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoDeEnvio",
                table: "PedidoModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoDeEnvio",
                table: "PedidoModel");
        }
    }
}
