using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePrestige.Data.Migrations
{
    public partial class AlteracaoUsuarioModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Administrador",
                table: "UsuarioModel");

            migrationBuilder.AddColumn<bool>(
                name: "Verificado",
                table: "UsuarioModel",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Verificado",
                table: "UsuarioModel");

            migrationBuilder.AddColumn<bool>(
                name: "Administrador",
                table: "UsuarioModel",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
