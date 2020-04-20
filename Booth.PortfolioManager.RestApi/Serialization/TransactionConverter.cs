using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Booth.PortfolioManager.RestApi.Transactions;

namespace Booth.PortfolioManager.RestApi.Serialization
{
    class TransactionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType.IsSubclassOf(typeof(Transaction)));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var transaction = new Aquisition();

            return transaction;
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            var transaction = value as Transaction;

            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(transaction.Id);

            writer.WriteEndObject();
        }

    }

}
