using System;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;

using Xunit;
using FluentAssertions;
using Moq;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Serialization;
using Booth.PortfolioManager.RestApi.Test.Serialization;

namespace Booth.PortfolioManager.RestApi.Test
{
    public class RestClientMessageHandlerGetAsyncTests
    {
        [Fact]
        public async Task WithoutAuthentication()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            HttpRequestMessage requestMessage = null;
            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.OK, x => requestMessage = x);
            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Deserialize<Time>(It.IsAny<Stream>())).Returns(new Time());

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", new HttpClient(httpHandler.Object), serializer.Object);

            var result = await messageHandler.GetAsync<Time>("authtest");


            requestMessage.Should().BeEquivalentTo(new { Method = HttpMethod.Get, RequestUri = new Uri("http://test.com.au/api/v2/authtest") });
            requestMessage.Headers.Should().NotContain("Authorisation");
        }
        
        [Fact]
        public async Task WithAuthentication()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            HttpRequestMessage requestMessage = null;
            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.OK, x => requestMessage = x);
            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Deserialize<Time>(It.IsAny<Stream>())).Returns(new Time());

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", new HttpClient(httpHandler.Object), serializer.Object);
            messageHandler.JwtToken = "DummyToken";

            var result = await messageHandler.GetAsync<Time>("authtest");

            requestMessage.Should().BeEquivalentTo(new { Method = HttpMethod.Get, RequestUri = new Uri("http://test.com.au/api/v2/authtest") });
            requestMessage.Headers.Authorization.Should().BeEquivalentTo(new { Scheme = "Bearer", Parameter = "DummyToken" });
        }
        
        [Fact]
        public void AuthenticationFailed()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.Forbidden);
            var serializer = mockRepository.Create<IRestClientSerializer>();

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", new HttpClient(httpHandler.Object), serializer.Object);
            messageHandler.JwtToken = "DummyToken";

            Func<Task> action = async() => await messageHandler.GetAsync<SingleValueTestData>("authtest");
            action.Should().ThrowExactly<RestException>().Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
        
        [Fact]
        public void NotFound()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var httpHandler = mockRepository.CreateHttpStatusMessageHandler(HttpStatusCode.NotFound);
            var serializer = mockRepository.Create<IRestClientSerializer>();

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", new HttpClient(httpHandler.Object), serializer.Object);
            messageHandler.JwtToken = "DummyToken";

            Func<Task> action = async () => await messageHandler.GetAsync<SingleValueTestData>("authtest");
            action.Should().ThrowExactly<RestException>().Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task SerializeObjectWhenReceiving()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var httpHandler = mockRepository.CreateJsonMessageHandler("{Field : \"Hello\"}");

            var serializer = mockRepository.Create<IRestClientSerializer>();
            serializer.Setup(x => x.Deserialize<SingleValueTestData>(It.IsAny<Stream>())).Returns(new SingleValueTestData() { Field = "Hello" }).Verifiable();

            var messageHandler = new RestClientMessageHandler("http://test.com.au/api/v2/", new HttpClient(httpHandler.Object), serializer.Object);

            var data = await messageHandler.GetAsync<SingleValueTestData>("standardtypes");

            data.Field.Should().Be("Hello");

            mockRepository.Verify();
        }  
    }

}
