using System;

namespace Mfm.Bc.CurrencyExchanger.Domain.Models.Models
{
    public class ConvertCurrencyAmountResult
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public DateTime ConvertDate { get; set; }
        public double Amount { get; set; }
    }
}
