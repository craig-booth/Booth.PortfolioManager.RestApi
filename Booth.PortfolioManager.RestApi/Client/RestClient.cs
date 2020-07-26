using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Runtime.CompilerServices;

using Booth.PortfolioManager.RestApi.Users;
using Booth.PortfolioManager.RestApi.Serialization;

[assembly: InternalsVisibleToAttribute("Booth.PortfolioManager.RestApi.Test")]

namespace Booth.PortfolioManager.RestApi.Client
{
    public class RestClient
    {
        public IRestClientMessageHandler MessageHandler { get; private set; }   
        public StockResource Stocks { get; }
        public TradingCalandarResource TradingCalander { get; }
        public CorporateActionResource CorporateActions { get; }
        public PortfolioResource Portfolio { get; }
        public HoldingResource Holdings { get; }
        public TransactionResource Transactions { get; }

        public RestClient(string baseURL)
            : this(new HttpClient(), baseURL)
        {
        }

        public RestClient(HttpClient httpClient, string baseURL)
            : this(new RestClientMessageHandler(baseURL, httpClient, new RestClientSerializer()), baseURL)
        {
        }

        public RestClient(IRestClientMessageHandler messageHandler, string baseURL)
        {
            MessageHandler = messageHandler;

            Stocks = new StockResource(MessageHandler);
            TradingCalander = new TradingCalandarResource(MessageHandler);
            CorporateActions = new CorporateActionResource(MessageHandler);
            Portfolio = new PortfolioResource(MessageHandler);
            Holdings = new HoldingResource(MessageHandler);
            Transactions = new TransactionResource(MessageHandler);
        }

        public async Task Authenticate(string userName, SecureString password)
        {
            SignOut();

            var request = new AuthenticationRequest()
            {
                UserName = userName,
                Password = new System.Net.NetworkCredential(string.Empty, password).Password
            };

            var response = await MessageHandler.PostAsync<AuthenticationResponse, AuthenticationRequest>("authenticate", request);

            MessageHandler.JwtToken = response.Token;
        }

        public void SignOut()
        {
            MessageHandler.JwtToken = null;
        }

        public void SetPortfolio(Guid portfolio)
        {
            MessageHandler.Portfolio = portfolio;
        }
    }
}
