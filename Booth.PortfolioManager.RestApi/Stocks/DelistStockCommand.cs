using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Booth.Common;

namespace Booth.PortfolioManager.RestApi.Stocks
{
    public class DelistStockCommand
    {
        public Guid Id { get; set; }
        public Date DelistingDate { get; set; }
    }
}
