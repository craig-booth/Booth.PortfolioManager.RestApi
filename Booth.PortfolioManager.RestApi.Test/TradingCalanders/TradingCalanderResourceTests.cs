using System;
using System.Threading.Tasks;
using System.Net;
using System.Security;

using Xunit;
using Moq;

using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.TradingCalendars;
using FluentAssertions;

namespace Booth.PortfolioManager.RestApi.Test.TradingCalanders
{
    public class TradingCalendarResourceTests
    {
        [Fact]
        public async Task GetTradingCalendar()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<TradingCalendar>(It.Is<string>(x => x == "tradingcalendars/" + 2013)))
                .Returns(Task<TradingCalendar>.FromResult(new TradingCalendar() { Year = 2013 }))
                .Verifiable();

            var resource = new TradingCalandarResource(messageHandler.Object);

            var result = await resource.Get(2013);

            result.Should().BeEquivalentTo(new { Year = 2013 });

            mockRepository.Verify();
        }

        [Fact]
        public async Task UpdateTradingCalendar()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.PostAsync<TradingCalendar>(
                It.Is<string>(x => x == "tradingcalendars/" + 2013),
                It.Is<TradingCalendar>(x => x.Year == 2013)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var resource = new TradingCalandarResource(messageHandler.Object);

            var tradingCalander = new TradingCalendar() { Year = 2013 };
            await resource.Update(tradingCalander);

            mockRepository.Verify();
        }

    }
}
