namespace Mfm.Bc.CurrencyExchanger.Domain.Models.Config
{
    /// <summary>
    /// Frankfurter App service Config
    /// </summary>
    public class CurrencyExchangerConfig
    {
        /// <summary>
        /// API base URL
        /// </summary>
        public virtual string[] UnsupportedConversionCurrencies { get; set; }

        /// <summary>
        /// Number of weeks displayed per historical rates page
        /// </summary>
        public virtual int NumberOfWeeksPerHistoricalPage { get; set; }
    }
}
