using System;

namespace Mfm.Bc.CurrencyExchanger.Domain.Models.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message): base(message)
        {
        }
    }
}
