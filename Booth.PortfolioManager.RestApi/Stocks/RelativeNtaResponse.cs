using System;
using System.Collections.Generic;
using System.Linq;

using Booth.Common;

namespace Booth.PortfolioManager.RestApi.Stocks
{
    public class RelativeNtaResponse
    {
        public Guid Id { get; set; }
        public string AsxCode { get; set; }
        public string Name { get; set; }
        public List<RelativeNta> RelativeNtas { get; } = new List<RelativeNta>();

        public void AddRelativeNta(Date fromDate, Date toDate, IEnumerable<ChildSecurityNta> relativeNtas)
        {
            var nta = new RelativeNta()
            {
                FromDate = fromDate,
                ToDate = toDate,
                RelativeNtas = relativeNtas.ToArray()
            };
            RelativeNtas.Add(nta);
        }

        public class RelativeNta
        {
            public Date FromDate { get; set; }
            public Date ToDate { get; set; }
            public ChildSecurityNta[] RelativeNtas { get; set; }
        }

        public struct ChildSecurityNta
        {
            public string ChildSecurity { get; set; }
            public decimal Percentage { get; set;  }

            public ChildSecurityNta(string childSecurity, decimal percentage)
            {
                ChildSecurity = childSecurity;
                Percentage = percentage;
            }

        }   
    }
}
