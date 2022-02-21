using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommercePrestige.Data.Migrations
{
    public partial class AdicionaCodigoBarras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoBarras",
                table: "ProdutosCorModel",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoBarras",
                table: "ProdutosCorModel");
        }
    }
}
