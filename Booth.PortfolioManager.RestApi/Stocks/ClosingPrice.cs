﻿using System;
using System.Collections.Generic;
using System.Text;

using Booth.Common;

namespace Booth.PortfolioManager.RestApi.Stocks
{
    public struct ClosingPrice
    {
        public Date Date { get; set; }
        public decimal Price { get; set; }
    }
}
