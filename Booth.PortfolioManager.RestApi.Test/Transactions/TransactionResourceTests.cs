using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using FluentAssertions;
using Moq;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Transactions;


namespace Booth.PortfolioManager.RestApi.Test.Transactions
{
    public class TransactionResourceTests
    {
       
        [Theory]
        [MemberData(nameof(TransactionTypesData))]
        public async Task GetTransaction(Transaction transaction)
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();

            var transactionId = Guid.NewGuid();
            transaction.Id = transactionId;

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupGet(x => x.Portfolio).Returns(portfolioId);
            messageHandler.Setup(x => x.GetAsync<Transaction>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/transactions/" + transactionId)))
                .Returns(Task<Transaction>.FromResult(transaction as Transaction))
                .Verifiable();

            var resource = new TransactionResource(messageHandler.Object);

            var result = await resource.Get(transactionId);

            result.Should().BeOfType(transaction.GetType()).And.BeEquivalentTo(new { Id = transactionId});
            
            mockRepository.Verify();
        }

        [Theory]
        [MemberData(nameof(TransactionTypesData))]
        public async Task AddTransaction(Transaction transaction)
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();

            var transactionId = Guid.NewGuid();
            transaction.Id = transactionId;

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupGet(x => x.Portfolio).Returns(portfolioId);
            messageHandler.Setup(x => x.PostAsync<Transaction>(
                It.Is<string>(x => x == "portfolio/" + portfolioId + "/transactions"),
                It.Is<Transaction>(x => x.GetType() == transaction.GetType() &&  x.Id == transactionId))) 
                .Returns(Task.CompletedTask)
                .Verifiable();

            var resource = new TransactionResource(messageHandler.Object);

            await resource.Add(transaction);

            mockRepository.Verify();
        }


        [Fact]
        public async Task AddMultipleTransaction()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();

            var transactions = new List<Transaction>();
            transactions.AddRange(TransactionTypesData().Select(x => (Transaction)x[0]));

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupGet(x => x.Portfolio).Returns(portfolioId);
            messageHandler.Setup(x => x.PostAsync<IEnumerable<Transaction>>(
                It.Is<string>(x => x == "portfolio/" + portfolioId + "/transactions"),
                It.Is<IEnumerable<Transaction>>(x => x.Count() == transactions.Count)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var resource = new TransactionResource(messageHandler.Object);

            await resource.Add(transactions);

            mockRepository.Verify();
        } 

        public static IEnumerable<object[]> TransactionTypesData()
        {
            var transactionTypes = TypeUtils.GetSubclassesOf(typeof(Transaction), true);

            return transactionTypes.Select(x => new object[] { Activator.CreateInstance(x) });
        }
    }

    
}
