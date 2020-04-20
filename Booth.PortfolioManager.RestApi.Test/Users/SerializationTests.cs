using System;

using NUnit.Framework;

using Booth.PortfolioManager.RestApi.Users;
using Booth.PortfolioManager.RestApi.Serialization;


namespace Booth.PortfolioManager.RestApi.Test.Users
{

    class SerializationTests
    {
        [TestCase]
        public void SerializeAuthenticationRequest()
        {
            var serializer = new RestClientSerializer();

            var request = new AuthenticationRequest()
            {
                UserName = "JoeBlogs",
                Password = "secret"
            };

            var json = serializer.Serialize(request);

            var expectedJson = "{\"userName\":\"JoeBlogs\",\"password\":\"secret\"}";

            Assert.That(json, Is.EqualTo(expectedJson));
        }

        [TestCase]
        public void DeserializeAuthenticationRequest()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"userName\":\"JoeBlogs\",\"password\":\"secret\"}";

            var request = serializer.Deserialize<AuthenticationRequest>(json);

            Assert.Multiple(() =>
            {
                Assert.That(request.UserName, Is.EqualTo("JoeBlogs"));
                Assert.That(request.Password, Is.EqualTo("secret"));
            });
        }

        [TestCase]
        public void SerializeAuthenticationResponse()
        {
            var serializer = new RestClientSerializer();

            var response = new AuthenticationResponse()
            {
                Token = "tokendata"
            };

            var json = serializer.Serialize(response);

            var expectedJson = "{\"token\":\"tokendata\"}";

            Assert.That(json, Is.EqualTo(expectedJson));
        }

        [TestCase]
        public void DeserializeAuthenticationResponse()
        {
            var serializer = new RestClientSerializer();

            var json = "{\"token\":\"tokendata\"}";

            var response = serializer.Deserialize<AuthenticationResponse>(json);

            Assert.Multiple(() =>
            {
                Assert.That(response.Token, Is.EqualTo("tokendata"));
            });
        }
    }
}
