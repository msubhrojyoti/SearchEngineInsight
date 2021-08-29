using HtmlParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper;

namespace SearchEngineInsightApp
{
    public class GoogleScrapeWrapper
    {
        private readonly GoogleWebScraper _scraper;
        private const string DIV_IDENTIFIER = "div class=\"kCrYT\"><a";
        private const string CLASS_INDENTIFIER = "BNeawe UPmit AP7Wnd";

        public GoogleScrapeWrapper(IHttpHandler httpHandler)
        {
            _scraper = new GoogleWebScraper(httpHandler);
        }
        public virtual List<string> StartScraping(string server, string keywords, int limit, string subject, out string[] ranking)
        {
            var result = _scraper.Scrape(server, keywords, limit);
            List<string> items = new List<string>();
            List<string> ranks = new List<string>();
            int rank = 0;
            foreach (var x in FilterSearch(result, subject))
            {
                rank++;
                items.Add(x.Item1);
                if (x.Item2)
                {
                    ranks.Add($"{rank}");
                }
            }

            ranking = ranks.ToArray();
            return items;
        }

        protected virtual IEnumerable<Tuple<string, bool>> FilterSearch(string content, string key)
        {
            int serialNo = 0;
            foreach (var index in XmlParserUtility.FindOccurences(content, DIV_IDENTIFIER))
            {
                var block = XmlParserUtility.ParseFirstCompleteXmlNode(content.Substring(index), 0);
                yield return Tuple.Create(
                    $"{++serialNo}. {XmlParserUtility.GetFirstXmlNodeValue(block, "div", "class", CLASS_INDENTIFIER)}",
                    block.Contains(key, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
