using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePrestige.Data.Migrations
{
    public partial class AddAviseMe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AviseMeModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    ProdutoCorModelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AviseMeModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AviseMeModel_ProdutosCorModel_ProdutoCorModelId",
                        column: x => x.ProdutoCorModelId,
                        principalTable: "ProdutosCorModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AviseMeModel_ProdutoCorModelId",
                table: "AviseMeModel",
                column: "ProdutoCorModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AviseMeModel");
        }
    }
}
