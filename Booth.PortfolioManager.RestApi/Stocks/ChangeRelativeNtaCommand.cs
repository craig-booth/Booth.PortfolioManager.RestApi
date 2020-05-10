using System;
using System.Collections.Generic;
using System.Text;

using Booth.Common;

namespace Booth.PortfolioManager.RestApi.Stocks
{
    public class ChangeRelativeNtaCommand
    {
        public Guid Id { get; set; }
        public Date ChangeDate { get; set; }
        public List<RelativeNta> RelativeNtas { get; set; } = new List<RelativeNta>();

        public void AddRelativeNta(string childSecurity, decimal percentage)
        {
            var nta = new RelativeNta()
            {
                ChildSecurity = childSecurity,
                Percentage = percentage
            };
            RelativeNtas.Add(nta);
        }

        public class RelativeNta
        {
            public string ChildSecurity { get; set; }
            public decimal Percentage { get; set; }
        }
    }
}
