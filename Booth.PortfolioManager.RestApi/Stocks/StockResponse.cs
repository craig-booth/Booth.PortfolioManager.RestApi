using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Booth.Common;

namespace Booth.PortfolioManager.RestApi.Stocks
{
    public enum DrpMethod { Round, RoundDown, RoundUp, RetainCashBalance }
    public class StockResponse
    {
        public Guid Id { get; set; }
        public string AsxCode { get; set; }
        public string Name { get; set; }

        public AssetCategory Category { get; set; }
        public bool Trust { get; set; }
        public bool StapledSecurity { get; set; }

        public Date ListingDate { get; set; }
        public Date DelistedDate { get; set; }
        public decimal LastPrice { get; set; }
        public decimal CompanyTaxRate { get; set; }
        public RoundingRule DividendRoundingRule { get; set; }
        public bool DrpActive { get; set; }
        public DrpMethod DrpMethod { get; set; }
        public List<StapledSecurityChild> ChildSecurities { get; } = new List<StapledSecurityChild>();
        
        public void AddChild(string asxCode, string name, bool trust)
        {
            var child = new StapledSecurityChild()
            {
                AsxCode = asxCode,
                Name = name,
                Trust = trust
            };
            ChildSecurities.Add(child);
        }

        public class StapledSecurityChild
        {
            public string AsxCode { get; set; }
            public string Name { get; set; }
            public bool Trust { get; set; }
        }
    }
}
