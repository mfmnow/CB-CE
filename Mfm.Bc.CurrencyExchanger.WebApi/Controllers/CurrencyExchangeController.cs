using Mfm.Bc.CurrencyExchanger.Domain.Contracts;
using Mfm.Bc.CurrencyExchanger.Domain.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Mfm.Bc.CurrencyExchanger.WebApi.Controllers
{
    /// <summary>
    /// CurrencyExchangeController class
    /// </summary>
    [ApiController]
    [Route("api/currency-exchange")]
    [EnableRateLimiting("Default")]
    public class CurrencyExchangeController : ControllerBase
    {
        private readonly ILogger<CurrencyExchangeController> _logger;
        private readonly IExchangeRatesDomainService _exchangeRatesDomainService;

        /// <summary>
        /// Manages Currency Exchange features
        /// </summary>
        /// <param name="exchangeRatesDomainService"></param>
        /// <param name="logger"></param>
        public CurrencyExchangeController(IExchangeRatesDomainService exchangeRatesDomainService, ILogger<CurrencyExchangeController> logger)
        {
            _exchangeRatesDomainService = exchangeRatesDomainService;
            _logger = logger;
        }

        /// <summary>
        /// Gets latest exchange rates against a given currencies list (https://www.frankfurter.app/docs/#currencies)
        /// </summary>
        /// <param name="currency">Short name of a currency. Supported list: https://www.frankfurter.app/docs/#currencies</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Currency exchange rates object</returns>
        [HttpGet("get-latest-exchange-rates/{currency}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyExchangeRates))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLatestExchangeRates(string currency, CancellationToken cancellationToken)
        {
            var currencyExchangeRates = await _exchangeRatesDomainService.GetCurrencyExchangeRates(currency, cancellationToken);
            return Ok(currencyExchangeRates);
        }

        /// <summary>
        /// Converts an amount of a currency to another currency given current exchange rate
        /// </summary>
        /// <param name="fromCurrency">Short name of from currency. Supported list: https://www.frankfurter.app/docs/#currencies</param>
        /// <param name="toCurrency">Short name of to currency. Supported list: https://www.frankfurter.app/docs/#currencies</param>
        /// <param name="amount">Amount of currency to be converted</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Converted amount details</returns>
        [HttpGet("convert-currency-amount/{fromCurrency}/{toCurrency}/{amount}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConvertCurrencyAmountResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConvertCurrencyAmount(string fromCurrency, string toCurrency, int amount, CancellationToken cancellationToken)
        {
            var convertCurrencyAmountResult = await _exchangeRatesDomainService.ConvertCurrencyAmount(fromCurrency, toCurrency, amount, cancellationToken);
            return Ok(convertCurrencyAmountResult);
        }

        /// <summary>
        /// Returns a single page of historical rates data of a currency.
        /// </summary>
        /// <param name="currency">Short name of a currency. Supported list: https://www.frankfurter.app/docs/#currencies</param>
        /// <param name="startDate">Start date of range</param>
        /// <param name="endDate">End date of range</param>
        /// <param name="pageNumber">Targeted page number</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A single page of historical rates data of a currency</returns>
        [HttpGet("get-historical-exchange-rates-page/{currency}/{startDate}/{endDate}/{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HistoricalCurrencyExchangeRatesRangePage))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHistoricalExchangeRatesPage(string currency, DateTime startDate, DateTime endDate, int pageNumber, CancellationToken cancellationToken)
        {
            var historicalCurrencyExchangeRatesRangePage = await _exchangeRatesDomainService.
                GetHistoricalCurrencyExchangeRatesRangePage(currency, startDate, endDate, pageNumber, cancellationToken);
            return Ok(historicalCurrencyExchangeRatesRangePage);
        }

    }
}
