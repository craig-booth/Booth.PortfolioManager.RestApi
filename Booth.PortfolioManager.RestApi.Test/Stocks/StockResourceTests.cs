using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xunit;
using Moq;
using FluentAssertions;

using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Stocks;
using Booth.Common;

namespace Booth.PortfolioManager.RestApi.Test.Stocks
{
    public class StockResourceTests
    {

        [Fact]
        public async Task GetAllStocks()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stocks = new List<StockResponse>()
            {
                new StockResponse() { Id = Guid.NewGuid(), AsxCode = "ABC" },
                new StockResponse() { Id = Guid.NewGuid(), AsxCode = "AYZ" }
            };

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<List<StockResponse>>(It.Is<string>(x => x == "stocks")))
                .Returns(Task<List<StockResponse>>.FromResult(stocks))
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var result = await resource.Get();

            result.Should().BeEquivalentTo(stocks);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetAllStocksAtDate()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stocks = new List<StockResponse>()
            {
                new StockResponse() { Id = Guid.NewGuid(), AsxCode = "ABC" },
                new StockResponse() { Id = Guid.NewGuid(), AsxCode = "AYZ" }
            };

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<List<StockResponse>>(It.Is<string>(x => x == "stocks?date=2000-01-02")))
                .Returns(Task<List<StockResponse>>.FromResult(stocks))
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var result = await resource.Get(new Date(2000, 01, 02));

            result.Should().BeEquivalentTo(stocks);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetAllStocksInDateRange()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stocks = new List<StockResponse>()
            {
                new StockResponse() { Id = Guid.NewGuid(), AsxCode = "ABC" },
                new StockResponse() { Id = Guid.NewGuid(), AsxCode = "AYZ" }
            };

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<List<StockResponse>>(It.Is<string>(x => x == "stocks?fromdate=2000-01-02&todate=2010-05-06")))
                .Returns(Task<List<StockResponse>>.FromResult(stocks))
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var result = await resource.Get(new DateRange(new Date(2000, 01, 02), new Date(2010, 05, 06)));

            result.Should().BeEquivalentTo(stocks);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetById()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stock = new StockResponse() { Id = Guid.NewGuid(), AsxCode = "ABC" };

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<StockResponse>(It.Is<string>(x => x == "stocks/" + stock.Id)))
                .Returns(Task<StockResponse>.FromResult(stock))
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var result = await resource.Get(stock.Id);

            result.Should().BeEquivalentTo(stock);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetByIdAtDate()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stock = new StockResponse() { Id = Guid.NewGuid(), AsxCode = "ABC" };

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<StockResponse>(It.Is<string>(x => x == "stocks/" + stock.Id + "?date=2000-01-02")))
                .Returns(Task<StockResponse>.FromResult(stock))
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var result = await resource.Get(stock.Id, new Date(2000, 01, 02));

            result.Should().BeEquivalentTo(stock);

            mockRepository.Verify();
        }

        [Fact]
        public async Task Find()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stocks = new List<StockResponse>()
            {
                new StockResponse() { Id = Guid.NewGuid(), AsxCode = "ABC" },
                new StockResponse() { Id = Guid.NewGuid(), AsxCode = "AYZ" }
            };


            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<List<StockResponse>>(It.Is<string>(x => x == "stocks?query=ABC")))
                .Returns(Task<List<StockResponse>>.FromResult(stocks))
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var result = await resource.Find("ABC");

            result.Should().BeEquivalentTo(stocks);

            mockRepository.Verify();
        }

        [Fact]
        public async Task FindAtDate()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stocks = new List<StockResponse>()
            {
                new StockResponse() { Id = Guid.NewGuid(), AsxCode = "ABC" },
                new StockResponse() { Id = Guid.NewGuid(), AsxCode = "AYZ" }
            };


            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<List<StockResponse>>(It.Is<string>(x => x == "stocks?query=ABC&date=2000-01-02")))
                .Returns(Task<List<StockResponse>>.FromResult(stocks))
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var result = await resource.Find("ABC", new Date(2000, 01, 02));

            result.Should().BeEquivalentTo(stocks);

            mockRepository.Verify();
        }

        [Fact]
        public async Task FindInDateRange()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stocks = new List<StockResponse>()
            {
                new StockResponse() { Id = Guid.NewGuid(), AsxCode = "ABC" },
                new StockResponse() { Id = Guid.NewGuid(), AsxCode = "AYZ" }
            };


            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<List<StockResponse>>(It.Is<string>(x => x == "stocks?query=ABC&fromdate=2000-01-02&todate=2010-05-06")))
                .Returns(Task<List<StockResponse>>.FromResult(stocks))
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var result = await resource.Find("ABC", new DateRange(new Date(2000, 01, 02), new Date(2010, 05, 06)));

            result.Should().BeEquivalentTo(stocks);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetHistory()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stock = new StockHistoryResponse() { Id = Guid.NewGuid(), AsxCode = "ABC" };

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<StockHistoryResponse>(It.Is<string>(x => x == "stocks/" + stock.Id + "/history")))
                .Returns(Task<StockHistoryResponse>.FromResult(stock))
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var result = await resource.GetHistory(stock.Id);

            result.Should().BeEquivalentTo(stock);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetPrices()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stock = new StockPriceResponse() { Id = Guid.NewGuid(), AsxCode = "ABC" };

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<StockPriceResponse>(It.Is<string>(x => x == "stocks/" + stock.Id + "/closingprices?fromdate=2000-01-02&todate=2010-05-06")))
                .Returns(Task<StockPriceResponse>.FromResult(stock))
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var result = await resource.GetPrices(stock.Id, new DateRange(new Date(2000, 01, 02), new Date(2010, 05, 06)));

            result.Should().BeEquivalentTo(stock);

            mockRepository.Verify();
        }

        [Fact]
        public async Task CreateStock()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stockId = Guid.NewGuid();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.PostAsync<CreateStockCommand>(
                It.Is<string>(x => x == "stocks"),
                It.Is<CreateStockCommand>(x => x.Id == stockId && x.AsxCode == "ABC")))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var stock = new CreateStockCommand()
            {
                Id = stockId,
                AsxCode = "ABC"
            };
            await resource.CreateStock(stock);

            mockRepository.Verify();
        }

        [Fact]
        public async Task ChangeStock()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stockId = Guid.NewGuid();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.PostAsync<ChangeStockCommand>(
                It.Is<string>(x => x == "stocks/" + stockId + "/change"),
                It.Is<ChangeStockCommand>(x => x.Id == stockId && x.AsxCode == "ABC")))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var change = new ChangeStockCommand()
            {
                Id = stockId,
                AsxCode = "ABC",
            };
            await resource.ChangeStock(change);

            mockRepository.Verify();
        }

        [Fact]
        public async Task DelistStock()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stockId = Guid.NewGuid();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.PostAsync<DelistStockCommand>(
                It.Is<string>(x => x == "stocks/" + stockId + "/delist"),
                It.Is<DelistStockCommand>(x => x.Id == stockId && x.DelistingDate == new Date(2000, 01, 02))))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var delist = new DelistStockCommand()
            {
                Id = stockId,
                DelistingDate = new Date(2000, 01, 02),
            };
            await resource.DelistStock(delist);

            mockRepository.Verify();
        }

        [Fact]
        public async Task UpdateClosingPrices()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stockId = Guid.NewGuid();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.PostAsync<UpdateClosingPricesCommand>(
                It.Is<string>(x => x == "stocks/" + stockId + "/closingprices"),
                It.Is<UpdateClosingPricesCommand>(x => x.Id == stockId && x.ClosingPrices.Count == 1)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var update = new UpdateClosingPricesCommand()
            {
                Id = stockId
            };
            update.AddClosingPrice(new Date(2000, 01, 02), 12.00m);
            await resource.UpdateClosingPrices(update);

            mockRepository.Verify();
        }

        [Fact]
        public async Task ChangeDividendRules()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stockId = Guid.NewGuid();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.PostAsync<ChangeDividendRulesCommand>(
                It.Is<string>(x => x == "stocks/" + stockId + "/changedividendrules"),
                It.Is<ChangeDividendRulesCommand>(x => x.Id == stockId && x.CompanyTaxRate == 0.45m)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var change = new ChangeDividendRulesCommand()
            {
                Id = stockId,
                CompanyTaxRate = 0.45m
            };
            await resource.ChangeDividendRules(change);

            mockRepository.Verify();
        }

        [Fact]
        public async Task ChangeReleativeNTAs()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stockId = Guid.NewGuid();

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.PostAsync<ChangeRelativeNtaCommand>(
                It.Is<string>(x => x == "stocks/" + stockId + "/changerelativenta"),
                It.Is<ChangeRelativeNtaCommand>(x => x.Id == stockId && x.RelativeNtas.Count == 1)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var resource = new StockResource(messageHandler.Object);

            var change = new ChangeRelativeNtaCommand()
            {
                Id = stockId,
            };
            change.AddRelativeNta("XYZ", 0.50m);

            await resource.ChangeRelativeNTAs(change);

            mockRepository.Verify();
        }
    }
}
