using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    /// <summary>
    ///     Interface for executing HTTP REST requests
    /// </summary>
    public interface IHttpHandler
    {
        /// <summary>
        ///     Executes a HTTP GET REST request and returns the HTTP status and response content.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="uri"></param>
        /// <param name="paramList">List of tuples with format: (param name, param value, param type)</param>
        /// <param name="responseContent"></param>
        /// <returns>HTTP status code</returns>
        HttpStatusCode ExecuteHttpGet(
            string server,
            string uri,
            List<Tuple<string, object, ParameterType>> paramList,
            out string responseContent);
    }
}
