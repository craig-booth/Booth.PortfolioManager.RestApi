using System;
using System.Text;
using System.Net;


namespace Booth.PortfolioManager.RestApi.Client
{
    public class RestException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public RestException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
