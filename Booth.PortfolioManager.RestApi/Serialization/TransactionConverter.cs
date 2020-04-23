using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Transactions;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Booth.PortfolioManager.RestApi.Serialization
{
    class TransactionConverter : JsonConverter
    {
        private Dictionary<string, Type> _TransactionTypes = new Dictionary<string, Type>();
        public TransactionConverter()
        {
            foreach (var transactionType in TypeUtils.GetSubclassesOf(typeof(Transaction), true))
            {
                var transaction = Activator.CreateInstance(transactionType) as Transaction;
                _TransactionTypes.Add(transaction.Type, transactionType);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Transaction));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JToken.ReadFrom(reader) as JObject;

            if (!jObject.TryGetValue("type", out var jToken))
                throw new JsonReaderException("Type field is missing. Unable to determine the type of the transaction");
            
            var type = jToken.ToString();
            if (!_TransactionTypes.TryGetValue(type, out var transactionType))
                throw new JsonReaderException("Type field is invalid. Unable to determine the type of the transaction");

            var transaction = Activator.CreateInstance(transactionType);

            serializer.Populate(jObject.CreateReader(), transaction);

            return transaction;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

    }

}
