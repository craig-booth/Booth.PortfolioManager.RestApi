using System;
using System.Collections.Generic;
using System.Text;

namespace Booth.PortfolioManager.RestApi.CorporateActions
{
    public class SplitConsolidation : CorporateAction
    {
        public override CorporateActionType Type => CorporateActionType.SplitConsolidation;
        public int OriginalUnits { get; set; }
        public int NewUnits { get; set; }
    }
}
