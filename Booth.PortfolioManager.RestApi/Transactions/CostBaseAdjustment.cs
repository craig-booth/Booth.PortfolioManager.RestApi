using System;
using System.Collections.Generic;
using System.Text;

namespace Booth.PortfolioManager.RestApi.Transactions
{
    public class CostBaseAdjustment : Transaction
    {
        public override string Type => "costbaseadjustment";
        public decimal Percentage { get; set; }
    } 
}
