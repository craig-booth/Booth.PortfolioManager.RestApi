using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Stocks;

namespace Booth.PortfolioManager.RestApi.Client
{
    public class StockResource
    {
        private readonly IRestClientMessageHandler _MessageHandler;

        public StockResource(IRestClientMessageHandler messageHandler)
        {
            _MessageHandler = messageHandler;
        }

        public async Task<List<StockResponse>> Get()
        {
             return await _MessageHandler.GetAsync<List<StockResponse>>("stocks");
        }

        public async Task<List<StockResponse>> Get(Date date)
        {
            return await _MessageHandler.GetAsync<List<StockResponse>>("stocks?date=" + date.ToIsoDateString());
        }

        public async Task<List<StockResponse>> Get(DateRange dateRange)
        {
            return await _MessageHandler.GetAsync<List<StockResponse>>("stocks?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString());
        }

        public async Task<StockResponse> Get(Guid id)
        {
            return await _MessageHandler.GetAsync<StockResponse>("stocks/" + id.ToString());
        }

        public async Task<StockResponse> Get(Guid id, Date date)
        {
            return await _MessageHandler.GetAsync<StockResponse>("stocks/" + id.ToString() + "?date=" + date.ToIsoDateString());
        }

        public async Task<IEnumerable<StockResponse>> Find(string query)
        {
            return await _MessageHandler.GetAsync<List<StockResponse>>("stocks?query=" + query);
        }

        public async Task<List<StockResponse>> Find(string query, Date date)
        {
            return await _MessageHandler.GetAsync<List<StockResponse>>("stocks?query=" + query + "&date=" + date.ToIsoDateString());
        }

        public async Task<List<StockResponse>> Find(string query, DateRange dateRange)
        {
            return await _MessageHandler.GetAsync<List<StockResponse>>("stocks?query=" + query + "&fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString());
        }

        public async Task<StockHistoryResponse> GetHistory(Guid id)
        {
            return await _MessageHandler.GetAsync<StockHistoryResponse>("stocks/" + id.ToString() + "/history");
        }

        public async Task<StockPriceResponse> GetPrices(Guid id, DateRange dateRange)
        {
            return await _MessageHandler.GetAsync<StockPriceResponse>("stocks/" + id.ToString() + "/closingprices?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString());
        }

        public async Task CreateStock(CreateStockCommand command)
        {
            await _MessageHandler.PostAsync<CreateStockCommand>("stocks", command);
        }

        public async Task ChangeStock(ChangeStockCommand command)
        {
            await _MessageHandler.PostAsync<ChangeStockCommand>("stocks/" + command.Id.ToString() + "/change", command);
        }

        public async Task DelistStock(DelistStockCommand command)
        {
            await _MessageHandler.PostAsync<DelistStockCommand>("stocks/" + command.Id.ToString() + "/delist", command);
        }

        public async Task UpdateClosingPrices(UpdateClosingPricesCommand command)
        {
            await _MessageHandler.PostAsync<UpdateClosingPricesCommand>("stocks/" + command.Id.ToString() + "/closingprices", command);
        }

        public async Task ChangeDividendRules(ChangeDividendRulesCommand command)
        {
            await _MessageHandler.PostAsync<ChangeDividendRulesCommand>("stocks/" + command.Id.ToString() + "/changedividendrules", command);
        }

        public async Task ChangeRelativeNTAs(ChangeRelativeNtaCommand command)
        {
            await _MessageHandler.PostAsync<ChangeRelativeNtaCommand>("stocks/" + command.Id.ToString() + "/changerelativenta", command);
        } 
    }
}
