using System;

using Xunit;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;

using Booth.PortfolioManager.RestApi.Users;
using Booth.PortfolioManager.RestApi.Serialization;


namespace Booth.PortfolioManager.RestApi.Test.Users
{

    public class SerializationTests
    {
        [Fact]
        public void SerializeAuthenticationRequest()
        {
            var serializer = new RestClientSerializer();

            var request = new AuthenticationRequest()
            {
                UserName = "JoeBlogs",
                Password = "secret"
            };

            var json = JToken.Parse(serializer.Serialize(request));

            var expectedJson = JToken.Parse("{\"userName\":\"JoeBlogs\",\"password\":\"secret\"}");

            json.Should().BeEquivalentTo(expectedJson);
        }
        
        [Fact]
        public void DeserializeAuthenticationRequest()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"userName\":\"JoeBlogs\",\"password\":\"secret\"}";

            var request = serializer.Deserialize<AuthenticationRequest>(json);

            request.Should().BeEquivalentTo(new AuthenticationRequest() { UserName = "JoeBlogs", Password = "secret" });
        }

        [Fact]
        public void SerializeAuthenticationResponse()
        {
            var serializer = new RestClientSerializer();

            var response = new AuthenticationResponse()
            {
                Token = "tokendata"
            };

            var json = JToken.Parse(serializer.Serialize(response));

            var expectedJson = JToken.Parse("{\"token\":\"tokendata\"}");

            json.Should().BeEquivalentTo(expectedJson);
        }

        [Fact]
        public void DeserializeAuthenticationResponse()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"token\":\"tokendata\"}";

            var response = serializer.Deserialize<AuthenticationResponse>(json);

            response.Should().BeEquivalentTo(new AuthenticationResponse() { Token = "tokendata" });
        } 
    }
}
