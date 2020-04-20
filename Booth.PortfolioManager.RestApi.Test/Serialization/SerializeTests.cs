using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Serialization;

namespace Booth.PortfolioManager.RestApi.Test.Serialization
{
    class SerializeTests
    {
        [TestCase]
        public void StandardTypes()
        {
            var serializer = new RestClientSerializer();

            var obj = new StandardTypesTestData()
            {
                Integer = 5,
                String = "Hello",
                Decimal = 123.45m,
                DateTime = new DateTime(2004, 02, 03, 14, 22, 04)
            };
            var result = serializer.Serialize<StandardTypesTestData>(obj);
          
            var expectedJson = "{\"integer\":5,\"string\":\"Hello\",\"decimal\":123.45,\"dateTime\":\"2004-02-03T14:22:04\"}";

            Assert.That(result, Is.EqualTo(expectedJson));
        }


        [TestCase]
        public void DateType()
        {
            var serializer = new RestClientSerializer();

            var obj = new DateTestData()
            {
                Date = new Date(2004, 01, 02)
            };
            var result = serializer.Serialize<DateTestData>(obj);

            var expectedJson = "{\"date\":\"2004-01-02\"}";

            Assert.That(result, Is.EqualTo(expectedJson));
        }

        [TestCase]
        public void TimeType()
        {
            var serializer = new RestClientSerializer();

            var obj = new TimeTestData()
            {
                Time = new Time(13, 45, 01)
            };
            var result = serializer.Serialize<TimeTestData>(obj);

            var expectedJson = "{\"time\":\"13:45:01\"}";

            Assert.That(result, Is.EqualTo(expectedJson));
        }

        [TestCase]
        public void NullValue()
        {
            var serializer = new RestClientSerializer();

            var obj = new StandardTypesTestData()
            {
                Integer = 5,
                String = null,
                Decimal = 123.45m,
                DateTime = new DateTime(2004, 02, 03, 14, 22, 04)
            };
            var result = serializer.Serialize<StandardTypesTestData>(obj);

            var expectedJson = "{\"integer\":5,\"decimal\":123.45,\"dateTime\":\"2004-02-03T14:22:04\"}";

            Assert.That(result, Is.EqualTo(expectedJson));
        }

        [TestCase]
        public void SerializeUntypedToStream()
        {
            var serializer = new RestClientSerializer();
            var stream = new MemoryStream();

            var obj = new SingleValueTestData()
            {
                Field = "Hello",
            };
            serializer.Serialize(stream, obj);

            var expectedJson = "{\"field\":\"Hello\"}";

            var result = Encoding.UTF8.GetString(stream.ToArray());
            Assert.That(result, Is.EqualTo(expectedJson));
        }

        [TestCase]
        public void SerializeTypedToStream()
        {
            var serializer = new RestClientSerializer();
            var stream = new MemoryStream();

            var obj = new SingleValueTestData()
            {
                Field = "Hello",
            };
            serializer.Serialize<SingleValueTestData>(stream, obj);

            var expectedJson = "{\"field\":\"Hello\"}";

            var result = Encoding.UTF8.GetString(stream.ToArray());
            Assert.That(result, Is.EqualTo(expectedJson));
        }

    }

}
