using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Serialization;

namespace Booth.PortfolioManager.RestApi.Test.Serialization
{
    class DeserializeTests
    {
        [TestCase]
        public void StandardTypes()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"integer\":5,\"string\":\"Hello\", \"decimal\":123.45,\"dateTime\":\"2004-02-03T14:22:04\"}";

            var result = serializer.Deserialize<StandardTypesTestData>(json);

            Assert.Multiple(() =>
            {
                Assert.That(result.Integer, Is.EqualTo(5));
                Assert.That(result.String, Is.EqualTo("Hello"));
                Assert.That(result.Decimal, Is.EqualTo(123.45m));
                Assert.That(result.DateTime, Is.EqualTo(new DateTime(2004, 02, 03, 14, 22, 04)));
            });
        }


        [TestCase]
        public void DateType()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"date\":\"2004-01-02\"}";

            var result = serializer.Deserialize<DateTestData>(json);

            Assert.That(result.Date, Is.EqualTo(new Date(2004, 01, 02)));
        }

        [TestCase]
        public void TimeType()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"time\":\"13:45:01\"}";

            var result = serializer.Deserialize<TimeTestData>(json);

            Assert.That(result.Time, Is.EqualTo(new Time(13, 45, 01)));
        }

        [TestCase]
        public void ExtraFieldReceived()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"field\":\"Hello\",\"extra\":\"value\"}";

            var result = serializer.Deserialize<SingleValueTestData>(json);

            Assert.That(result.Field, Is.EqualTo("Hello"));
        }

        [TestCase]
        public void NotAllFieldsReceived()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"string\":\"Hello\"}";

            var result = serializer.Deserialize<StandardTypesTestData>(json);

            Assert.Multiple(() =>
            {
                Assert.That(result.String, Is.EqualTo("Hello"));
                Assert.That(result.Integer, Is.EqualTo(0));
            });
        }

        [TestCase]
        public void DeserializeFromStream()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"field\":\"Hello\"}";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var result = serializer.Deserialize<SingleValueTestData>(stream);

            Assert.That(result.Field, Is.EqualTo("Hello"));
        }

        [TestCase]
        public void DeserializeFromStreamInvalidJson()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"field\" \"Hello\"}";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            Assert.That(() => serializer.Deserialize<StandardTypesTestData>(stream), Throws.TypeOf<Newtonsoft.Json.JsonReaderException>());
        }

        [TestCase]
        public void DeserializeFromStreamTypeWrong()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"field\":\"Hello\"}";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var result = serializer.Deserialize<StandardTypesTestData>(stream);

            Assert.Multiple(() =>
            {
                Assert.That(result.Integer, Is.EqualTo(0));
                Assert.That(result.String, Is.Null);
                Assert.That(result.Decimal, Is.EqualTo(0.00m));
                Assert.That(result.DateTime, Is.EqualTo(new DateTime(0001, 01, 01, 00, 00, 00)));
            });
        }

    }

}
