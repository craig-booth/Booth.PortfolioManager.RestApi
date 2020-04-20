using System;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;

using NUnit.Framework;
using Moq;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Serialization;
using Booth.PortfolioManager.RestApi.Test.Serialization;

namespace Booth.PortfolioManager.RestApi.Test
{
    class RestClientMessageHandlerGetAsyncTests
    {
        [TestCase]
        public async Task WithoutAuthentication()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            HttpRequestMessage requestMessage = null;
            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.OK, x => requestMessage = x);
            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Deserialize<Time>(It.IsAny<Stream>())).Returns(new Time());

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", httpHandler.Object, serializer.Object);

            var result = await messageHandler.GetAsync<Time>("authtest");

            Assert.That(requestMessage.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(requestMessage.RequestUri, Is.EqualTo(new Uri("http://test.com.au/api/v2/authtest")));
            Assert.That(requestMessage.Headers.Contains("Authorisation"), Is.False);
        }

        [TestCase]
        public async Task WithAuthentication()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            HttpRequestMessage requestMessage = null;
            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.OK, x => requestMessage = x);
            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Deserialize<Time>(It.IsAny<Stream>())).Returns(new Time());

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", httpHandler.Object, serializer.Object);
            messageHandler.JwtToken = "DummyToken";

            var result = await messageHandler.GetAsync<Time>("authtest");

            Assert.That(requestMessage.Method, Is.EqualTo(HttpMethod.Get));
            Assert.That(requestMessage.RequestUri, Is.EqualTo(new Uri("http://test.com.au/api/v2/authtest")));
            Assert.That(requestMessage.Headers.Authorization.Scheme, Is.EqualTo("Bearer"));
            Assert.That(requestMessage.Headers.Authorization.Parameter, Is.EqualTo("DummyToken"));
        }

        [TestCase]
        public void AuthenticationFailed()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.Forbidden);
            var serializer = mockRepository.Create<IRestClientSerializer>();

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", httpHandler.Object, serializer.Object);
            messageHandler.JwtToken = "DummyToken";

            Assert.That(async() => await messageHandler.GetAsync<SingleValueTestData>("authtest"),
                Throws.TypeOf<RestException>().With.Property("StatusCode").EqualTo(HttpStatusCode.Forbidden));
        }

        [TestCase]
        public void NotFound()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.NotFound);
            var serializer = mockRepository.Create<IRestClientSerializer>();

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", httpHandler.Object, serializer.Object);
            messageHandler.JwtToken = "DummyToken";

            Assert.That(async () => await messageHandler.GetAsync<SingleValueTestData>("authtest"), 
                Throws.TypeOf<RestException>().With.Property("StatusCode").EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase]
        public async Task SerializeObjectWhenReceiving()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var httpHandler = mockRepository.CreateJsonMessageHandler("{Field : \"Hello\"}");

            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Deserialize<SingleValueTestData>(It.IsAny<Stream>())).Returns(new SingleValueTestData() { Field = "Hello" }).Verifiable();

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", httpHandler.Object, serializer.Object);

            var data = await messageHandler.GetAsync<SingleValueTestData>("standardtypes");

            Assert.That(data.Field, Is.EqualTo("Hello"));

            mockRepository.Verify();
        }
    }

}
