using Mfm.Bc.CurrencyExchanger.Domain.Models.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mfm.Bc.CurrencyExchanger.Domain.Contracts
{
    public interface IExchangeRatesDomainService
    {
        /// <summary>
        /// Check currency and throws BusinessValidationException if matched any unsupported currency
        /// </summary>
        /// <param name="currency">Currency to check</param>
        void CheckUnsupportedCurrencies(string currency);

        /// <summary>
        /// Gets latest exchange rates against a given currencies list
        /// </summary>
        /// <param name="currency">Short name of a currency.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Currency exchange rates object <see cref="CurrencyExchangeRates"/></returns>
        Task<CurrencyExchangeRates> GetCurrencyExchangeRates(string currency, CancellationToken cancellationToken);

        /// <summary>
        /// Converts an amount of a currency to another currency given current exchange rate
        /// </summary>
        /// <param name="fromCurrency">Short name of from currency.</param>
        /// <param name="toCurrency">Short name of to currency.</param>
        /// <param name="amount">Amount of currency to be converted</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Converted amount details <see cref="ConvertCurrencyAmountResult"/></returns>
        Task<ConvertCurrencyAmountResult> ConvertCurrencyAmount(string fromCurrency, string toCurrency, double amount, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a single page of historical rates data of a currency.
        /// </summary>
        /// <param name="baseCurrency">Short name of a currency.</param>
        /// <param name="startDate">Range start date</param>
        /// <param name="endDate">Range end date</param>
        /// <param name="page">Targeted page</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Targeted hitorical rates page <see cref="HistoricalCurrencyExchangeRatesRangePage"/></returns>
        Task<HistoricalCurrencyExchangeRatesRangePage> GetHistoricalCurrencyExchangeRatesRangePage(string baseCurrency, DateTime startDate, DateTime endDate, int page, CancellationToken cancellationToken);
    }
}
