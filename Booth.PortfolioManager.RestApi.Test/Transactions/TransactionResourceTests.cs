using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;
using Moq;

using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Transactions;


namespace Booth.PortfolioManager.RestApi.Test.Transactions
{
    class TransactionResourceTests
    {
       
        [GenericTestCaseSource(nameof(TransactionTypesData))]
        public async Task GetTransaction<T>() where T : Transaction, new()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();

            var transactionId = Guid.NewGuid();
            var transaction = new T() { Id = transactionId };

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupGet(x => x.Portfolio).Returns(portfolioId);
            messageHandler.Setup(x => x.GetAsync<Transaction>(It.Is<string>(x => x == "portfolio/" + portfolioId + "/transactions/" + transactionId)))
                .Returns(Task<Transaction>.FromResult(transaction as Transaction))
                .Verifiable();

            var resource = new TransactionResource(messageHandler.Object);

            var result = await resource.Get(transactionId);

            Assert.That(result, Is.TypeOf<T>());
            Assert.That(result.Id, Is.EqualTo(transactionId));

            mockRepository.Verify();
        }

        [GenericTestCaseSource(nameof(TransactionTypesData))]
        public async Task AddTransaction<T>() where T : Transaction, new()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();

            var transactionId = Guid.NewGuid();
            var transaction = new T() { Id = transactionId };

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupGet(x => x.Portfolio).Returns(portfolioId);
            messageHandler.Setup(x => x.PostAsync<Transaction>(
                It.Is<string>(x => x == "portfolio/" + portfolioId + "/transactions"),
                It.Is<Transaction>(x => x.GetType() == typeof(T) &&  x.Id == transactionId))) 
                .Returns(Task.CompletedTask)
                .Verifiable();

            var resource = new TransactionResource(messageHandler.Object);

            await resource.Add(transaction);

            mockRepository.Verify();
        }


        [TestCase]
        public async Task AddMultipleTransaction()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var portfolioId = Guid.NewGuid();

            var transactions = new List<Transaction>();

            foreach (var transactionTypeData in TransactionTypesData())
            {
                var transactionType = transactionTypeData.Arguments[0] as Type;
                var transaction = Activator.CreateInstance(transactionType) as Transaction;
                transactions.Add(transaction);
            }

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


        static IEnumerable<TestCaseData> TransactionTypesData()
        {
            yield return new TestCaseData(typeof(Aquisition));
        }
    }
}
