﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Booth.Common;

namespace Booth.PortfolioManager.RestApi.Stocks
{
    public class StockPriceResponse
    {
        public Guid Id { get; set; }
        public string AsxCode { get; set; }
        public string Name { get; set; }
        public List<ClosingPrice> ClosingPrices { get; } = new List<ClosingPrice>();

        public void AddClosingPrice(Date date, decimal price)
        {
            var closingPrice = new ClosingPrice()
            {
                Date = date,
                Price = price
            };
            ClosingPrices.Add(closingPrice);
        }
    }
}
