using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.AppServices.Implementations;
using EcommercePrestige.Application.AutoMapper;
using EcommercePrestige.Blob;
using EcommercePrestige.CieloApiWebServices;
using EcommercePrestige.ConsultaReceitaApi;
using EcommercePrestige.CorreiosApi;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Data.Repository;
using EcommercePrestige.Data.UoW;
using EcommercePrestige.EmailApi;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Infrastructure;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;
using EcommercePrestige.Model.Interfaces.UoW;
using EcommercePrestige.Services;
using EcommercePrestige.StoneCheckoutApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcommercePrestige.IoC
{
    public static class NativeInjector
    {
        public static void RegisterInjections(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            AutoMapperConfig.RegisterMappings();

            services.AddScoped<IEmpresaServices, EmpresaServices>();
            services.AddScoped<IEmpresaAppServices, EmpresaAppServices>();

            services.AddScoped<IMarcaAppServices, MarcaAppServices>();
            services.AddScoped<IMarcaServices, MarcaServices>();

            services.AddScoped<IMaterialAppServices, MaterialAppServices>();
            services.AddScoped<IMaterialServices, MaterialServices>();

            services.AddScoped<IUsuarioAppServices, UsuarioAppServices>();
            services.AddScoped<IUsuarioServices, UsuarioServices>();

            services.AddScoped<IProdutoAppServices, ProdutoAppServices>();
            services.AddScoped<IProdutoServices, ProdutoServices>();

            services.AddScoped<IProdutoFotoAppServices, ProdutoFotoAppServices>();
            services.AddScoped<IProdutoFotoServices, ProdutoFotoServices>();

            services.AddScoped<IProdutoCorAppServices, ProdutoCorAppServices>();
            services.AddScoped<IProdutoCorServices, ProdutoCorServices>();

            services.AddScoped<ISuporteAppServices, SuporteAppServices>();
            services.AddScoped<ISuporteServices, SuporteServices>();

            services.AddScoped<IKitAppServices, KitAppServices>();
            services.AddScoped<IKitsServices, KitServices>();

            services.AddScoped<IPedidoAppServices, PedidoAppServices>();
            services.AddScoped<IPedidoService, PedidoService>();

            services.AddScoped<ICorAppServices, CorAppService>();
            services.AddScoped<ICorService, CorService>();

            services.AddScoped<IAviseMeServices, AviseMeServices>();

            services.AddScoped<ITextoHomeServices, TextoHomeServices>();

            services.AddScoped<IBannersHomeServices, BannersHomeServices>();

            services.AddScoped<IHomeAppServices, HomeAppServices>();

            services.AddScoped<IAdministracaoProdutosAppServices, AdministracaoProdutosAppServices>();

            services.AddScoped<ICarrinhoAppServices, CarrinhoAppServices>();


            services.AddScoped<IEmailSenderServices, AuthMessageSender>();

            services.AddScoped<IConsultaCnpjReceitaApi,ConsultaCnpjReceitaApi>();

            services.AddScoped<IConsultaCnpjAwsApi, ConsultaReceitaAwsApi>();

            services.AddScoped<IPackageAppServices, PackageAppServices>();
            services.AddScoped<ICorreiosServices, CorreioServices>();

            services.AddScoped<Correios.NET.Services>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICieloCheckout, CieloCheckout>();
            services.AddScoped<IPagarMeCheckout, PagarMeCheckout>();

            services.Configure<PagarMeSettingsModel>(configuration.GetSection("PagarMeSettings"));
            services.Configure<EmailSettingsModel>(configuration.GetSection("EmailSettings"));

            services.AddDbContext<EcommerceContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("EcommerceContext")));

            services.AddTransient<IBlobInfrastructure, BlobInfrastructure>(provider =>
                new BlobInfrastructure(configuration.GetConnectionString("StorageAccount")));

            services.AddScoped<IEmpresaRepository, EmpresaRepository>();
            services.AddScoped<IMarcaRepository, MarcaRepository>();
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoFotoRepository, ProdutoFotoRepository>();
            services.AddScoped<IProdutoCorRepository, ProdutoCorRepository>();
            services.AddScoped<ISuporteRepository, SuporteRepository>();
            services.AddScoped<IKitsRepository, KitRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<ICorRepository, CorRepository>();
            services.AddScoped<ICorreiosInfrastructure, CorreioInfrastructure>();
            services.AddScoped<ITextoHomeRepository, TextoHomeRepository>();
            services.AddScoped<IBannersHomeRepository, BannersHomeRepository>();
            services.AddScoped<IAviseMeRepository, AviseMeRepository>();

        }
    }
}
