﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Booth.PortfolioManager.RestApi.Transactions
{
    public enum CgtCalculationMethod { MinimizeGain, MaximizeGain, FirstInFirstOut, LastInFirstOut }
    public class Disposal : Transaction
    {
        public override TransactionType Type => TransactionType.Disposal;
        public int Units { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TransactionCosts { get; set; }
        public CgtCalculationMethod CgtMethod { get; set; }
        public bool CreateCashTransaction { get; set; }
    } 
}
