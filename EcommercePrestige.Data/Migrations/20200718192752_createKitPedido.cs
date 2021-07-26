using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePrestige.Data.Migrations
{
    public partial class createKitPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PedidoKitModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoModelId = table.Column<int>(nullable: false),
                    KitModelId = table.Column<int>(nullable: false),
                    Quantidade = table.Column<int>(nullable: false),
                    ValorUnitario = table.Column<double>(nullable: false),
                    ValorTotal = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoKitModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoKitModel_KitsModel_KitModelId",
                        column: x => x.KitModelId,
                        principalTable: "KitsModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoKitModel_PedidoModel_PedidoModelId",
                        column: x => x.PedidoModelId,
                        principalTable: "PedidoModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoKitModel_KitModelId",
                table: "PedidoKitModel",
                column: "KitModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoKitModel_PedidoModelId",
                table: "PedidoKitModel",
                column: "PedidoModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoKitModel");
        }
    }
}
