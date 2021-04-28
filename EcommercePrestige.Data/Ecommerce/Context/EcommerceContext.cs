using EcommercePrestige.Model.Entity;
using Microsoft.EntityFrameworkCore;


namespace EcommercePrestige.Data.Ecommerce.Context
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext (DbContextOptions<EcommerceContext> options)
            : base(options)
        {
        }

        public DbSet<EmpresaModel> EmpresaModel { get; set; }
        public DbSet<UsuarioModel> UsuarioModel { get; set; }
        public DbSet<MarcaModel> MarcaModel { get; set; }
        public DbSet<MaterialModel> MaterialModel { get; set; }
        public DbSet<ProdutoModel> ProdutosModel { get; set; }
        public DbSet<ProdutoFotoModel> ProdutosFotoModel { get; set; }
        public DbSet<ProdutoCorModel> ProdutosCorModel { get; set; }
        public DbSet<KitModel> KitsModel { get; set; }
        public DbSet<PedidoModel> PedidoModel { get; set; }
        public DbSet<PedidoProdutosModel> PedidoProdutosModel { get; set; }
        public DbSet<PedidoKitModel> PedidoKitModel { get; set; }
        public DbSet<TextoHomeModel> TextoHomeModel { get; set; }
        public DbSet<BannersHomeModel> BannersHomeModel { get; set; }
        public DbSet<AviseMeModel> AviseMeModel { get; set; }

    }
}
