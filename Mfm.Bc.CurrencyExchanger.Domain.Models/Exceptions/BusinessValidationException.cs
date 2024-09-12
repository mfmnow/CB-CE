using System;

namespace Mfm.Bc.CurrencyExchanger.Domain.Models.Exceptions
{
    public class BusinessValidationException : Exception
    {
        public BusinessValidationException(string message): base(message)
        {
        }
    }
}
