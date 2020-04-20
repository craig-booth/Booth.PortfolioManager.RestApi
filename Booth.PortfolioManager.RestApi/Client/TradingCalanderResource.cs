using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booth.PortfolioManager.RestApi.Client
{
    public class TradingCalanderResource
    {
        private readonly IRestClientMessageHandler _MessageHandler;

        public TradingCalanderResource(IRestClientMessageHandler messageHandler)
        {
            _MessageHandler = messageHandler;
        }
/*
        public async Task<TradingCalanderResponse> Get(int year)
        {
            return await GetAsync<TradingCalanderResponse>("/api/v2/tradingcalander/" + year.ToString());
        }

        public async Task Update(UpdateTradingCalanderCommand command)
        {
            await PostAsync<UpdateTradingCalanderCommand>("/api/v2/tradingcalander/" + command.Year.ToString(), command);
        } */
    }
}
