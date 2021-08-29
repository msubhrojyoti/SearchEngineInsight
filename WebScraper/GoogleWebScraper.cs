using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WebScraper
{
    public class GoogleWebScraper
    {
        private const string SEARCH_RESOURCE = "/search";
        private readonly IHttpHandler _reqHandler;

        public GoogleWebScraper(IHttpHandler handler)
        {
            _reqHandler = handler;
        }

        public string Scrape(string server, string searchKeyWords, int limit)
        {
            List<Tuple<string, object, ParameterType>> param = new List<Tuple<string, object, ParameterType>>()
            {
                Tuple.Create("num", (object)limit, ParameterType.QueryString),
                Tuple.Create("q", (object)searchKeyWords, ParameterType.QueryString)
            };
            var status = _reqHandler.ExecuteHttpGet(server, SEARCH_RESOURCE, param, out string result);
            if (status != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRequestException($"Failed to execute HTTP request. Status: '{status}'");
            }

            return result;
        }
    }
}
