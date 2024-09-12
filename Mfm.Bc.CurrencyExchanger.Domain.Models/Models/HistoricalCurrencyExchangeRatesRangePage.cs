using System.Collections.Generic;

namespace Mfm.Bc.CurrencyExchanger.Domain.Models.Models
{
    public class HistoricalCurrencyExchangeRatesRangePage
    {
        public string Currency { get; set; }
        public int Page { get; set; }
        public Dictionary<string, Dictionary<string, double>> Rates { get; set; }
    }
}
