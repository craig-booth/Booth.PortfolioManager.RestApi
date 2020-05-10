using System;
using System.Collections.Generic;
using System.Text;


namespace Booth.PortfolioManager.RestApi.Portfolios
{
    public class ChangeDrpParticipationCommand
    {
        public Guid Holding { get; set; }
        public bool Participate { get; set; }
    }
}
