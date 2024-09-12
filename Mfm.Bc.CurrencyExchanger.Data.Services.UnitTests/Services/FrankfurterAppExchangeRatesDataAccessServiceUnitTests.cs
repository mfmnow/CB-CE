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
    public class FrankfurterAppExchangeRatesDataAccessServiceUnitTests
    {
        private readonly Mock<HttpClient> _mockedHttpClient;
        private readonly Mock<ILogger<FrankfurterAppExchangeRatesDataAccessService>> _mockedLogger;
        private readonly Mock<IExchangeRatesCacheService> _mockedExchangeRatesCacheService;
        private readonly Mock<FrankfurterAppExchangeRatesDataAccessService> _frankfurterAppExchangeRatesDataAccessService;

        public FrankfurterAppExchangeRatesDataAccessServiceUnitTests()
        {
            _mockedHttpClient = new Mock<HttpClient>();
            _mockedExchangeRatesCacheService = new Mock<IExchangeRatesCacheService>();
            _mockedLogger = new Mock<ILogger<FrankfurterAppExchangeRatesDataAccessService>>();
            _frankfurterAppExchangeRatesDataAccessService = new Mock<FrankfurterAppExchangeRatesDataAccessService>
                (_mockedHttpClient.Object, _mockedExchangeRatesCacheService.Object, _mockedLogger.Object)
            { CallBase = true };
        }

        [Fact]
        public async Task GetLatestExchangeRatesEntity_Should_Follow_LogicalFlow_And_Return_Valid_Result_When_Called()
        {
            //Arrange
            var mockedExchangeRatesEntity = new ExchangeRatesEntity
            {
                Base = FrankfurterAppExchangeRatesDataAccessServiceMockerProvider.MockedCurrency,
                Amount = FrankfurterAppExchangeRatesDataAccessServiceMockerProvider.MockedAmount,
                Rates = FrankfurterAppExchangeRatesDataAccessServiceMockerProvider.MockedRates
            };
            var endpoint = $"latest?from={FrankfurterAppExchangeRatesDataAccessServiceMockerProvider.MockedCurrency}";
            _frankfurterAppExchangeRatesDataAccessService.Setup(d => d.ExecuteGetRequestWithRetry(endpoint,CancellationToken.None))
                .Returns(Task.FromResult(FrankfurterAppExchangeRatesDataAccessServiceMockerProvider.GetMockedHttpResponseMessage(mockedExchangeRatesEntity)));

            //Execute
            var response = await _frankfurterAppExchangeRatesDataAccessService.Object.GetLatestExchangeRatesEntity(FrankfurterAppExchangeRatesDataAccessServiceMockerProvider.MockedCurrency, CancellationToken.None);

            //Assert
            Assert.NotNull(response);
            response.Rates.Should().BeEquivalentTo(mockedExchangeRatesEntity.Rates);
            Assert.Equal(response.Base, FrankfurterAppExchangeRatesDataAccessServiceMockerProvider.MockedCurrency);
            _frankfurterAppExchangeRatesDataAccessService.Verify(t => t.ExecuteGetRequestWithRetry(endpoint, CancellationToken.None), Times.Once);
        }

        //TODO Test rest of the methods in the service
    }
}
