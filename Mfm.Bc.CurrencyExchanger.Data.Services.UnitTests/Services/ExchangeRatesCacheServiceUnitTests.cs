using FluentAssertions;
using Mfm.Bc.CurrencyExchanger.Data.Contracts;
using Mfm.Bc.CurrencyExchanger.Data.Entities;
using Mfm.Bc.CurrencyExchanger.Data.Services.UnitTests.MockProviders;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mfm.Bc.CurrencyExchanger.Data.Services.UnitTests.Services
{
    public class ExchangeRatesCacheServiceUnitTests
    {
        private readonly Mock<ExchangeRatesCacheService> _exchangeRatesCacheService;

        public ExchangeRatesCacheServiceUnitTests()
        {
            _exchangeRatesCacheService = new Mock<ExchangeRatesCacheService>(){ CallBase = true };
        }

        [Fact]
        public void CreateCacheObject_Then_GetCachedObject_Should_Work_AS_Expected_When_Using_Valid_Key()
        {
            var mockedExchangeRatesEntity = new ExchangeRatesEntity
            {
                Base = FrankfurterAppExchangeRatesDataAccessServiceMockerProvider.MockedCurrency,
                Amount = FrankfurterAppExchangeRatesDataAccessServiceMockerProvider.MockedAmount,
                Rates = FrankfurterAppExchangeRatesDataAccessServiceMockerProvider.MockedRates
            };
            var mockedCacheKey = "mockedCacheKey";

            //Execute
            _exchangeRatesCacheService.Object.CreateCacheObject<ExchangeRatesEntity>(mockedCacheKey, mockedExchangeRatesEntity);
            var response = _exchangeRatesCacheService.Object.GetCachedObject<ExchangeRatesEntity>(mockedCacheKey);

            //Assert
            Assert.NotNull(response);
            response.Should().BeEquivalentTo(mockedExchangeRatesEntity);
        }

        [Fact]
        public void CreateCacheObject_Then_GetCachedObject_Should_Return_Null_When_Using_Non_Valid_Key()
        {
            var mockedCacheKey = "mockedCacheKey";

            //Execute
            var response = _exchangeRatesCacheService.Object.GetCachedObject<ExchangeRatesEntity>(mockedCacheKey);

            //Assert
            Assert.Null(response);
        }
    }
}
