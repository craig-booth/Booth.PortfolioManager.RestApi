using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using FluentAssertions;
using Moq;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.CorporateActions;

namespace Booth.PortfolioManager.RestApi.Test.CorporateActions
{
    public class CorporateActionResourceTests
    {
        [Theory]
        [MemberData(nameof(CorporateActionTypesData))]
        public async Task GetCorporateAction(CorporateAction action)
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var actionId = Guid.NewGuid();
            action.Id = actionId;
            var stockId = Guid.NewGuid();
            action.Stock = stockId;

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<CorporateAction>(It.Is<string>(x => x == "stocks/" + stockId + "/corporateactions/" + actionId)))
                .Returns(Task<CorporateAction>.FromResult(action as CorporateAction))
                .Verifiable();

            var resource = new CorporateActionResource(messageHandler.Object);

            var result = await resource.Get(stockId, actionId);

            result.Should().BeOfType(action.GetType()).And.BeEquivalentTo(new { Id = actionId });

            mockRepository.Verify();
        }

        [Theory]
        [MemberData(nameof(CorporateActionTypesData))]
        public async Task AddCorporateAction(CorporateAction action)
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var actionId = Guid.NewGuid();
            action.Id = actionId;
            var stockId = Guid.NewGuid();
            action.Stock = stockId;

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.PostAsync<CorporateAction>(
                It.Is<string>(x => x == "stocks/" + stockId + "/corporateactions"),
                It.Is<CorporateAction>(x => x.GetType() == action.GetType() && x.Id == actionId)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var resource = new CorporateActionResource(messageHandler.Object);

            await resource.Add(stockId, action);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetAll()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stockId = Guid.NewGuid();

            var actions = new List<CorporateAction>();
            actions.AddRange(CorporateActionTypesData().Select(x => (CorporateAction)x[0]));


            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<List<CorporateAction>>(It.Is<string>(x => x == "stocks/" + stockId + "/corporateactions")))
                .Returns(Task<List<CorporateAction>>.FromResult(actions))
                .Verifiable();

            var resource = new CorporateActionResource(messageHandler.Object);

            var result = await resource.GetAll(stockId);

            result.Should().BeEquivalentTo(actions);

            mockRepository.Verify();
        }

        [Fact]
        public async Task GetAllInDateRange()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var stockId = Guid.NewGuid();

            var actions = new List<CorporateAction>();
            actions.AddRange(CorporateActionTypesData().Select(x => (CorporateAction)x[0]));


            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.Setup(x => x.GetAsync<List<CorporateAction>>(It.Is<string>(x => x == "stocks/" + stockId + "/corporateactions?fromdate=2000-01-02&todate=2001-01-02")))
                .Returns(Task<List<CorporateAction>>.FromResult(actions))
                .Verifiable();

            var resource = new CorporateActionResource(messageHandler.Object);

            var result = await resource.GetAll(stockId, new DateRange(new Date(2000, 01, 02), new Date(2001, 01, 02)));

            result.Should().BeEquivalentTo(actions);

            mockRepository.Verify();
        }


        public static IEnumerable<object[]> CorporateActionTypesData()
        {
            var transactionTypes = TypeUtils.GetSubclassesOf(typeof(CorporateAction), true);

            return transactionTypes.Select(x => new object[] { Activator.CreateInstance(x) });
        }
    }
}
