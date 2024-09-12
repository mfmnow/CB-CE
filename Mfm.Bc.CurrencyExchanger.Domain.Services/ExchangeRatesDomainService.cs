using Mfm.Bc.CurrencyExchanger.Data.Contracts;
using Mfm.Bc.CurrencyExchanger.Domain.Contracts;
using Mfm.Bc.CurrencyExchanger.Domain.Models.Config;
using Mfm.Bc.CurrencyExchanger.Domain.Models.Exceptions;
using Mfm.Bc.CurrencyExchanger.Domain.Models.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mfm.Bc.CurrencyExchanger.Domain.Services
{
    /// <inheritdoc/>
    public class ExchangeRatesDomainService : IExchangeRatesDomainService
    {
        private readonly IExchangeRatesDataAccessService _exchangeRatesDataAccessService;
        private readonly CurrencyExchangerConfig _currencyExchangerConfig;

        public ExchangeRatesDomainService(CurrencyExchangerConfig currencyExchangerConfig, IExchangeRatesDataAccessService exchangeRatesDataAccessService)
        {
            _currencyExchangerConfig = currencyExchangerConfig;
            _exchangeRatesDataAccessService = exchangeRatesDataAccessService;
        }

        public virtual void CheckUnsupportedCurrencies(string currency)
        {
            if (_currencyExchangerConfig.UnsupportedConversionCurrencies.Any(c => c.ToLower() == currency.ToLower()))
            {
                throw new BusinessValidationException($"{currency} is not supported");
            }
        }

        public async Task<CurrencyExchangeRates> GetCurrencyExchangeRates(string currency, CancellationToken cancellationToken)
        {
            var latestExchangeRatesEntity =
                await _exchangeRatesDataAccessService.GetLatestExchangeRatesEntity(currency, cancellationToken);
            return new CurrencyExchangeRates { 
                Currency = latestExchangeRatesEntity.Base,
                CurrencyDate = latestExchangeRatesEntity.Date,
                Rates = latestExchangeRatesEntity.Rates
            };
        }

        public async Task<ConvertCurrencyAmountResult> ConvertCurrencyAmount(string fromCurrency, string toCurrency, double amount, CancellationToken cancellationToken)
        {
            CheckUnsupportedCurrencies(fromCurrency);
            CheckUnsupportedCurrencies(toCurrency);
            var exchangeRatesEntity =
                await _exchangeRatesDataAccessService.ConvertCurrencyAmount(fromCurrency, toCurrency, amount, cancellationToken);
            return new ConvertCurrencyAmountResult
            {
                FromCurrency = exchangeRatesEntity.Base,
                ConvertDate = exchangeRatesEntity.Date,
                ToCurrency = exchangeRatesEntity.Rates.FirstOrDefault().Key,
                Amount = exchangeRatesEntity.Rates.FirstOrDefault().Value
            };
        }

        public async Task<HistoricalCurrencyExchangeRatesRangePage> GetHistoricalCurrencyExchangeRatesRangePage(string baseCurrency, DateTime startDate, DateTime endDate, int page, CancellationToken cancellationToken)
        {
            if (startDate >= endDate) {
                throw new BusinessValidationException("Start date cannot be bigger than or equal end date");
            }
            if (startDate >= DateTime.UtcNow)
            {
                throw new BusinessValidationException("Start date cannot be bigger than current UTC");
            }
            var pageStartDate = startDate.AddDays(_currencyExchangerConfig.NumberOfWeeksPerHistoricalPage * 7 * (page - 1));
            var pageEndDate = startDate.AddDays(_currencyExchangerConfig.NumberOfWeeksPerHistoricalPage * 7 * page);
            if (endDate > DateTime.UtcNow)
            {
                pageEndDate = DateTime.UtcNow;
            }
            if (pageStartDate < startDate)
            {
                pageStartDate = startDate;
            }

            var historicalExchangeRatesRangeEntity =
                await _exchangeRatesDataAccessService.GetHistoricalExchangeRatesRangeEntity(baseCurrency, pageStartDate, pageEndDate, cancellationToken);
            return new HistoricalCurrencyExchangeRatesRangePage
            {
                Currency = historicalExchangeRatesRangeEntity.Base,
                Page = page,
                Rates = historicalExchangeRatesRangeEntity.Rates
            };
        }
    }
}
