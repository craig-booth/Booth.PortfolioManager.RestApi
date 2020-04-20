using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using NUnit.Framework;
using Moq;

using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Serialization;

using Booth.PortfolioManager.RestApi.Test.Serialization;


namespace Booth.PortfolioManager.RestApi.Test
{
    class RestClientMessageHandlerPostAsyncTests
    {
        [TestCase]
        public async Task WithoutAuthentication()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            HttpRequestMessage requestMessage = null;
            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.OK, x => requestMessage = x);

            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Serialize<SingleValueTestData>(It.IsAny<Stream>(), It.IsAny<SingleValueTestData>()));

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", httpHandler.Object, serializer.Object);

            var data = new SingleValueTestData()
            {
                Field = "test"
            };
            await messageHandler.PostAsync<SingleValueTestData>("authtest", data);

            Assert.That(requestMessage.Method, Is.EqualTo(HttpMethod.Post));
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
            serializer.Setup(x => x.Serialize<SingleValueTestData>(It.IsAny<Stream>(), It.IsAny<SingleValueTestData>()));

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", httpHandler.Object, serializer.Object);
            messageHandler.JwtToken = "DummyToken";
            var data = new SingleValueTestData()
            {
                Field = "test"
            };
            await messageHandler.PostAsync<SingleValueTestData>("authtest", data);

            Assert.That(requestMessage.Method, Is.EqualTo(HttpMethod.Post));
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
            serializer.Setup(x => x.Serialize<SingleValueTestData>(It.IsAny<Stream>(), It.IsAny<SingleValueTestData>()));

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", httpHandler.Object, serializer.Object);
            messageHandler.JwtToken = "DummyToken";

            var data = new SingleValueTestData()
            {
                Field = "test"
            };
            Assert.That(async () => await messageHandler.PostAsync<SingleValueTestData>("authtest", data), 
                Throws.TypeOf<RestException>().With.Property("StatusCode").EqualTo(HttpStatusCode.Forbidden));
        }

        [TestCase]
        public void NotFound()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.NotFound);
            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Serialize<SingleValueTestData>(It.IsAny<Stream>(), It.IsAny<SingleValueTestData>()));

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", httpHandler.Object, serializer.Object);
            messageHandler.JwtToken = "DummyToken";

            var data = new SingleValueTestData()
            {
                Field = "test"
            };
            Assert.That(async () => await messageHandler.PostAsync<SingleValueTestData>("authtest", data),
                Throws.TypeOf<RestException>().With.Property("StatusCode").EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase]
        public async Task SerializeObjectWhenSending()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            HttpRequestMessage requestMessage = null;
            var httpHandler = mockRepository.CreateJsonMessageHandler("{}", x => requestMessage = x);

            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Serialize<SingleValueTestData>(It.IsAny<Stream>(), It.Is<SingleValueTestData>(x => x.Field == "Hello"))).Verifiable();

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", httpHandler.Object, serializer.Object);

            var data = new SingleValueTestData()
            {
                Field = "Hello"
            };
            await messageHandler.PostAsync<SingleValueTestData>("standardtypes", data);

            Assert.That(requestMessage.Content.Headers.ContentType.MediaType, Is.EqualTo("application/json"));

            mockRepository.Verify();
        }

        [TestCase]
        public async Task SerializeObjectWhenReceiving()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var httpHandler = mockRepository.CreateJsonMessageHandler("{}");

            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Serialize<SingleValueTestData>(It.IsAny<Stream>(), It.Is<SingleValueTestData>(x => x.Field == "Hello"))).Verifiable();
            serializer.Setup(x => x.Deserialize<StandardTypesTestData>(It.IsAny<Stream>())).Returns(new StandardTypesTestData() { Integer = 5, String = "World" }).Verifiable();

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", httpHandler.Object, serializer.Object);

            var data = new SingleValueTestData()
            {
                Field = "Hello"
            };
            var result = await messageHandler.PostAsync<StandardTypesTestData, SingleValueTestData>("standardtypes", data);

            Assert.Multiple(() =>
            {
                Assert.That(result.Integer, Is.EqualTo(5));
                Assert.That(result.String, Is.EqualTo("World"));
            });


            mockRepository.Verify();
        }

    }

}
