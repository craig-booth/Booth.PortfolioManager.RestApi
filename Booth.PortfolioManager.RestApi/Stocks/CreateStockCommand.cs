using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Booth.Common;

namespace Booth.PortfolioManager.RestApi.Stocks
{
    public enum AssetCategory { AustralianStocks, InternationalStocks, AustralianProperty, InternationalProperty, AustralianFixedInterest, InternationlFixedInterest, Cash }
    public class CreateStockCommand
    {
        public Guid Id { get; set; }
        public Date ListingDate { get; set; }
        public string AsxCode { get; set; }
        public string Name { get; set; }
        public bool Trust { get; set; }
        public AssetCategory Category { get; set; }
        public List<StapledSecurityChild> ChildSecurities { get; } = new List<StapledSecurityChild>();

        public void AddChildSecurity(string asxCode, string name, bool trust)
        {
            var child = new StapledSecurityChild()
            {
                ASXCode = asxCode,
                Name = name,
                Trust = trust
            };
            ChildSecurities.Add(child);
        }
        public class StapledSecurityChild
        {
            public string ASXCode { get; set; }
            public string Name { get; set; }
            public bool Trust { get; set; }
        } 
    }

}
