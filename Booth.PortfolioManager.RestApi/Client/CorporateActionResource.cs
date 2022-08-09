using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Booth.Common;
using Booth.PortfolioManager.RestApi.CorporateActions;

namespace Booth.PortfolioManager.RestApi.Client
{
    public class CorporateActionResource
    {
        private readonly IRestClientMessageHandler _MessageHandler;

        public CorporateActionResource(IRestClientMessageHandler messageHandler)
        {
            _MessageHandler = messageHandler;
        }

        public async Task<List<CorporateAction>> GetAll(Guid stockId)
        {
            return await _MessageHandler.GetAsync<List<CorporateAction>>("stocks/" + stockId + "/corporateactions");
        }

        public async Task<List<CorporateAction>> GetAll(Guid stockId, DateRange dateRange)
        {
            return await _MessageHandler.GetAsync<List<CorporateAction>>("stocks/" + stockId + "/corporateactions?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString());
        }

        public async Task<CorporateAction> Get(Guid stockId, Guid id)
        {
            return await _MessageHandler.GetAsync<CorporateAction>("stocks/" + stockId + "/corporateactions/" + id);
        }

        public async Task Add(Guid stockId, CorporateAction corporateAction)
        {
            await _MessageHandler.PostAsync<CorporateAction>("stocks/" + stockId + "/corporateactions", corporateAction);
        }
        public async Task Update(Guid stockId, CorporateAction corporateAction)
        {
            await _MessageHandler.PostAsync<CorporateAction>("stocks/" + stockId + "/corporateactions/" + corporateAction.Id, corporateAction);
        }
        public async Task Delete(Guid stockId, Guid id)
        {
            await _MessageHandler.DeleteAsync("stocks/" + stockId + "/corporateactions/" + id);
        }
    }
}
