using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Portfolios;

namespace Booth.PortfolioManager.RestApi.Client
{
    public class HoldingResource
    {
        private readonly IRestClientMessageHandler _MessageHandler;

        public HoldingResource(IRestClientMessageHandler messageHandler)
        {
            _MessageHandler = messageHandler;
        }

        public async Task<List<Holding>> Get(Date date)
        {
            var url = "/api/v2/portfolio/" + _MessageHandler.Portfolio + "/holdings?date=" + date.ToIsoDateString();

            return await _MessageHandler.GetAsync<List<Holding>>(url);

        }
        public async Task<List<Holding>> Get(DateRange dateRange)
        {
            var url = "/api/v2/portfolio/" + _MessageHandler.Portfolio + "/holdings?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString();

            return await _MessageHandler.GetAsync<List<Holding>>(url);
        }

        public async Task<Holding> Get(Guid stockId, Date date)
        {
            var url = "/api/v2/portfolio/" + _MessageHandler.Portfolio + "/holdings/" + stockId + "?date=" + date.ToIsoDateString();

            return await _MessageHandler.GetAsync<Holding>(url);
        }

        public async Task<PortfolioValueResponse> GetValue(Guid stockId, DateRange dateRange, ValueFrequency frequency)
        {
            var url = "/api/v2/portfolio/" + _MessageHandler.Portfolio + "/holdings/" + stockId + "/value?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString() + "&frequency=" + frequency.ToString();

            return await _MessageHandler.GetAsync<PortfolioValueResponse>(url);
        }

        public async Task<TransactionsResponse> GetTransactions(Guid stockId, DateRange dateRange)
        {
            return await _MessageHandler.GetAsync<TransactionsResponse>("/api/v2/portfolio/" + _MessageHandler.Portfolio + "/holdings/" + stockId + "/transactions?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString());
        }

        public async Task<SimpleUnrealisedGainsResponse> GetCapitalGains(Guid stockId, Date date)
        {
            return await _MessageHandler.GetAsync<SimpleUnrealisedGainsResponse>("/api/v2/portfolio/" + _MessageHandler.Portfolio + "/holdings/" + stockId + "/capitalgains?date=" + date.ToIsoDateString());
        }

        public async Task<DetailedUnrealisedGainsResponse> GetDetailedCapitalGains(Guid stockId, Date date)
        {
            return await _MessageHandler.GetAsync<DetailedUnrealisedGainsResponse>("/api/v2/portfolio/" + _MessageHandler.Portfolio + "/holdings/" + stockId + "/detailedcapitalgains?date=" + date.ToIsoDateString());
        }

        public async Task<CorporateActionsResponse> GetCorporateActions(Guid stockId)
        {
            return await _MessageHandler.GetAsync<CorporateActionsResponse>("/api/v2/portfolio/" + _MessageHandler.Portfolio + "/holdings/" + stockId + "/corporateactions");
        }

        public async Task ChangeDrpParticipation(Guid stockId, bool participate)
        {
            var command = new ChangeDrpParticipationCommand()
            {
                Holding = stockId,
                Participate = participate
            };
            await _MessageHandler.PostAsync<ChangeDrpParticipationCommand>("/api/v2/portfolio/" + _MessageHandler.Portfolio + "/holdings/" + stockId + "/changedrpparticipation", command);
        } 
    }
}
