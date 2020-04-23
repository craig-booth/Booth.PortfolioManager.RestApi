using System;
using System.Collections.Generic;
using System.Text;

using Booth.Common;

namespace Booth.PortfolioManager.RestApi.Transactions
{
    public class OpeningBalance : Transaction
    {
        public override string Type => "openingbalance";
        public int Units { get; set; }
        public decimal CostBase { get; set; }
        public Date AquisitionDate { get; set; }
    } 
}
