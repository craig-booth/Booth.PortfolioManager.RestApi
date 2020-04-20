using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Booth.PortfolioManager.RestApi.Client
{
    public class CorporateActionResource
    {
        private readonly IRestClientMessageHandler _MessageHandler;

        public CorporateActionResource(IRestClientMessageHandler messageHandler)
        {
            _MessageHandler = messageHandler;
        }
/*
        public async Task<IEnumerable<CorporateAction>> GetAll(Guid stockid)
        {
            return await GetAsync<IEnumerable<CorporateAction>>("/api/v2/stocks/" + stockid + "/corporateactions");
        }

        public async Task<IEnumerable<CorporateAction>> GetAll(Guid stockid, DateRange dateRange)
        {
            return await GetAsync<IEnumerable<CorporateAction>>("/api/v2/stocks/" + stockid + "/corporateactions?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString());
        }

        public async Task<CorporateAction> Get(Guid stockid, Guid id)
        {
            return await GetAsync<CorporateAction>("/api/v2/stocks/" + stockid + "/corporateactions/" + id);
        }

        public async Task Add(Guid stockid, CorporateAction corporateAction)
        {
            await PostAsync<CorporateAction>("/api/v2/stocks/" + stockid + "/corporateactions/", corporateAction);
        } */
    }
}
