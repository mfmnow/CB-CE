using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mfm.Bc.CurrencyExchanger.Data.Entities
{
    public class HistoricalExchangeRatesRangeEntity
    {
        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("base")]
        public string Base { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, Dictionary<string, double>> Rates { get; set; }
    }
}
