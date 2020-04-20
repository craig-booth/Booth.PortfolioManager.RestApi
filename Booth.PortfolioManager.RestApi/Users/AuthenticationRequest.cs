using System;
using System.Collections.Generic;
using System.Text;

namespace Booth.PortfolioManager.RestApi.Users
{
    public class AuthenticationRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
