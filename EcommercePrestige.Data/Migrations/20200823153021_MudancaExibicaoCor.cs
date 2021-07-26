using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePrestige.Data.Migrations
{
    public partial class MudancaExibicaoCor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "ProdutosCorModel");

            migrationBuilder.DropColumn(
                name: "Rgb",
                table: "ProdutosCorModel");

            migrationBuilder.AddColumn<string>(
                name: "CodigoInterno",
                table: "ProdutosCorModel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CorModelId",
                table: "ProdutosCorModel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CorModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImgUrl = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosCorModel_CorModelId",
                table: "ProdutosCorModel",
                column: "CorModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosCorModel_CorModel_CorModelId",
                table: "ProdutosCorModel",
                column: "CorModelId",
                principalTable: "CorModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosCorModel_CorModel_CorModelId",
                table: "ProdutosCorModel");

            migrationBuilder.DropTable(
                name: "CorModel");

            migrationBuilder.DropIndex(
                name: "IX_ProdutosCorModel_CorModelId",
                table: "ProdutosCorModel");

            migrationBuilder.DropColumn(
                name: "CodigoInterno",
                table: "ProdutosCorModel");

            migrationBuilder.DropColumn(
                name: "CorModelId",
                table: "ProdutosCorModel");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "ProdutosCorModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rgb",
                table: "ProdutosCorModel",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
