using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;

using com.yahoo.search.xsd;
using Omuk.OmukEngine;

namespace com.yahoo.search
{
    /// <summary>
    /// 
    /// </summary>
    public class YSearchItem
    {
        public string AbstractText { get; set; }

        public string ClickUrl { get; set; }

        public string DateField { get; set; }

        public string Language { get; set; }

        public string Source { get; set; }

        public string SourceUrl { get; set; }

        public System.DateTime TimeField { get; set; }

        public string Title { get; set; }

        public string UrlField { set; get; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class YSearchResultCollection
    {
        public string NextPageUrl { get; set; }
        List<YSearchItem> searchResult = new List<YSearchItem>();

        public List<YSearchItem> SearchResult
        {
            get { return searchResult; }
            set { searchResult = value; }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class YSearch
    {
        public YSearchResultCollection SearchWeb(String query, int offset, int limit, ref OmukSemantics semantics)
        {
            //semantics = new OmukSemantics();
            YSearchResultCollection resultCollection = new YSearchResultCollection();
            
            ysearchresponse ysearchResponse= this.ProcessRequest(query, offset, limit);
            resultCollection.NextPageUrl = ysearchResponse.nextpage;
            foreach (ysearchresponseResultset_webResult webResult in ysearchResponse.resultset_web.result)
            {
                resultCollection.SearchResult.Add(new YSearchItem()
                                                    {
                                                        AbstractText = webResult.@abstract,
                                                        Title = webResult.title,
                                                        UrlField = webResult.url
                                                    });
                semantics.ProcessContext(webResult.title, webResult.url, webResult.@abstract);
            }

            if (offset == 0)
            {
                if (semantics.Category.Equals("movie"))
                {
                    ysearchResponse = this.ProcessRequest(query + " rottentomatoes", 0, 4);
                    foreach (ysearchresponseResultset_webResult webResult in ysearchResponse.resultset_web.result)
                    {
                        if (webResult.url.StartsWith("http://www.rottentomatoes.com/m") || webResult.url.StartsWith("http://rottentomatoes.com/m"))
                        {
                            resultCollection.SearchResult.Insert(1, (new YSearchItem()
                            {
                                AbstractText = webResult.@abstract,
                                Title = webResult.title,
                                UrlField = webResult.url
                            }));
                        }
                    }
                }

            }

            return resultCollection;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        private ysearchresponse ProcessRequest(String query, int offset, int limit)
        {
            String endpoint = String.Format("http://boss.yahooapis.com/ysearch/web/v1/{0}?appid=xjCtHczIkY0i2VRz8Wxl0V.4z9OQ0m3N&format=xml&start={1}&count={2}", query, offset, limit);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(ysearchresponse));

            return (ysearchresponse)serializer.Deserialize(response.GetResponseStream());
        }
    }
}
