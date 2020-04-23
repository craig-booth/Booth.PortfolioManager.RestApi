using System;
using System.Collections.Generic;
using System.Text;

namespace Booth.PortfolioManager.RestApi.Transactions
{
    public class Disposal : Transaction
    {
        public override string Type => "disposal";
        public int Units { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TransactionCosts { get; set; }
        public string CGTMethod { get; set; }
        public bool CreateCashTransaction { get; set; }
    } 
}
