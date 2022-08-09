using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Booth.PortfolioManager.RestApi.Transactions;

namespace Booth.PortfolioManager.RestApi.Client
{
    public class TransactionResource
    {
        private readonly IRestClientMessageHandler _MessageHandler;

        public TransactionResource(IRestClientMessageHandler messageHandler)
        {
            _MessageHandler = messageHandler;
        }

        public async Task<Transaction> Get(Guid id)
        {
            var url = "portfolio/" + _MessageHandler.Portfolio + "/transactions/" + id;

            return await _MessageHandler.GetAsync<Transaction>(url);
        }

        public async Task Add(Transaction transaction)
        {
            var url = "portfolio/" + _MessageHandler.Portfolio + "/transactions";

            await _MessageHandler.PostAsync<Transaction>(url, transaction);       
        }

        public async Task Add(IEnumerable<Transaction> transactions)
        {
            var url = "portfolio/" + _MessageHandler.Portfolio + "/transactions";

            await _MessageHandler.PostAsync<IEnumerable<Transaction>>(url, transactions);
        }

        public async Task Update(Transaction transaction)
        {
            var url = "portfolio/" + _MessageHandler.Portfolio + "/transactions/" + transaction.Id;

            await _MessageHandler.PostAsync<Transaction>(url, transaction);
        }

        public async Task Delete(Guid id)
        {
            var url = "portfolio/" + _MessageHandler.Portfolio + "/transactions/" + id;

            await _MessageHandler.DeleteAsync(url);
        }

        public async Task<List<Transaction>> GetTransactionsForCorporateAction(Guid stock, Guid action)
        {
            return await _MessageHandler.GetAsync<List<Transaction>>("portfolio/" + _MessageHandler.Portfolio + "/transactions/" + stock.ToString() + "/corporateactions/" + action.ToString());
        }
        
    }
}
