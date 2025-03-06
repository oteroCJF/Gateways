using Api.Gateway.WebClient.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Api.Gateway.WebClient.Config.Comedor;
using Api.Gateway.WebClient.Config.Mensajeria;
using Api.Gateway.WebClient.Config.Transporte;
using Api.Gateway.WebClient.Config.Microbiologicos;
using Api.Gateway.WebClient.Config.Convencional;
using Api.Gateway.WebClient.Config.Celular;
using Api.Gateway.WebClient.Config.Agua;
using Api.Gateway.WebClient.Config.BMuebles;

namespace Api.Gateway.WebClient
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
            services.AddHttpContextAccessor();

            services.AddAppsettingBinding(Configuration).AddProxiesGenerales(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesCatalogos(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesEstatus(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesAEElectrica(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesLimpieza(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesMensajeriaQueries(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesMensajeriaCommands(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesFumigacion(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesBMuebles(Configuration);

            services.AddAppsettingBinding(Configuration).AddProxiesComedorQueries(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesComedorCommands(Configuration);
                
            services.AddAppsettingBinding(Configuration).AddProxiesTransporteQueries(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesTransporteCommands(Configuration);
            
            services.AddAppsettingBinding(Configuration).AddProxiesMicrobiologicosQueries(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesMicrobiologicosCommands(Configuration);
            
            services.AddAppsettingBinding(Configuration).AddProxiesConvencionalQueries(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesConvencionalCommands(Configuration);
            
            services.AddAppsettingBinding(Configuration).AddProxiesCelularQueries(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesCelularCommands(Configuration);
            
            services.AddAppsettingBinding(Configuration).AddProxiesAguaQueries(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesAguaCommands(Configuration);

            services.AddAppsettingBinding(Configuration).AddProxiesBMueblesQueries(Configuration);
            services.AddAppsettingBinding(Configuration).AddProxiesBMueblesCommands(Configuration);



            services.AddControllers().AddJsonOptions(options => { 
                options.JsonSerializerOptions.IgnoreNullValues = true; 
                options.JsonSerializerOptions.PropertyNamingPolicy = null; 
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                //options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddControllers();

            var secretKey = Encoding.ASCII.GetBytes(
                Configuration.GetValue<string>("SecretKey")
            );

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
