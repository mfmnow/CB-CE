using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Collections.Generic;
using System;

namespace Mfm.Bc.CurrencyExchanger.Data.Services.UnitTests.MockProviders
{
    internal static class FrankfurterAppExchangeRatesDataAccessServiceMockerProvider
    {
        public static string MockedCurrency = "MockedCurrency";
        public static string MockedToCurrency = "MockedToCurrency";
        public static int MockedAmount = 20;
#pragma warning disable S6562 // Always set the "DateTimeKind" when creating new "DateTime" instances
        public static DateTime MockedStartDate = new DateTime(2000, 10, 10);
        public static DateTime MockedEndDate = new DateTime(2000, 11, 10);
#pragma warning restore S6562 // Always set the "DateTimeKind" when creating new "DateTime" instances
        public static int MockedPage = 5;
        public static Dictionary<string, double> MockedRates = new Dictionary<string, double>() {
            { "MockedToCurrency" , 1.1 } ,
            { "mockedCurrency2" , 5.1 }
        };

        public static HttpResponseMessage GetMockedHttpResponseMessage(object contentToSerialize)
        {
            HttpContent content = new StringContent(JsonSerializer.Serialize(contentToSerialize));

            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content };
        }
    }
}
