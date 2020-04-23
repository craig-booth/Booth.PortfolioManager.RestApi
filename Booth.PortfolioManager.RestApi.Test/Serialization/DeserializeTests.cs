using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

using Xunit;
using FluentAssertions;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Serialization;
using System.Runtime.InteropServices;

namespace Booth.PortfolioManager.RestApi.Test.Serialization
{
    public class DeserializeTests
    {
        [Fact]
        public void StandardTypes()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"integer\":5,\"string\":\"Hello\", \"decimal\":123.45,\"dateTime\":\"2004-02-03T14:22:04\"}";

            var result = serializer.Deserialize<StandardTypesTestData>(json);

            var expected = new StandardTypesTestData()
            {
                Integer = 5,
                String = "Hello",
                Decimal = 123.45m,
                DateTime = new DateTime(2004, 02, 03, 14, 22, 04)
            };

            result.Should().BeEquivalentTo(expected);
        }


        [Fact]
        public void DateType()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"date\":\"2004-01-02\"}";

            var result = serializer.Deserialize<DateTestData>(json);

            result.Should().BeEquivalentTo(new { Date = new Date(2004, 01, 02) });
        }

        [Fact]
        public void TimeType()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"time\":\"13:45:01\"}";

            var result = serializer.Deserialize<TimeTestData>(json);

            result.Should().BeEquivalentTo(new { Time = new Time(13, 45, 01) });
        }

        [Fact]
        public void ExtraFieldReceived()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"field\":\"Hello\",\"extra\":\"value\"}";

            var result = serializer.Deserialize<SingleValueTestData>(json);

            result.Should().BeEquivalentTo(new { Field = "Hello" } );
        }

        [Fact]
        public void NotAllFieldsReceived()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"string\":\"Hello\"}";

            var result = serializer.Deserialize<StandardTypesTestData>(json);

            var expected = new StandardTypesTestData() { String = "Hello" };
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void DeserializeFromStream()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"field\":\"Hello\"}";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var result = serializer.Deserialize<SingleValueTestData>(stream);

            var expected = new SingleValueTestData() { Field = "Hello" };
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void DeserializeFromStreamInvalidJson()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"field\" \"Hello\"}";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            Action a = () => serializer.Deserialize<StandardTypesTestData>(stream);
            a.Should().ThrowExactly<Newtonsoft.Json.JsonReaderException>();
        }

        [Fact]
        public void DeserializeFromStreamTypeWrong()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"field\":\"Hello\"}";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var result = serializer.Deserialize<StandardTypesTestData>(stream);

            result.Should().BeEquivalentTo(new StandardTypesTestData());
        } 

    }

}
