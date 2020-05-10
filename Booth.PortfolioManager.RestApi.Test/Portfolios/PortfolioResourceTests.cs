using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xunit;
using Moq;
using FluentAssertions;

using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Portfolios;
using Booth.Common;
using System.Xml.Schema;

namespace Booth.PortfolioManager.RestApi.Test.Portfolios
{
    public class PortfolioResourceTests
    {
        [Fact]
        public async Task GetProperties()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();
            var response = new PortfolioPropertiesResponse()
            {
                Id = portfolioId
            };

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.Portfolio, portfolioId);
            messageHandler.Setup(x => x.GetAsync<PortfolioPropertiesResponse>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/properties")))
                .Returns(Task<PortfolioPropertiesResponse>.FromResult(response))
                .Verifiable();

            var resource = new PortfolioResource(messageHandler.Object);

            var result = await resource.GetProperties();

            result.Should().Be(response);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetSummary()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();
            var date = new Date(2001, 02, 03);
            var response = new PortfolioSummaryResponse()
            {
                PortfolioValue = 9000.00m
            };

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.Portfolio, portfolioId);
            messageHandler.Setup(x => x.GetAsync<PortfolioSummaryResponse>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/summary?date=2001-02-03")))
                .Returns(Task<List<PortfolioSummaryResponse>>.FromResult(response))
                .Verifiable();

            var resource = new PortfolioResource(messageHandler.Object);

            var result = await resource.GetSummary(date);

            result.Should().Be(response);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetPerformance()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();
            var dateRange = new DateRange(new Date(2001, 02, 03) ,new Date(2008, 06, 30));
            var response = new PortfolioPerformanceResponse()
            {
                ChangeInMarketValue = 1000.00m
            };

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.Portfolio, portfolioId);
            messageHandler.Setup(x => x.GetAsync<PortfolioPerformanceResponse>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/performance?fromdate=2001-02-03&todate=2008-06-30")))
                .Returns(Task<PortfolioPerformanceResponse>.FromResult(response))
                .Verifiable();

            var resource = new PortfolioResource(messageHandler.Object);

            var result = await resource.GetPerformance(dateRange);

            result.Should().Be(response);

            mockRepository.Verify();
        }

        [Theory]
        [InlineData(ValueFrequency.Day, "day")]
        [InlineData(ValueFrequency.Week, "week")]
        [InlineData(ValueFrequency.Month, "month")]
        public async Task GetValue(ValueFrequency frequency, string queryString)
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();
            var dateRange = new DateRange(new Date(2001, 02, 03), new Date(2008, 06, 30));
            var response = new PortfolioValueResponse();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.Portfolio, portfolioId);
            messageHandler.Setup(x => x.GetAsync<PortfolioValueResponse>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/value?fromdate=2001-02-03&todate=2008-06-30&frequency=" + queryString)))
                .Returns(Task<PortfolioValueResponse>.FromResult(response))
                .Verifiable();

            var resource = new PortfolioResource(messageHandler.Object);

            var result = await resource.GetValue(dateRange, frequency);

            result.Should().Be(response);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetTransactions()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();
            var dateRange = new DateRange(new Date(2001, 02, 03), new Date(2008, 06, 30));
            var response = new TransactionsResponse();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.Portfolio, portfolioId);
            messageHandler.Setup(x => x.GetAsync<TransactionsResponse>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/transactions?fromdate=2001-02-03&todate=2008-06-30")))
                .Returns(Task<TransactionsResponse>.FromResult(response))
                .Verifiable();

            var resource = new PortfolioResource(messageHandler.Object);

            var result = await resource.GetTransactions(dateRange);

            result.Should().Be(response);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetCapitalGains()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();
            var date = new Date(2001, 02, 03);
            var response = new SimpleUnrealisedGainsResponse();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.Portfolio, portfolioId);
            messageHandler.Setup(x => x.GetAsync<SimpleUnrealisedGainsResponse>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/capitalgains?date=2001-02-03")))
                .Returns(Task<SimpleUnrealisedGainsResponse>.FromResult(response))
                .Verifiable();

            var resource = new PortfolioResource(messageHandler.Object);

            var result = await resource.GetCapitalGains(date);

            result.Should().Be(response);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetDetailedCapitalGains()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();
            var date = new Date(2001, 02, 03);
            var response = new DetailedUnrealisedGainsResponse();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.Portfolio, portfolioId);
            messageHandler.Setup(x => x.GetAsync<DetailedUnrealisedGainsResponse>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/detailedcapitalgains?date=2001-02-03")))
                .Returns(Task<DetailedUnrealisedGainsResponse>.FromResult(response))
                .Verifiable();

            var resource = new PortfolioResource(messageHandler.Object);

            var result = await resource.GetDetailedCapitalGains(date);

            result.Should().Be(response);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetCGTLiability()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();
            var dateRange = new DateRange(new Date(2001, 02, 03), new Date(2008, 06, 30));
            var response = new CgtLiabilityResponse();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.Portfolio, portfolioId);
            messageHandler.Setup(x => x.GetAsync<CgtLiabilityResponse>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/cgtliability?fromdate=2001-02-03&todate=2008-06-30")))
                .Returns(Task<CgtLiabilityResponse>.FromResult(response))
                .Verifiable();

            var resource = new PortfolioResource(messageHandler.Object);

            var result = await resource.GetCGTLiability(dateRange);

            result.Should().Be(response);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetCashAccount()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();
            var dateRange = new DateRange(new Date(2001, 02, 03), new Date(2008, 06, 30));
            var response = new CashAccountTransactionsResponse();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.Portfolio, portfolioId);
            messageHandler.Setup(x => x.GetAsync<CashAccountTransactionsResponse>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/cashaccount?fromdate=2001-02-03&todate=2008-06-30")))
                .Returns(Task<CashAccountTransactionsResponse>.FromResult(response))
                .Verifiable();

            var resource = new PortfolioResource(messageHandler.Object);

            var result = await resource.GetCashAccount(dateRange);

            result.Should().Be(response);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetIncome()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();
            var dateRange = new DateRange(new Date(2001, 02, 03), new Date(2008, 06, 30));
            var response = new IncomeResponse();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.Portfolio, portfolioId);
            messageHandler.Setup(x => x.GetAsync<IncomeResponse>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/income?fromdate=2001-02-03&todate=2008-06-30")))
                .Returns(Task<IncomeResponse>.FromResult(response))
                .Verifiable();

            var resource = new PortfolioResource(messageHandler.Object);

            var result = await resource.GetIncome(dateRange);

            result.Should().Be(response);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetCorporateActions()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();
            var response = new CorporateActionsResponse();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.Portfolio, portfolioId);
            messageHandler.Setup(x => x.GetAsync<CorporateActionsResponse>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/corporateactions")))
                .Returns(Task<IncomeResponse>.FromResult(response))
                .Verifiable();

            var resource = new PortfolioResource(messageHandler.Object);

            var result = await resource.GetCorporateActions();

            result.Should().Be(response);

            mockRepository.Verify();
        }

    }
}
