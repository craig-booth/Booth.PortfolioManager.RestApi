using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security;

using Booth.PortfolioManager.RestApi.Users;

namespace Booth.PortfolioManager.RestApi.Client
{
    public class UserResource
    {
        private readonly IRestClientMessageHandler _MessageHandler;

        public UserResource(IRestClientMessageHandler messageHandler)
        {
            _MessageHandler = messageHandler;
        }

        public async Task Authenticate(string userName, SecureString password)
        {
            _MessageHandler.JwtToken = null;

            var request = new AuthenticationRequest()
            {
                UserName = userName,
                Password = new System.Net.NetworkCredential(string.Empty, password).Password
            };

            var response = await _MessageHandler.PostAsync<AuthenticationResponse, AuthenticationRequest>("users/authenticate", request);

            _MessageHandler.JwtToken = response.Token;
        } 
    }
}
