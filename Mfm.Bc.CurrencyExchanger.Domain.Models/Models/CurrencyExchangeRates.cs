using System.Collections.Generic;
using System;

namespace Mfm.Bc.CurrencyExchanger.Domain.Models.Models
{
    public class CurrencyExchangeRates
    {
        public string Currency { get; set; }
        public DateTime CurrencyDate { get; set; }
        public Dictionary<string, double> Rates { get; set; }
    }
}
