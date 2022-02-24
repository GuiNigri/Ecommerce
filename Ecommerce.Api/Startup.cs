using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Ecommerce.Api.Authentication;
using Ecommerce.Api.Helpers;
using EcommercePrestige.IoC;
using EcommercePrestige.Model.Interfaces.Authentication;
using EcommercePrestige.Model.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Ecommerce.Api.Controllers.Produto.AutoMapper;
using EcommercePrestige.Data.Repository;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Services;
using Microsoft.EntityFrameworkCore;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.EmailApi;
using EcommercePrestige.ConsultaReceitaApi;
using EcommercePrestige.Model.Interfaces.Infrastructure;
using EcommercePrestige.CorreiosApi;
using EcommercePrestige.Model.Interfaces.UoW;
using EcommercePrestige.Data.UoW;
using EcommercePrestige.CieloApiWebServices;
using EcommercePrestige.StoneCheckoutApi;
using EcommercePrestige.Model.Entity;

namespace Ecommerce.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));

            services.RegisterIdentityForWebApi(Configuration);

            services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwt => {
                    var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true, 
                        IssuerSigningKey = new SymmetricSecurityKey(key), 
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };
                });

            services.AddAuthorization(
                options => options.AddPolicy("Admin", policy => policy.RequireClaim("AdminClaim")));

            services.AddScoped<IUserAuthentication, UserAuthentication>();
            services.AddScoped<TokenProvider>();
            services.AddScoped<IProdutoCorServices, ProdutoCorServices>();
            services.AddScoped<IProdutoCorRepository, ProdutoCorRepository>();
            services.AddScoped<IEmailSenderServices, AuthMessageSender>();
            services.AddScoped<IAviseMeServices, AviseMeServices>();
            services.AddScoped<IAviseMeRepository, AviseMeRepository>();
            services.AddScoped<IEmpresaServices, EmpresaServices>();
            services.AddScoped<IEmpresaRepository, EmpresaRepository>();
            services.AddScoped<IConsultaCnpjAwsApi, ConsultaReceitaAwsApi>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioServices, UsuarioServices>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<ISuporteServices, SuporteServices>();
            services.AddScoped<ISuporteRepository, SuporteRepository>();
            services.AddScoped<IKitsServices, KitServices>();
            services.AddScoped<IKitsRepository, KitRepository>();
            services.AddScoped<ICorreiosInfrastructure, CorreioInfrastructure>();
            services.AddScoped<Correios.NET.Services>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICieloCheckout, CieloCheckout>();
            services.AddScoped<IPagarMeCheckout, PagarMeCheckout>();

            services.Configure<PagarMeSettingsModel>(Configuration.GetSection("PagarMeSettings"));
            services.Configure<EmailSettingsModel>(Configuration.GetSection("EmailSettings"));

            services.AddDbContext<EcommerceContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("EcommerceContext")));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddHttpClient();
            //services.RegisterInjections(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}