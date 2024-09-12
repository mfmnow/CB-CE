using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Mfm.Bc.CurrencyExchanger.WebApi.App_Code.ServiceExtensions
{
    /// <summary>
    /// Swagger Config Service
    /// </summary>
    public static class SwaggerConfigService
    {
        /// <summary>
        /// Configures Swagger for a .NET Core WebApi
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var version = configuration.GetValue<string>("Version");
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment?.ToLower() == "development")
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = Assembly.GetExecutingAssembly().GetName().Name, Version = version });
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                    c.CustomSchemaIds(type => type.ToString());
                });
            }
        }
    }
}
