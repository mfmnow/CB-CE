using Mfm.Bc.CurrencyExchanger.Data.Contracts;
using Mfm.Bc.CurrencyExchanger.Data.Entities;
using Mfm.Bc.CurrencyExchanger.Domain.Models.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mfm.Bc.CurrencyExchanger.Data.Services
{
    public class FrankfurterAppExchangeRatesDataAccessService : IExchangeRatesDataAccessService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FrankfurterAppExchangeRatesDataAccessService> _logger;
        private readonly IExchangeRatesCacheService _exchangeRatesCacheService;
        private readonly static SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public FrankfurterAppExchangeRatesDataAccessService(HttpClient httpClient, 
            IExchangeRatesCacheService exchangeRatesCacheService,
            ILogger<FrankfurterAppExchangeRatesDataAccessService> logger) 
        {
            _httpClient = httpClient;
            _exchangeRatesCacheService = exchangeRatesCacheService;
            _logger = logger;
        }

        public virtual async Task<HttpResponseMessage> ExecuteGetRequestWithRetry(string endpoint, CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            int maxRetries = 3;
            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    response = await _httpClient.GetAsync(endpoint, cancellationToken);
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new NotFoundException($"URL: {endpoint} not found");
                    }
                    response.EnsureSuccessStatusCode();
                    return response;
                }
                catch (NotFoundException)
                {
                    throw;
                }
                catch
                {
                    response = null;
                    retries++;
                }
            }
            return null;
        }

        public async Task<ExchangeRatesEntity> GetLatestExchangeRatesEntity(string baseCurrency, CancellationToken cancellationToken)
        {
            ExchangeRatesEntity exchangeRatesEntity;
            _lock.Wait(cancellationToken);
            try
            {
                var endpoint = $"latest?from={baseCurrency}";
                var response = await ExecuteGetRequestWithRetry(endpoint, cancellationToken);
                exchangeRatesEntity = await response.Content.ReadFromJsonAsync<ExchangeRatesEntity>();
                _logger.LogDebug($"nameof(GetLatestExchangeRatesEntity) endpoint{endpoint} {exchangeRatesEntity}", endpoint, exchangeRatesEntity);
            }
            finally
            {
                _lock.Release();
            }
            return exchangeRatesEntity;
        }

        public async Task<ExchangeRatesEntity> ConvertCurrencyAmount(string baseCurrency, string toCurrency, double amount, CancellationToken cancellationToken)
        {
            ExchangeRatesEntity exchangeRatesEntity;
            _lock.Wait(cancellationToken);
            try
            {
                var endpoint = $"latest?amount={amount}&from={baseCurrency}&to={toCurrency}";
                var response = await ExecuteGetRequestWithRetry(endpoint, cancellationToken);
                exchangeRatesEntity = await response.Content.ReadFromJsonAsync<ExchangeRatesEntity>();
                _logger.LogDebug($"nameof(GetLatestExchangeRatesEntity) endpoint{endpoint} {exchangeRatesEntity}", endpoint, exchangeRatesEntity);
            }
            finally
            {
                _lock.Release();
            }
            return exchangeRatesEntity;
        }

        public async Task<HistoricalExchangeRatesRangeEntity> GetHistoricalExchangeRatesRangeEntity(string baseCurrency, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            HistoricalExchangeRatesRangeEntity historicalExchangeRatesEntity;

            //Attempt to read page from Cache
            var cacheKey = $"{startDate.ToString("yyyy-MM-dd")}..{endDate.ToString("yyyy-MM-dd")}?from={baseCurrency}";
            historicalExchangeRatesEntity = _exchangeRatesCacheService.GetCachedObject<HistoricalExchangeRatesRangeEntity>(cacheKey);
            if (historicalExchangeRatesEntity != null)
            {
                return historicalExchangeRatesEntity;
            }
            _lock.Wait(cancellationToken);
            try
            {
                var endpoint = cacheKey;
                var response = await ExecuteGetRequestWithRetry(endpoint, cancellationToken);
                historicalExchangeRatesEntity = await response.Content.ReadFromJsonAsync<HistoricalExchangeRatesRangeEntity>();
                
                //Save page to Cache
                _exchangeRatesCacheService.CreateCacheObject(cacheKey, historicalExchangeRatesEntity);
                _logger.LogDebug($"nameof(GetLatestExchangeRatesEntity) endpoint{endpoint} {historicalExchangeRatesEntity}", endpoint, historicalExchangeRatesEntity);
            }
            finally
            {
                _lock.Release();
            }
            return historicalExchangeRatesEntity;
        }
    }
}
