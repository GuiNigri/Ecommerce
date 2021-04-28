using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePrestige.Data.Migrations
{
    public partial class AdicioneiStatusAtivacaoProduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusAtivacao",
                table: "ProdutosModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusAtivacao",
                table: "ProdutosFotoModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusAtivacao",
                table: "ProdutosCorModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusAtivacao",
                table: "ProdutosModel");

            migrationBuilder.DropColumn(
                name: "StatusAtivacao",
                table: "ProdutosFotoModel");

            migrationBuilder.DropColumn(
                name: "StatusAtivacao",
                table: "ProdutosCorModel");
        }
    }
}
