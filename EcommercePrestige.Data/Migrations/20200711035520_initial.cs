using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePrestige.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmpresaModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    Cnpj = table.Column<string>(nullable: true),
                    RazaoSocial = table.Column<string>(nullable: true),
                    Cnae = table.Column<string>(nullable: true),
                    Cep = table.Column<string>(nullable: true),
                    Logradouro = table.Column<string>(nullable: true),
                    Numero = table.Column<int>(nullable: false),
                    Complemento = table.Column<string>(nullable: true),
                    Bairro = table.Column<string>(nullable: true),
                    Municipio = table.Column<string>(nullable: true),
                    Uf = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    NomeOtica = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpresaModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KitsModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    ValorVenda = table.Column<double>(nullable: false),
                    Descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitsModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarcaModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarcaModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Material = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuporteModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "100000, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    DocumentoUsuario = table.Column<string>(nullable: true),
                    Cpf = table.Column<bool>(nullable: false),
                    Cnpj = table.Column<bool>(nullable: false),
                    Assunto = table.Column<string>(nullable: true),
                    Mensagem = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuporteModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    NomeCompleto = table.Column<string>(nullable: true),
                    BloqueioAutomatico = table.Column<bool>(nullable: false),
                    BloqueioManual = table.Column<bool>(nullable: false),
                    Administrador = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProdutosModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarcaModelId = table.Column<int>(nullable: false),
                    MaterialModelId = table.Column<int>(nullable: false),
                    Referencia = table.Column<string>(nullable: true),
                    Tamanho = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    ValorVenda = table.Column<double>(nullable: false),
                    StatusProduto = table.Column<string>(nullable: true),
                    Genero = table.Column<string>(nullable: true),
                    BestSeller = table.Column<bool>(nullable: false),
                    Liquidacao = table.Column<bool>(nullable: false),
                    Vitrine = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutosModel_MarcaModel_MarcaModelId",
                        column: x => x.MarcaModelId,
                        principalTable: "MarcaModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutosModel_MaterialModel_MaterialModelId",
                        column: x => x.MaterialModelId,
                        principalTable: "MaterialModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutosCorModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: true),
                    Rgb = table.Column<string>(nullable: true),
                    Estoque = table.Column<int>(nullable: false),
                    ProdutoModelId = table.Column<int>(nullable: false),
                    PedidoGold = table.Column<bool>(nullable: false),
                    PedidoSilver = table.Column<bool>(nullable: false),
                    PedidoBasic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosCorModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutosCorModel_ProdutosModel_ProdutoModelId",
                        column: x => x.ProdutoModelId,
                        principalTable: "ProdutosModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProdutosFotoModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UriBlob = table.Column<string>(nullable: true),
                    Principal = table.Column<bool>(nullable: false),
                    ProdutoModelId = table.Column<int>(nullable: false),
                    ProdutoCorModelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosFotoModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutosFotoModel_ProdutosCorModel_ProdutoCorModelId",
                        column: x => x.ProdutoCorModelId,
                        principalTable: "ProdutosCorModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProdutosFotoModel_ProdutosModel_ProdutoModelId",
                        column: x => x.ProdutoModelId,
                        principalTable: "ProdutosModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosCorModel_ProdutoModelId",
                table: "ProdutosCorModel",
                column: "ProdutoModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosFotoModel_ProdutoCorModelId",
                table: "ProdutosFotoModel",
                column: "ProdutoCorModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosFotoModel_ProdutoModelId",
                table: "ProdutosFotoModel",
                column: "ProdutoModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosModel_MarcaModelId",
                table: "ProdutosModel",
                column: "MarcaModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosModel_MaterialModelId",
                table: "ProdutosModel",
                column: "MaterialModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpresaModel");

            migrationBuilder.DropTable(
                name: "KitsModel");

            migrationBuilder.DropTable(
                name: "ProdutosFotoModel");

            migrationBuilder.DropTable(
                name: "SuporteModel");

            migrationBuilder.DropTable(
                name: "UsuarioModel");

            migrationBuilder.DropTable(
                name: "ProdutosCorModel");

            migrationBuilder.DropTable(
                name: "ProdutosModel");

            migrationBuilder.DropTable(
                name: "MarcaModel");

            migrationBuilder.DropTable(
                name: "MaterialModel");
        }
    }
}
