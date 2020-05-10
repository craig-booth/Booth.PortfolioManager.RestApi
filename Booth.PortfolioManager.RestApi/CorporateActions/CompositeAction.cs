﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Booth.PortfolioManager.RestApi.CorporateActions
{
    public class CompositeAction : CorporateAction
    {
        public override CorporateActionType Type => CorporateActionType.CompositeAction;
        public List<CorporateAction> ChildActions { get; } = new List<CorporateAction>();
    }
}
