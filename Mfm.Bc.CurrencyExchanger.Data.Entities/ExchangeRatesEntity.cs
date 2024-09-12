using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mfm.Bc.CurrencyExchanger.Data.Entities
{
    public class ExchangeRatesEntity
    {
        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("base")]
        public string Base { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, double> Rates { get; set; }
    }
}
