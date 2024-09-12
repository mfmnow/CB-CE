using Mfm.Bc.CurrencyExchanger.Data.Contracts;
using Mfm.Bc.CurrencyExchanger.Data.Services;
using Mfm.Bc.CurrencyExchanger.Domain.Contracts;
using Mfm.Bc.CurrencyExchanger.Domain.Models.Config;
using Mfm.Bc.CurrencyExchanger.Domain.Services;
using Microsoft.Extensions.Options;

namespace Mfm.Bc.CurrencyExchanger.WebApi.App_Code.ServiceExtensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class CurrencyExchangerServiceCollectionExtensions
    {
        public static IServiceCollection AddFrankfurterServices(this IServiceCollection services, Action<FrankfurterAppConfig> frankfurterAppConfigOptions)
        {
            services.Configure(frankfurterAppConfigOptions);

            services.AddHttpClient<IExchangeRatesDataAccessService, FrankfurterAppExchangeRatesDataAccessService>((provider, httpClient) =>
            {
                var frankfurterAppConfig = provider.GetRequiredService<IOptions<FrankfurterAppConfig>>().Value;
                httpClient.BaseAddress = new Uri(frankfurterAppConfig.BaseUrl);
            });

            return services;
        }

        public static IServiceCollection RegisterDomainServices(this IServiceCollection services)
        {
            services.AddSingleton<IExchangeRatesDomainService, ExchangeRatesDomainService>();
            services.AddSingleton<IExchangeRatesCacheService, ExchangeRatesCacheService>();
            return services;
        }

        public static IServiceCollection RegisterConfigs(this IServiceCollection services, IConfiguration configuration)
        {
#pragma warning disable CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
            services.AddSingleton(configuration.GetSection("CurrencyExchangerConfig").Get<CurrencyExchangerConfig>());
#pragma warning restore CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
            return services;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
