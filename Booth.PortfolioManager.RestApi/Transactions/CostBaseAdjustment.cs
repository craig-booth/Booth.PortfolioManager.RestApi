using System;
using System.Collections.Generic;
using System.Text;

namespace Booth.PortfolioManager.RestApi.Transactions
{
    public class CostBaseAdjustment : Transaction
    {
        public override TransactionType Type => TransactionType.CostBaseAdjustment;
        public decimal Percentage { get; set; }
    } 
}
