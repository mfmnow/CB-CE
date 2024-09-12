using Mfm.Bc.CurrencyExchanger.Data.Entities;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mfm.Bc.CurrencyExchanger.Data.Contracts
{
    public interface IExchangeRatesDataAccessService
    {
        /// <summary>
        ///  In some cases, the Frankfurter API may not respond to the first request but to the second or third. A retry logic is needed.
        /// </summary>
        /// <param name="endpoint">Endpoint</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        Task<HttpResponseMessage> ExecuteGetRequestWithRetry(string endpoint, CancellationToken cancellationToken);

        /// <summary>
        /// Gets Get Latest Exchange Rates from data source
        /// </summary>
        /// <param name="baseCurrency">Base currency name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        Task<ExchangeRatesEntity> GetLatestExchangeRatesEntity(string baseCurrency, CancellationToken cancellationToken);

        /// <summary>
        /// Use data source to convert an amout from baseCurrency to toCurrency
        /// </summary>
        /// <param name="baseCurrency"></param>
        /// <param name="toCurrency"></param>
        /// <param name="amount"></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        Task<ExchangeRatesEntity> ConvertCurrencyAmount(string baseCurrency, string toCurrency, double amount, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieve historical data of a currency for a specific data range
        /// </summary>
        /// <param name="baseCurrency"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        Task<HistoricalExchangeRatesRangeEntity> GetHistoricalExchangeRatesRangeEntity(string baseCurrency, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    }
}
