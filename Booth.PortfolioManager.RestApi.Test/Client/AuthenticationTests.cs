using System;
using System.Threading.Tasks;
using System.Net;

using Xunit;
using FluentAssertions;
using Moq;

using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Users;


namespace Booth.PortfolioManager.RestApi.Test.Client
{
   
    public class AuthenticationTests
    {
        [Fact]
        public async Task SuccessfullAuthentication()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupSet(x => x.JwtToken = It.IsAny<string>());
            messageHandler.Setup(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(It.IsAny<string>(), It.IsAny<AuthenticationRequest>()))
                .Returns(Task<AuthenticationResponse>.FromResult(new AuthenticationResponse() { Token = "valid" }));

            var client = new RestClient(messageHandler.Object, "http://test.com");

            await client.Authenticate("JoeBlogs", "Secret");

            messageHandler.Verify(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(
                It.Is<string>(x => x == "authenticate"), 
                It.Is<AuthenticationRequest>(x => x.UserName == "JoeBlogs" && x.Password == "Secret")));
            messageHandler.VerifySet(x => x.JwtToken = "valid");   
        }

        [Fact]
        public void FailedAuthentication()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupSet(x => x.JwtToken = It.IsAny<string>());
            messageHandler.Setup(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(It.IsAny<string>(), It.IsAny<AuthenticationRequest>()))
                .Throws(new RestException(HttpStatusCode.Forbidden, "Forbidden"));

            var client = new RestClient(messageHandler.Object, "http://test.com");

            Func<Task> a = async () => await client.Authenticate("JoeBlogs", "Secret");
            a.Should().Throw<RestException>().Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            messageHandler.Verify(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(
                It.Is<string>(x => x == "authenticate"),
                It.Is<AuthenticationRequest>(x => x.UserName == "JoeBlogs" && x.Password == "Secret")));
            messageHandler.VerifySet(x => x.JwtToken = null);
        }

        [Fact]
        public async Task SuccessfullAuthenticateWhenAlreadyAuthenticated()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.JwtToken, "oldtoken");
            messageHandler.Setup(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(It.IsAny<string>(), It.IsAny<AuthenticationRequest>()))
                .Returns(Task<AuthenticationResponse>.FromResult(new AuthenticationResponse() { Token = "valid" }));

            var client = new RestClient(messageHandler.Object, "http://test.com");

            await client.Authenticate("JoeBlogs", "Secret");

            messageHandler.Verify(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(
                It.Is<string>(x => x == "authenticate"),
                It.Is<AuthenticationRequest>(x => x.UserName == "JoeBlogs" && x.Password == "Secret")));
            messageHandler.VerifySet(x => x.JwtToken = null);
            messageHandler.VerifySet(x => x.JwtToken = "valid");
        }

        [Fact]
        public void FailedAuthenticateWhenAlreadyAuthenticated()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.JwtToken, "oldtoken");
            messageHandler.Setup(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(It.IsAny<string>(), It.IsAny<AuthenticationRequest>()))
                .Throws(new RestException(HttpStatusCode.Forbidden, "Forbidden"));

            var client = new RestClient(messageHandler.Object, "http://test.com");

            Func<Task> a = async () => await client.Authenticate("JoeBlogs", "Secret");
            a.Should().Throw<RestException>().Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            messageHandler.Verify(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(
                It.Is<string>(x => x == "authenticate"),
                It.Is<AuthenticationRequest>(x => x.UserName == "JoeBlogs" && x.Password == "Secret")));
            messageHandler.VerifySet(x => x.JwtToken = null);     
        }

        [Fact]
        public async Task SignOutRemovesToken()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.JwtToken, "oldtoken");
            messageHandler.Setup(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(It.IsAny<string>(), It.IsAny<AuthenticationRequest>()))
                .Returns(Task<AuthenticationResponse>.FromResult(new AuthenticationResponse() { Token = "valid" }));

            var client = new RestClient(messageHandler.Object, "http://test.com");

            await client.Authenticate("JoeBlogs", "Secret");

            client.SignOut();

            messageHandler.Object.JwtToken.Should().BeNull();

            messageHandler.Verify(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(
                It.Is<string>(x => x == "authenticate"),
                It.Is<AuthenticationRequest>(x => x.UserName == "JoeBlogs" && x.Password == "Secret")));
            messageHandler.VerifySet(x => x.JwtToken = null);
            messageHandler.VerifySet(x => x.JwtToken = "valid");       
        }

        [Fact]
        public void SignOutIfNotAuthenticatedDoesNotHaveError()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.JwtToken, "oldtoken");

            var client = new RestClient(messageHandler.Object, "http://test.com");

            client.SignOut();

            messageHandler.Object.JwtToken.Should().BeNull();

            messageHandler.VerifySet(x => x.JwtToken = null);
        }

        [Fact]
        public async Task AuthenticateAfterSigningOut()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.JwtToken, "oldtoken");
            messageHandler.Setup(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(It.IsAny<string>(), It.IsAny<AuthenticationRequest>()))
                .Returns(Task<AuthenticationResponse>.FromResult(new AuthenticationResponse() { Token = "valid" }));

            var client = new RestClient(messageHandler.Object, "http://test.com");

            await client.Authenticate("JoeBlogs", "Secret");

            client.SignOut();

            await client.Authenticate("JoeBlogs2", "Secret");

            messageHandler.Verify(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(
                It.Is<string>(x => x == "authenticate"),
                It.Is<AuthenticationRequest>(x => x.UserName == "JoeBlogs" && x.Password == "Secret")));
            messageHandler.Verify(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(
                It.Is<string>(x => x == "authenticate"),
                It.Is<AuthenticationRequest>(x => x.UserName == "JoeBlogs2" && x.Password == "Secret")));
            messageHandler.VerifySet(x => x.JwtToken = null);
            messageHandler.VerifySet(x => x.JwtToken = "valid");
        }
    }
}
