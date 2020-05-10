using System;
using System.Collections.Generic;
using System.Text;

using Booth.PortfolioManager.RestApi.Stocks;

namespace Booth.PortfolioManager.RestApi.Portfolios
{
    public enum ValueFrequency { Day, Week, Month };

    public class PortfolioValueResponse
    {
        public List<ClosingPrice> Values { get; } = new List<ClosingPrice>();   
    }
}
