using System;
using System.Threading.Tasks;
using System.Net;
using System.Security;

using Xunit;
using Moq;

using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.RestApi.Users;


namespace Booth.PortfolioManager.RestApi.Test.Users
{
   
    class UserResourceTests
    {/*
        [TestCase]
        public async Task SuccessfullAuthentication()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupSet(x => x.JwtToken = It.IsAny<string>());
            messageHandler.Setup(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(It.IsAny<string>(), It.IsAny<AuthenticationRequest>()))
                .Returns(Task<AuthenticationResponse>.FromResult(new AuthenticationResponse() { Token = "valid" }));

            var resource = new UserResource(messageHandler.Object);

            var password = new SecureString();
            password.AppendChar('S');
            password.AppendChar('e');
            password.AppendChar('c');
            password.AppendChar('r');
            password.AppendChar('e');
            password.AppendChar('t');
            await resource.Authenticate("JoeBlogs", password);

            messageHandler.Verify(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(
                It.Is<string>(x => x == "users/authenticate"), 
                It.Is<AuthenticationRequest>(x => x.UserName == "JoeBlogs" && x.Password == "Secret")));
            messageHandler.VerifySet(x => x.JwtToken = "valid");   
        }

        [TestCase]
        public void FailedAuthentication()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupSet(x => x.JwtToken = It.IsAny<string>());
            messageHandler.Setup(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(It.IsAny<string>(), It.IsAny<AuthenticationRequest>()))
                .Throws(new RestException(HttpStatusCode.Forbidden, "Forbidden"));

            var resource = new UserResource(messageHandler.Object);

            var password = new SecureString();
            password.AppendChar('S');
            password.AppendChar('e');
            password.AppendChar('c');
            password.AppendChar('r');
            password.AppendChar('e');
            password.AppendChar('t');

            Assert.That(async() => await resource.Authenticate("JoeBlogs", password), Throws.TypeOf<RestException>());

            messageHandler.Verify(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(
                It.Is<string>(x => x == "users/authenticate"),
                It.Is<AuthenticationRequest>(x => x.UserName == "JoeBlogs" && x.Password == "Secret")));
            messageHandler.VerifySet(x => x.JwtToken = null);
        }

        [TestCase]
        public async Task SuccessfullAuthenticateWhenAlreadyAuthenticated()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.JwtToken, "oldtoken");
            messageHandler.Setup(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(It.IsAny<string>(), It.IsAny<AuthenticationRequest>()))
                .Returns(Task<AuthenticationResponse>.FromResult(new AuthenticationResponse() { Token = "valid" }));

            var resource = new UserResource(messageHandler.Object);

            var password = new SecureString();
            password.AppendChar('S');
            password.AppendChar('e');
            password.AppendChar('c');
            password.AppendChar('r');
            password.AppendChar('e');
            password.AppendChar('t');
            await resource.Authenticate("JoeBlogs", password);

            messageHandler.Verify(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(
                It.Is<string>(x => x == "users/authenticate"),
                It.Is<AuthenticationRequest>(x => x.UserName == "JoeBlogs" && x.Password == "Secret")));
            messageHandler.VerifySet(x => x.JwtToken = null);
            messageHandler.VerifySet(x => x.JwtToken = "valid");
        }

        [TestCase]
        public void FailedAuthenticateWhenAlreadyAuthenticated()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var messageHandler = mockRepository.Create<IRestClientMessageHandler>();
            messageHandler.SetupProperty(x => x.JwtToken, "oldtoken");
            messageHandler.Setup(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(It.IsAny<string>(), It.IsAny<AuthenticationRequest>()))
                .Throws(new RestException(HttpStatusCode.Forbidden, "Forbidden"));

            var resource = new UserResource(messageHandler.Object);

            var password = new SecureString();
            password.AppendChar('S');
            password.AppendChar('e');
            password.AppendChar('c');
            password.AppendChar('r');
            password.AppendChar('e');
            password.AppendChar('t');

            Assert.That(async () => await resource.Authenticate("JoeBlogs", password), Throws.TypeOf<RestException>());

            messageHandler.Verify(x => x.PostAsync<AuthenticationResponse, AuthenticationRequest>(
                It.Is<string>(x => x == "users/authenticate"),
                It.Is<AuthenticationRequest>(x => x.UserName == "JoeBlogs" && x.Password == "Secret")));
            messageHandler.VerifySet(x => x.JwtToken = null);     
        } */
    }
}
