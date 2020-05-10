using System;
using System.Collections.Generic;
using System.Text;

using Booth.Common;

namespace Booth.PortfolioManager.RestApi.CorporateActions
{
    public class Dividend : CorporateAction
    {
        public override CorporateActionType Type => CorporateActionType.Dividend;
        public Date PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public decimal PercentFranked { get; set; }
        public decimal DrpPrice { get; set; }
    }
}
