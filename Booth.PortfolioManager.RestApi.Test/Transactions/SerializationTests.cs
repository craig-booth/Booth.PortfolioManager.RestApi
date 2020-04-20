using System;

using NUnit.Framework;

using Booth.PortfolioManager.RestApi.Transactions;
using Booth.PortfolioManager.RestApi.Serialization;

namespace Booth.PortfolioManager.RestApi.Test.Transactions
{
    class SerializationTests
    {
        [TestCase]
        public void SerializeAquisition()
        {
            var serializer = new RestClientSerializer();

            var transactionId = Guid.NewGuid();
            var stockId = Guid.NewGuid();
            var transaction = new Aquisition()
            {
                Id = transactionId,
                Stock = stockId,
                TransactionDate = new DateTime(2000, 01, 10),
                Comment = "comment",
                Description = "description",
                Units = 100,
                AveragePrice = 12.00m,
                TransactionCosts = 19.95m,
                CreateCashTransaction = true
            };

            var json = serializer.Serialize(transaction);

            var expectedJson = "{\"id\":\"" + transactionId + "\","
                             + "\"stock\":\"" + stockId + "\","
                             + "\"type\":\"aquisition\","
                             + "\"transactionDate\":\"2000-01-10\","
                             + "\"comment\":\"comment\","
                             + "\"description\":\"description\","
                             + "\"units\":\"100\","
                             + "\"averagePrice\":\"12.00\","
                             + "\"transactionCosts\":\"19.95\","
                             + "\"createCashTransaction\":\"true\"}";

            Assert.That(json, Is.EqualTo(expectedJson));
        }

        [TestCase]
        public void DeserializeAquisition()
        {
            var serializer = new RestClientSerializer();

            var transactionId = Guid.NewGuid();
            var stockId = Guid.NewGuid();

            var json = "{\"id\":\"" + transactionId + "\","
                             + "\"stock\":\"" + stockId + "\"}"
                             + "\"type\":\"aquisition\""
                             + "\"transactionDate\":\"2000-01-10\"}"
                             + "\"comment\":\"comment\"}"
                             + "\"description\":\"description\"}"
                             + "\"units\":\"100\"}"
                             + "\"averagePrice\":\"12.00\"}"
                             + "\"transactionCosts\":\"19.95\"}"
                             + "\"createCashTransaction\":\"true\"}";

            var transaction = serializer.Deserialize<Transaction>(json);

            Assert.That(transaction, Is.TypeOf<Aquisition>());
            var aquisition = transaction as Aquisition;
            Assert.Multiple(() =>
            {
                Assert.That(aquisition.Id, Is.EqualTo(transactionId));
                Assert.That(aquisition.Stock, Is.EqualTo(stockId));
                Assert.That(aquisition.TransactionDate, Is.EqualTo(new DateTime(2000, 01, 10)));
                Assert.That(aquisition.Comment, Is.EqualTo("comment"));
                Assert.That(aquisition.Description, Is.EqualTo("description"));
                Assert.That(aquisition.Units, Is.EqualTo(100));
                Assert.That(aquisition.AveragePrice, Is.EqualTo(12.00m));
                Assert.That(aquisition.TransactionCosts, Is.EqualTo(19.95m));
                Assert.That(aquisition.CreateCashTransaction, Is.EqualTo(true));
            });
        }

    }
}
