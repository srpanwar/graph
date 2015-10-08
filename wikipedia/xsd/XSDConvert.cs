using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace org.wikipedia.www.xsd
{
    public class XSDConvert
    {
        private static XSDConvert instance = null;
        private XSDConvert() { }

        /// <summary>
        /// 
        /// </summary>
        public static XSDConvert Instance
        {
            get
            {
                if (instance == null)
                    instance = new XSDConvert();
                return instance;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public String ConvertToHtml(String text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;
            //Cheat: Rather than processing the whole article,
            //       thought it is earier to fetch html page.
            
            /*Regex regex = new Regex(@"\<\!DOCTYPE(.+?)\>(\r{0,1})(\n{0,1})");
            text = regex.Replace(text, this.MatchEvaluatorSimple);
            XDocument doc = XDocument.Load(XmlReader.Create(new MemoryStream(System.Convert.ToByte(text))));

            IEnumerable<XElement> elements = doc.Descendants(XName.Get("div", String.Empty));
            */
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaWiki"></param>
        /// <returns></returns>
        public String[] Convert(mediawiki mediaWiki)
        {
            List<String> arParts = new List<string>();
            if (mediaWiki == null)
                return null;

            arParts.Add(mediaWiki.page.title);
            arParts.Add(String.Format("http://en.wikipedia.org/wiki/{0}", mediaWiki.page.title));
            arParts.Add(mediaWiki.page.revision.text.Value);

            String text = mediaWiki.page.revision.text.Value;
            //Remove all the comments
            Regex regex = new Regex(@"<{1}.*?>{1}");
            text = regex.Replace(text, this.MatchEvaluatorSimple);
            
            //Process only the Introduction to save time and space
            int firstHeaderIndex = text.IndexOf("==");
            if (firstHeaderIndex != -1)
                text = text.Substring(0, firstHeaderIndex);

            //Remove all the special text
            regex = new Regex(@"{{([^{{])+?}}");
            while (regex.IsMatch(text))
                text = regex.Replace(text, this.MatchEvaluatorSimple);

            //Remove all the unneccesary text like "(, )"
            regex = new Regex(@"\(([^a..zA..Z0..9])+?\)");
            while (regex.IsMatch(text))
                text = regex.Replace(text, this.MatchEvaluatorSimple);

            //Simplify all the link text
            regex = new Regex(@"\[\[([^\|\]]+)(\|{0,1})([^\|\]]+)\]\](<nowiki>(.+)?</nowiki>){0,1}");
            text = regex.Replace(text, this.MatchEvaluator);
            text = text.Replace("'''", "");
            text = text.Replace("''", "");
            text = text.Substring(0, text.Length > 500 ? 500 : text.Length - 1);
            text += ".....";
            arParts.Add(text);
            return arParts.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private string MatchEvaluatorSimple(Match match)
        {
            return String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private string MatchEvaluator(Match match)
        {
            String topic = String.Empty;
            String link = "<a href='{0}'>{1}</a>";
           
            if (match.Groups.Count < 6)
                return match.Value;

            if (String.IsNullOrEmpty(match.Groups[2].Value))
            {
                topic = String.Concat(match.Groups[1].Value, match.Groups[3].Value);//.Replace(' ', '_');
                link = String.Format(link, topic, topic);
            }
            else
            {
                topic = match.Groups[1].Value;//.Replace(' ', '_');
                if (String.IsNullOrEmpty(match.Groups[3].Value))
                    link = String.Format(link, topic, topic);
                else
                    link = String.Format(link, topic, match.Groups[3].Value);
            }

            return topic;
        }
    }


}

//{{ }}
//#Redirect [[Wikipedia:How to edit a page]] 
//[[#Links and URLs]]
//[[kingdom (biology)|]]
//[[Name of page that does not exist yet]]
//http://en.wikipedia.org/w/index.php?title=Nakajo_syndrome&action=edit&redlink=1
//[[:Category:Character sets]]