using FluentAssertions;
using Mfm.Bc.CurrencyExchanger.Data.Contracts;
using Mfm.Bc.CurrencyExchanger.Data.Entities;
using Mfm.Bc.CurrencyExchanger.Domain.Models.Config;
using Mfm.Bc.CurrencyExchanger.Domain.Models.Exceptions;
using Mfm.Bc.CurrencyExchanger.Domain.Services.UnitTests.MockProviders;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mfm.Bc.CurrencyExchanger.Domain.Services.UnitTests
{
    public class ExchangeRatesDomainServiceUnitTests
    {
        private readonly Mock<IExchangeRatesDataAccessService> _mockedExchangeRatesDataAccessService;
        private readonly Mock<CurrencyExchangerConfig> _mockedCurrencyExchangerConfig;
        private readonly Mock<ExchangeRatesDomainService> _exchangeRatesDomainService;

        public ExchangeRatesDomainServiceUnitTests()
        {
            _mockedExchangeRatesDataAccessService = new Mock<IExchangeRatesDataAccessService>();
            _mockedCurrencyExchangerConfig = new Mock<CurrencyExchangerConfig>();
            _exchangeRatesDomainService = new Mock<ExchangeRatesDomainService>(_mockedCurrencyExchangerConfig.Object, _mockedExchangeRatesDataAccessService.Object)
            { CallBase = true };
        }

        [Fact]
        public void CheckUnsupportedCurrencies_Throws_Exception_When_UnsupportedCurrencies_Is_Tested()
        {
            //Arrange
            string[] mockedUnsupportedCurrencies = ExchangeRatesDomainServiceMockerProvider.MockedUnsupportedCurrencies;
            string testedCurrency = "Cur1";
            _mockedCurrencyExchangerConfig.Setup(c => c.UnsupportedConversionCurrencies).Returns(mockedUnsupportedCurrencies);

            //Execute
            Assert.Throws<BusinessValidationException>(() =>
            _exchangeRatesDomainService.Object.CheckUnsupportedCurrencies(testedCurrency));

            //Assert
            _mockedCurrencyExchangerConfig.Verify(t => t.UnsupportedConversionCurrencies, Times.Once);
        }

        [Fact]
        public void CheckUnsupportedCurrencies_Passes_UnsupportedCurrencies_Is_Tested()
        {
            //Arrange
            string[] mockedUnsupportedCurrencies = ExchangeRatesDomainServiceMockerProvider.MockedUnsupportedCurrencies;
            string testedCurrency = "testedCurrency";
            _mockedCurrencyExchangerConfig.Setup(c => c.UnsupportedConversionCurrencies).Returns(mockedUnsupportedCurrencies);

            //Execute
            _exchangeRatesDomainService.Object.CheckUnsupportedCurrencies(testedCurrency);

            //Assert
            _mockedCurrencyExchangerConfig.Verify(t => t.UnsupportedConversionCurrencies, Times.Once);
        }

        [Fact]
        public async Task GetCurrencyExchangeRates_Should_Follow_LogicalFlow_And_Return_Valid_Result_When_Called()
        {
            //Arrange
            var mockedExchangeRatesEntity = new ExchangeRatesEntity
            {
                Base = ExchangeRatesDomainServiceMockerProvider.MockedCurrency,
                Amount = ExchangeRatesDomainServiceMockerProvider.MockedAmount,
                Rates = ExchangeRatesDomainServiceMockerProvider.MockedRates
            };
            _mockedExchangeRatesDataAccessService.Setup(e => e.GetLatestExchangeRatesEntity(ExchangeRatesDomainServiceMockerProvider.MockedCurrency, CancellationToken.None))
                .Returns(Task.FromResult(mockedExchangeRatesEntity));

            //Execute
            var response = await _exchangeRatesDomainService.Object.GetCurrencyExchangeRates(ExchangeRatesDomainServiceMockerProvider.MockedCurrency, CancellationToken.None);

            //Assert
            Assert.NotNull(response);
            response.Rates.Should().BeEquivalentTo(mockedExchangeRatesEntity.Rates);
            Assert.Equal(response.Currency, ExchangeRatesDomainServiceMockerProvider.MockedCurrency);
            _mockedExchangeRatesDataAccessService.Verify(t => t.GetLatestExchangeRatesEntity(ExchangeRatesDomainServiceMockerProvider.MockedCurrency, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task ConvertCurrencyAmount_Should_Follow_LogicalFlow_And_Return_Valid_Result_When_Called()
        {
            //Arrange
            var mockedExchangeRatesEntity = new ExchangeRatesEntity
            {
                Base = ExchangeRatesDomainServiceMockerProvider.MockedCurrency,
                Amount = ExchangeRatesDomainServiceMockerProvider.MockedAmount,
                Rates = ExchangeRatesDomainServiceMockerProvider.MockedRates
            };
            _exchangeRatesDomainService.Setup(e => e.CheckUnsupportedCurrencies(It.IsAny<string>())).Verifiable();
            _mockedExchangeRatesDataAccessService.Setup(e => e.ConvertCurrencyAmount(ExchangeRatesDomainServiceMockerProvider.MockedCurrency,
                ExchangeRatesDomainServiceMockerProvider.MockedToCurrency,
                ExchangeRatesDomainServiceMockerProvider.MockedAmount, CancellationToken.None))
                .Returns(Task.FromResult(mockedExchangeRatesEntity));

            //Execute
            var response = await _exchangeRatesDomainService.Object.ConvertCurrencyAmount(ExchangeRatesDomainServiceMockerProvider.MockedCurrency,
                ExchangeRatesDomainServiceMockerProvider.MockedToCurrency,
                ExchangeRatesDomainServiceMockerProvider.MockedAmount, CancellationToken.None);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(response.FromCurrency, ExchangeRatesDomainServiceMockerProvider.MockedCurrency);
            Assert.Equal(response.ToCurrency, ExchangeRatesDomainServiceMockerProvider.MockedToCurrency);
            Assert.Equal(response.Amount, ExchangeRatesDomainServiceMockerProvider.MockedRates.FirstOrDefault().Value);
            _exchangeRatesDomainService.Verify(t => t.CheckUnsupportedCurrencies(It.IsAny<string>()), Times.Exactly(2));
            _mockedExchangeRatesDataAccessService.Verify(t => t.ConvertCurrencyAmount(ExchangeRatesDomainServiceMockerProvider.MockedCurrency,
                ExchangeRatesDomainServiceMockerProvider.MockedToCurrency,
                ExchangeRatesDomainServiceMockerProvider.MockedAmount, CancellationToken.None), Times.Once);
        }

        //TODO public async Task ConvertCurrencyAmount_Should_Follow_LogicalFlow_And_Return_Valid_Result_When_Dates_Are_Valid()
        //TODO public async Task ConvertCurrencyAmount_Should_Follow_LogicalFlow_And_Throws_Exception_When_StartDate_Bigger_Than_EndDate()
        //TODO public async Task ConvertCurrencyAmount_Should_Follow_LogicalFlow_And_Throws_Exception_When_StartDate_Is_Bigger_Than_Now()

    }
}