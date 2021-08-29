using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace HtmlParser
{
    public enum HtmlTagParsingState
    {
        IDLE,
        PROCESSING,
        FINALISING,
    }

    public class XmlParserUtility
    {
        public static IEnumerable<int> FindOccurences(string input, string key)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be empty.");
            }

            for (int index = 0; ; index += key.Length)
            {
                index = input.IndexOf(key, index);
                if (index == -1)
                {
                    yield break;
                }
                yield return index;
            }
        }

        public static string GetFirstXmlNodeValue(string xmlData, string xmlElementName, string attributeName, string key)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);
            var node = doc.SelectSingleNode($"//{xmlElementName}[@{attributeName}='{key}']");
            if (null == node)
            {
                throw new ArgumentException(
                    $"Cannot find attribute '{attributeName}' with value '{key}' in '{xmlElementName}' any XML node.");
            }

            return node.InnerText;
        }

        public static string ParseFirstCompleteXmlNode(string input, int startIndex)
        {
            input = CleanContent(input);
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be empty.");
            }

            if (startIndex < 0)
            {
                throw new ArgumentException("Index cannot be less than 0.");
            }

            var state = HtmlTagParsingState.IDLE;
            StringBuilder builder = new();
            int tagTracker = 0;
            char last = '0';
            foreach (char current in input.Substring(startIndex))
            {
                switch (state)
                {
                    case HtmlTagParsingState.IDLE:
                        {
                            /*when IDLE state, start ignoring any characters other than '<',
                             * which marks start of HTML tag*/
                            if (current == '<')
                            {
                                builder.Append(current);
                                tagTracker++;
                                state = HtmlTagParsingState.PROCESSING;
                            }
                            break;
                        }
                    case HtmlTagParsingState.FINALISING:
                        {
                            builder.Append(current);
                            if (current == '>')
                            {
                                return builder.ToString();
                            }
                        }
                        break;
                    case HtmlTagParsingState.PROCESSING:
                        {
                            builder.Append(current);
                            if (current == '>' && last == '/')
                            {
                                tagTracker--;
                                if (tagTracker == 0)
                                {
                                    return builder.ToString();
                                }
                            }
                            else if (current == '/' && last == '<')
                            {
                                tagTracker -= 2;
                                if (tagTracker == 0)
                                {
                                    state = HtmlTagParsingState.FINALISING;
                                }
                            }
                            else if (current == '<')
                            {
                                tagTracker++;
                            }

                            break;
                        }
                    default:
                        throw new InvalidOperationException($"Invalid state '{state}'");
                }
                if (current != ' ')
                {
                    last = current;
                }
            }

            throw new InvalidDataException($"Failed to find valid HTML tag.");
        }

        private static string CleanContent(string input)
        {
            string[] emptyTags = new string[] 
            {
                "<area>",
                "<base>",
                "<br>",
                "<col>",
                "<embed>",
                "<hr>",
                "<img>",
                "<input>",
                "<keygen>",
                "<link>",
                "<meta>",
                "<param>",
                "<source>",
                "<track>",
                "<wbr>"
            };
            foreach(var item in emptyTags)
            {
                string newItem= item.Insert(item.LastIndexOf(">"),"/");
                input = input.Replace(item, newItem);
            }

            return input;
        }
    }
}
