using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Booth.Common;
using Booth.PortfolioManager.RestApi.Portfolios;
using Booth.PortfolioManager.RestApi.Stocks;

namespace Booth.PortfolioManager.RestApi.Client
{

    public class PortfolioResource
    {
        private readonly IRestClientMessageHandler _MessageHandler;

        public PortfolioResource(IRestClientMessageHandler messageHandler)
        {
            _MessageHandler = messageHandler;
        }
        public async Task CreatePortfolio(CreatePortfolioCommand command)
        {
            await _MessageHandler.PostAsync<CreatePortfolioCommand>("portfolio", command);

            _MessageHandler.Portfolio = command.Id;
        }

        public async Task<PortfolioPropertiesResponse> GetProperties()
        {
            return await _MessageHandler.GetAsync<PortfolioPropertiesResponse>("portfolio/" + _MessageHandler.Portfolio + "/properties");
        }

        public async Task<PortfolioSummaryResponse> GetSummary(Date date)
        {
            return await _MessageHandler.GetAsync<PortfolioSummaryResponse>("portfolio/" + _MessageHandler.Portfolio + "/summary?date=" + date.ToIsoDateString());
        }

        public async Task<PortfolioPerformanceResponse> GetPerformance(DateRange dateRange)
        {
            return await _MessageHandler.GetAsync<PortfolioPerformanceResponse>("portfolio/" + _MessageHandler.Portfolio + "/performance?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString());
        }

        public async Task<PortfolioValueResponse> GetValue(DateRange dateRange, ValueFrequency frequency)
        {
            var url = "portfolio/" + _MessageHandler.Portfolio + "/value?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString() + "&frequency=" + frequency.ToString().ToLower();

            return await _MessageHandler.GetAsync<PortfolioValueResponse>(url);
        }

        public async Task<TransactionsResponse> GetTransactions(DateRange dateRange)
        {
            return await _MessageHandler.GetAsync<TransactionsResponse>("portfolio/" + _MessageHandler.Portfolio + "/transactions?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString());
        }

        public async Task<SimpleUnrealisedGainsResponse> GetCapitalGains(Date date)
        {
            return await _MessageHandler.GetAsync<SimpleUnrealisedGainsResponse>("portfolio/" + _MessageHandler.Portfolio + "/capitalgains?date=" + date.ToIsoDateString());
        }

        public async Task<DetailedUnrealisedGainsResponse> GetDetailedCapitalGains(Date date)
        {
            return await _MessageHandler.GetAsync<DetailedUnrealisedGainsResponse>("portfolio/" + _MessageHandler.Portfolio + "/detailedcapitalgains?date=" + date.ToIsoDateString());
        }

        public async Task<CgtLiabilityResponse> GetCGTLiability(DateRange dateRange)
        {
            return await _MessageHandler.GetAsync<CgtLiabilityResponse>("portfolio/" + _MessageHandler.Portfolio + "/cgtliability?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString());
        }

        public async Task<CashAccountTransactionsResponse> GetCashAccount(DateRange dateRange)
        {
            return await _MessageHandler.GetAsync<CashAccountTransactionsResponse>("portfolio/" + _MessageHandler.Portfolio + "/cashaccount?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString());
        }

        public async Task<IncomeResponse> GetIncome(DateRange dateRange)
        {
            return await _MessageHandler.GetAsync<IncomeResponse>("portfolio/" + _MessageHandler.Portfolio + "/income?fromdate=" + dateRange.FromDate.ToIsoDateString() + "&todate=" + dateRange.ToDate.ToIsoDateString());
        }

        public async Task<CorporateActionsResponse> GetCorporateActions()
        {
            return await _MessageHandler.GetAsync<CorporateActionsResponse>("portfolio/" + _MessageHandler.Portfolio + "/corporateactions");
        } 
    }
}
