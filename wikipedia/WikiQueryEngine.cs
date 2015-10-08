using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Security;

using org.wikipedia.www.xsd;

namespace org.wikipedia.www
{
    public class WikiQueryEngine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        public SearchSuggestion QueryServerSearch(string action, string topic)
        {
            String endpoint = String.Format("http://en.wikipedia.org/w/api.php?action={0}&search={1}&format=xml",action, topic);
            SearchSuggestion searchSuggest = ProcessSearchQuery(endpoint);
            return searchSuggest;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="topic"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public SearchSuggestion QueryServerSearch(string action, string topic, int count)
        {
            topic = System.Web.HttpUtility.UrlEncode(topic);
            String endpoint = String.Format("http://en.wikipedia.org/w/api.php?action={0}&search={1}&format=xml&limit={2}", action, topic, count);
            SearchSuggestion searchSuggest = ProcessSearchQuery(endpoint);
            return searchSuggest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        private static SearchSuggestion ProcessSearchQuery(String endpoint)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            request.UserAgent = "IE";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(SearchSuggestion));
            SearchSuggestion searchSuggest = (SearchSuggestion)serializer.Deserialize(response.GetResponseStream());
            return searchSuggest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="topic"></param>
        /// <param name="convertor"></param>
        /// <returns></returns>
        public mediawiki QueryServerArticle(string action, string topic)
        {
            String endpoint = String.Format("http://en.wikipedia.org/w/api.php?action={0}&titles={1}&export&exportnowrap", action, topic);
            mediawiki mediaWiki = ProcessArticleQuery(endpoint);
            return mediaWiki;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        private mediawiki ProcessArticleQuery(string endpoint)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(mediawiki));
            mediawiki mediaWiki = (mediawiki)serializer.Deserialize(response.GetResponseStream());
            return mediaWiki;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        internal String QueryServerArticleHtml(string endpoint)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using(StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
