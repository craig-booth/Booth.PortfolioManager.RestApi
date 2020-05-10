using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Booth.PortfolioManager.RestApi.TradingCalendars;

namespace Booth.PortfolioManager.RestApi.Client
{
    public class TradingCalandarResource
    {
        private readonly IRestClientMessageHandler _MessageHandler;

        public TradingCalandarResource(IRestClientMessageHandler messageHandler)
        {
            _MessageHandler = messageHandler;
        }

        public async Task<TradingCalendar> Get(int year)
        {
            return await _MessageHandler.GetAsync<TradingCalendar>("tradingcalendars/" + year.ToString());
        }

        public async Task Update(TradingCalendar calendar)
        {
            await _MessageHandler.PostAsync<TradingCalendar>("tradingcalendars/" + calendar.Year.ToString(), calendar);
        } 
    }
}
