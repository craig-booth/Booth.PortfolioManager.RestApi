using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using Xunit;
using FluentAssertions;
using Moq;

using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Serialization;

using Booth.PortfolioManager.RestApi.Test.Serialization;


namespace Booth.PortfolioManager.RestApi.Test.Client
{
    
    public class RestClientMessageHandlerPostAsyncTests
    {
        [Fact]
        public async Task WithoutAuthentication()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            HttpRequestMessage requestMessage = null;
            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.OK, x => requestMessage = x);

            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Serialize<SingleValueTestData>(It.IsAny<StreamWriter>(), It.IsAny<SingleValueTestData>()));

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/", new HttpClient(httpHandler.Object), serializer.Object);

            var data = new SingleValueTestData()
            {
                Field = "test"
            };
            await messageHandler.PostAsync<SingleValueTestData>("authtest", data);

            requestMessage.Should().BeEquivalentTo(new { Method = HttpMethod.Post, RequestUri = new Uri("http://test.com.au/api/authtest") });
            requestMessage.Headers.Should().NotContain("Authorisation");
        }

        [Fact]
        public async Task WithAuthentication()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            HttpRequestMessage requestMessage = null;
            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.OK, x => requestMessage = x);

            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Serialize<SingleValueTestData>(It.IsAny<StreamWriter>(), It.IsAny<SingleValueTestData>()));

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/", new HttpClient(httpHandler.Object), serializer.Object);
            messageHandler.JwtToken = "DummyToken";
            var data = new SingleValueTestData()
            {
                Field = "test"
            };
            await messageHandler.PostAsync<SingleValueTestData>("authtest", data);

            requestMessage.Should().BeEquivalentTo(new { Method = HttpMethod.Post, RequestUri = new Uri("http://test.com.au/api/authtest") });
            requestMessage.Headers.Authorization.Should().BeEquivalentTo(new { Scheme = "Bearer", Parameter = "DummyToken" });
        }

        [Fact]
        public void AuthenticationFailed()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.Forbidden);
            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Serialize<SingleValueTestData>(It.IsAny<StreamWriter>(), It.IsAny<SingleValueTestData>()));

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/", new HttpClient(httpHandler.Object), serializer.Object);
            messageHandler.JwtToken = "DummyToken";

            var data = new SingleValueTestData()
            {
                Field = "test"
            };
            Func<Task> action = async () => await messageHandler.PostAsync<SingleValueTestData>("authtest", data);
            action.Should().ThrowExactly<RestException>().And.StatusCode.Equals(HttpStatusCode.Forbidden);
        }

        [Fact]
        public void NotFound()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.NotFound);
            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Serialize<SingleValueTestData>(It.IsAny<StreamWriter>(), It.IsAny<SingleValueTestData>()));

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/", new HttpClient(httpHandler.Object), serializer.Object);
            messageHandler.JwtToken = "DummyToken";

            var data = new SingleValueTestData()
            {
                Field = "test"
            };
            Func<Task> action = async () => await messageHandler.PostAsync<SingleValueTestData>("authtest", data);
            action.Should().ThrowExactly<RestException>().And.StatusCode.Equals(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SerializeObjectWhenSending()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            HttpRequestMessage requestMessage = null;
            var httpHandler = mockRepository.CreateJsonMessageHandler("{}", x => requestMessage = x);

            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Serialize<SingleValueTestData>(It.IsAny<StreamWriter>(), It.Is<SingleValueTestData>(x => x.Field == "Hello"))).Verifiable();

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/", new HttpClient(httpHandler.Object), serializer.Object);

            var data = new SingleValueTestData()
            {
                Field = "Hello"
            };
            await messageHandler.PostAsync<SingleValueTestData>("standardtypes", data);

            requestMessage.Content.Headers.ContentType.MediaType.Should().Be("application/json");

            mockRepository.Verify();
        }

        [Fact]
        public async Task SerializeObjectWhenReceiving()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var httpHandler = mockRepository.CreateJsonMessageHandler("{}");

            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Serialize<SingleValueTestData>(It.IsAny<StreamWriter>(), It.Is<SingleValueTestData>(x => x.Field == "Hello"))).Verifiable();
            serializer.Setup(x => x.Deserialize<StandardTypesTestData>(It.IsAny<StreamReader>())).Returns(new StandardTypesTestData() { Integer = 5, String = "World" }).Verifiable();

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/", new HttpClient(httpHandler.Object), serializer.Object);

            var data = new SingleValueTestData()
            {
                Field = "Hello"
            };
            var result = await messageHandler.PostAsync<StandardTypesTestData, SingleValueTestData>("standardtypes", data);

            result.Should().BeEquivalentTo(new { Integer = 5, String = "World"});


            mockRepository.Verify();
        }
        
    }

}
