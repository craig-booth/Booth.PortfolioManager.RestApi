using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

using Xunit;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Serialization;

namespace Booth.PortfolioManager.RestApi.Test.Serialization
{
    public class SerializeTests
    {
        [Fact]
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
            var result = JToken.Parse(serializer.Serialize<StandardTypesTestData>(obj));
          
            var expectedJson = JToken.Parse("{\"integer\":5,\"string\":\"Hello\",\"decimal\":123.45,\"dateTime\":\"2004-02-03T14:22:04\"}");

            result.Should().BeEquivalentTo(expectedJson);
        }


        [Fact]
        public void DateType()
        {
            var serializer = new RestClientSerializer();

            var obj = new DateTestData()
            {
                Date = new Date(2004, 01, 02)
            };
            var result = JToken.Parse(serializer.Serialize<DateTestData>(obj));

            var expectedJson = JToken.Parse("{\"date\":\"2004-01-02\"}");

            result.Should().BeEquivalentTo(expectedJson);
        }

        [Fact]
        public void TimeType()
        {
            var serializer = new RestClientSerializer();

            var obj = new TimeTestData()
            {
                Time = new Time(13, 45, 01)
            };
            var result = JToken.Parse(serializer.Serialize<TimeTestData>(obj));

            var expectedJson = JToken.Parse("{\"time\":\"13:45:01\"}");

            result.Should().BeEquivalentTo(expectedJson);
        }

        [Fact]
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
            var result = JToken.Parse(serializer.Serialize<StandardTypesTestData>(obj));

            var expectedJson = JToken.Parse("{\"integer\":5,\"decimal\":123.45,\"dateTime\":\"2004-02-03T14:22:04\"}");

            result.Should().BeEquivalentTo(expectedJson);
        }

        [Fact]
        public void SerializeUntypedToStream()
        {
            var serializer = new RestClientSerializer();
            var stream = new MemoryStream();

            var obj = new SingleValueTestData()
            {
                Field = "Hello",
            };
            serializer.Serialize(stream, obj);

            var expectedJson = JToken.Parse("{\"field\":\"Hello\"}");

            var result = JToken.Parse(Encoding.UTF8.GetString(stream.ToArray()));

            result.Should().BeEquivalentTo(expectedJson);
        }

        [Fact]
        public void SerializeTypedToStream()
        {
            var serializer = new RestClientSerializer();
            var stream = new MemoryStream();

            var obj = new SingleValueTestData()
            {
                Field = "Hello",
            };
            serializer.Serialize<SingleValueTestData>(stream, obj);

            var expectedJson = JToken.Parse("{\"field\":\"Hello\"}");

            var result = JToken.Parse(Encoding.UTF8.GetString(stream.ToArray()));

            result.Should().BeEquivalentTo(expectedJson);
        }
        
    }

}
