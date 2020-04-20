using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Runtime.CompilerServices;

using Booth.PortfolioManager.RestApi.Serialization;

[assembly: InternalsVisibleToAttribute("Booth.PortfolioManager.RestApi.Test")]

namespace Booth.PortfolioManager.RestApi.Client
{
    public class RestClient
    {
        public RestClientMessageHandler MessageHandler { get; private set; }   
        public StockResource Stocks { get; }
        public TradingCalanderResource TradingCalander { get; }
        public CorporateActionResource CorporateActions { get; }
        public PortfolioResource Portfolio { get; }
        public HoldingResource Holdings { get; }
        public TransactionResource Transactions { get; }

        public RestClient(string baseURL)
        {
            MessageHandler = new RestClientMessageHandler(baseURL, new RestClientSerializer());

            Stocks = new StockResource(MessageHandler);
            TradingCalander = new TradingCalanderResource(MessageHandler);
            CorporateActions = new CorporateActionResource(MessageHandler);
            Portfolio = new PortfolioResource(MessageHandler);
            Holdings = new HoldingResource(MessageHandler);
            Transactions = new TransactionResource(MessageHandler);
        }

        public async Task Authenticate(string userName, SecureString password)
        {
            var userResource = new UserResource(MessageHandler);

            await userResource.Authenticate(userName, password);
        }

        public void SetPortfolio(Guid portfolio)
        {
            MessageHandler.Portfolio = portfolio;
        }
    }
}
