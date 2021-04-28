using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePrestige.Data.Migrations
{
    public partial class CreatePedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PedidoModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    UsuarioModelId = table.Column<int>(nullable: false),
                    Rastreio = table.Column<string>(nullable: true),
                    FormaDePagamento = table.Column<string>(nullable: true),
                    Parcelas = table.Column<string>(nullable: true),
                    Subtotal = table.Column<double>(nullable: false),
                    Desconto = table.Column<decimal>(nullable: false),
                    ValorTotal = table.Column<double>(nullable: false),
                    DataPedido = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoModel_UsuarioModel_UsuarioModelId",
                        column: x => x.UsuarioModelId,
                        principalTable: "UsuarioModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoProdutosModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoModelId = table.Column<int>(nullable: false),
                    ProdutoCorModelId = table.Column<int>(nullable: false),
                    Quantidade = table.Column<int>(nullable: false),
                    ValorUnitario = table.Column<double>(nullable: false),
                    ValorTotal = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoProdutosModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoProdutosModel_PedidoModel_PedidoModelId",
                        column: x => x.PedidoModelId,
                        principalTable: "PedidoModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoProdutosModel_ProdutosCorModel_ProdutoCorModelId",
                        column: x => x.ProdutoCorModelId,
                        principalTable: "ProdutosCorModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoModel_UsuarioModelId",
                table: "PedidoModel",
                column: "UsuarioModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProdutosModel_PedidoModelId",
                table: "PedidoProdutosModel",
                column: "PedidoModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProdutosModel_ProdutoCorModelId",
                table: "PedidoProdutosModel",
                column: "ProdutoCorModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoProdutosModel");

            migrationBuilder.DropTable(
                name: "PedidoModel");
        }
    }
}
