﻿using System;
using System.Collections.Generic;
using System.Text;

using Booth.PortfolioManager.RestApi.Stocks;

namespace Booth.PortfolioManager.RestApi.Portfolios
{
    public class Stock
    {
        public Guid Id { get; set; }
        public string AsxCode { get; set; }
        public string Name { get; set; }
        public AssetCategory Category { get; set; }
    }
}
