using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Mfm.Bc.CurrencyExchanger.WebApi.Controllers;
using Mfm.Bc.CurrencyExchanger.Domain.Contracts;
using System.Threading;
using Mfm.Bc.CurrencyExchanger.Domain.Models.Models;
using Mfm.Bc.CurrencyExchanger.App.UnitTests.MockProviders;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace Mfm.Bc.CurrencyExchanger.App.UnitTests
{
    public class CurrencyExchangeControllerUnitTests
    {
        private readonly Mock<IExchangeRatesDomainService> _mockedExchangeRatesDomainService;
        private readonly Mock<ILogger<CurrencyExchangeController>> _mockedLogger;
        private readonly Mock<CurrencyExchangeController> _currencyExchangeController;

        public CurrencyExchangeControllerUnitTests()
        {
            _mockedExchangeRatesDomainService = new Mock<IExchangeRatesDomainService>();
            _mockedLogger = new Mock<ILogger<CurrencyExchangeController>>();
            _currencyExchangeController = new Mock<CurrencyExchangeController>(_mockedExchangeRatesDomainService.Object, _mockedLogger.Object)
            { CallBase = true };
        }

        [Fact]
        public async Task GetLatestExchangeRates_Should_Follow_LogicalFlow_And_Return_Valid_Result_When_Called()
        {
            //Arrange
            var mockedCurrencyExchangeRates = new CurrencyExchangeRates
            {
                Currency = CurrencyExchangeControllerUnitTestsMockerProvider.MockedCurrency,
                Rates = CurrencyExchangeControllerUnitTestsMockerProvider.MockedRates
            };
            _mockedExchangeRatesDomainService.Setup(e => e.GetCurrencyExchangeRates(CurrencyExchangeControllerUnitTestsMockerProvider.MockedCurrency, CancellationToken.None))
                .Returns(Task.FromResult(mockedCurrencyExchangeRates));

            //Execute
            var response = await _currencyExchangeController.Object.GetLatestExchangeRates(CurrencyExchangeControllerUnitTestsMockerProvider.MockedCurrency, CancellationToken.None);

            //Assert
            Assert.NotNull(response);
            var content = response as OkObjectResult;
            content?.Value?.Should().BeEquivalentTo(mockedCurrencyExchangeRates); 
            _mockedExchangeRatesDomainService.Verify(t => t.GetCurrencyExchangeRates(CurrencyExchangeControllerUnitTestsMockerProvider.MockedCurrency, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task ConvertCurrencyAmount_Should_Follow_LogicalFlow_And_Return_Valid_Result_When_Called()
        {
            //Arrange
            var convertCurrencyAmountResult = new ConvertCurrencyAmountResult
            {
                FromCurrency = CurrencyExchangeControllerUnitTestsMockerProvider.MockedCurrency,
                ToCurrency = CurrencyExchangeControllerUnitTestsMockerProvider.MockedToCurrency,
                Amount = CurrencyExchangeControllerUnitTestsMockerProvider.MockedAmount
            };
            _mockedExchangeRatesDomainService.Setup(e => e.ConvertCurrencyAmount(CurrencyExchangeControllerUnitTestsMockerProvider.MockedCurrency,
                CurrencyExchangeControllerUnitTestsMockerProvider.MockedToCurrency, CurrencyExchangeControllerUnitTestsMockerProvider.MockedAmount, CancellationToken.None))
                .Returns(Task.FromResult(convertCurrencyAmountResult));

            //Execute
            var response = await _currencyExchangeController.Object.ConvertCurrencyAmount(CurrencyExchangeControllerUnitTestsMockerProvider.MockedCurrency,
                CurrencyExchangeControllerUnitTestsMockerProvider.MockedToCurrency, CurrencyExchangeControllerUnitTestsMockerProvider.MockedAmount, CancellationToken.None);

            //Assert
            Assert.NotNull(response);
            var content = response as OkObjectResult;
            content?.Value?.Should().BeEquivalentTo(convertCurrencyAmountResult);
            _mockedExchangeRatesDomainService.Verify(t => t.ConvertCurrencyAmount(CurrencyExchangeControllerUnitTestsMockerProvider.MockedCurrency,
                CurrencyExchangeControllerUnitTestsMockerProvider.MockedToCurrency, CurrencyExchangeControllerUnitTestsMockerProvider.MockedAmount, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GetHistoricalExchangeRatesPage_Should_Follow_LogicalFlow_And_Return_Valid_Result_When_Called()
        {
            //Arrange
            var mockedHistoricalCurrencyExchangeRatesRangePage = new HistoricalCurrencyExchangeRatesRangePage
            {
                Currency = CurrencyExchangeControllerUnitTestsMockerProvider.MockedCurrency,
                Page = CurrencyExchangeControllerUnitTestsMockerProvider.MockedPage,
                Rates = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, double>> {
                    {CurrencyExchangeControllerUnitTestsMockerProvider.MockedEndDate.ToShortDateString(), CurrencyExchangeControllerUnitTestsMockerProvider.MockedRates}
                }
            };
            _mockedExchangeRatesDomainService.Setup(e => e.GetHistoricalCurrencyExchangeRatesRangePage(CurrencyExchangeControllerUnitTestsMockerProvider.MockedCurrency,
            CurrencyExchangeControllerUnitTestsMockerProvider.MockedStartDate, CurrencyExchangeControllerUnitTestsMockerProvider.MockedEndDate,
            CurrencyExchangeControllerUnitTestsMockerProvider.MockedPage, CancellationToken.None))
                .Returns(Task.FromResult(mockedHistoricalCurrencyExchangeRatesRangePage));

            //Execute
            var response = await _currencyExchangeController.Object.GetHistoricalExchangeRatesPage(CurrencyExchangeControllerUnitTestsMockerProvider.MockedCurrency,
            CurrencyExchangeControllerUnitTestsMockerProvider.MockedStartDate, CurrencyExchangeControllerUnitTestsMockerProvider.MockedEndDate,
            CurrencyExchangeControllerUnitTestsMockerProvider.MockedPage, CancellationToken.None);

            //Assert
            Assert.NotNull(response);
            var content = response as OkObjectResult;
            content?.Value?.Should().BeEquivalentTo(mockedHistoricalCurrencyExchangeRatesRangePage);
            _mockedExchangeRatesDomainService.Verify(t => t.GetHistoricalCurrencyExchangeRatesRangePage(CurrencyExchangeControllerUnitTestsMockerProvider.MockedCurrency,
            CurrencyExchangeControllerUnitTestsMockerProvider.MockedStartDate, CurrencyExchangeControllerUnitTestsMockerProvider.MockedEndDate,
            CurrencyExchangeControllerUnitTestsMockerProvider.MockedPage, CancellationToken.None), Times.Once);
        }
    }
}