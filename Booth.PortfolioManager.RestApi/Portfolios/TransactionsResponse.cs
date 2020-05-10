﻿using System;
using System.Collections.Generic;
using System.Text;

using Booth.Common;

namespace Booth.PortfolioManager.RestApi.Portfolios
{
    public class TransactionsResponse
    {
        public List<TransactionItem> Transactions = new List<TransactionItem>();

        public class TransactionItem
        {
            public Guid Id { get; set; }
            public Stock Stock { get; set; }
            public Date TransactionDate { get; set; }
            public string Description { get; set; }
            public string Comment { get; set; }
        }
    }
}
