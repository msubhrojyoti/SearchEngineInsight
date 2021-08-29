using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    public class HttpHandler : IHttpHandler
    {
        /// <inheritdoc/>
        public HttpStatusCode ExecuteHttpGet(
            string server,
            string uri,
            List<Tuple<string, object, ParameterType>> paramList,
            out string responseContent)
        {
            RestClient client = new RestClient(server);
            RestRequest request = new RestRequest(uri, Method.GET);

            paramList?.ForEach(x => request.AddParameter(
                x.Item1,
                x.Item2,
                x.Item3));

            IRestResponse response = client.Execute(request);
            responseContent = response.Content;
            return response.StatusCode;
        }
    }
}
