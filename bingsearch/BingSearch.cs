using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Omuk.OmukEngine;

namespace com.bing.www.search
{
    /// <summary>
    /// 
    /// </summary>
    public class BingSearchError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
        public string HelpUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BingSearchItem
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string UrlField { set; get; }
        public string DisplayUrl { get; set; }
        public System.DateTime TimeField { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BingSearchItemCollection
    {
        public Int64 Total { get; set; }
        public Int64 Offset { get; set; }
        List<BingSearchItem> results = new List<BingSearchItem>();
        List<BingSearchError> errors = new List<BingSearchError>();

        public List<BingSearchError> Errors
        {
            get { return errors; }
            set { errors = value; }
        }

        public List<BingSearchItem> Results
        {
            get { return results; }
            set { results = value; }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class BingSearch
    {
        private String AppID = "52447E790CBE691A81FDFF596A4A976B85715ED4";//"52447E790CBE691A81FDFF596A4A976BB4FD39CD";
        public BingSearchItemCollection SearchWeb(String query, int offset, int limit, ref OmukSemantics semantics)
        {
            xsd.SearchResponse searchResponse = this.ProcessRequest(query, offset, limit);
            BingSearchItemCollection itemCollection = new BingSearchItemCollection();

            if (searchResponse.Errors != null && searchResponse.Errors.Length > 0)
            {
                foreach (xsd.SearchResponseErrors errors in searchResponse.Errors)
                {
                    itemCollection.Errors.Add(new BingSearchError()
                                                {
                                                    Code = errors.Error.Code,
                                                    HelpUrl = errors.Error.HelpUrl,
                                                    Message = errors.Error.Message,
                                                    Parameter = errors.Error.Parameter,
                                                    Value = errors.Error.Value
                                                });
                }
            }

            if (searchResponse.Web != null && searchResponse.Web.Results != null)
            {
                itemCollection.Offset = searchResponse.Web.Offset;
                itemCollection.Total = searchResponse.Web.Total;
                if (searchResponse.Web.Results.Length > 0)
                {
                    foreach (xsd.WebWebResult result in searchResponse.Web.Results)
                    {
                        String desc = String.IsNullOrEmpty(result.Description) ? String.Empty :
                                                                    result.Description.Replace("", "<b>").Replace("", "</b>");
                        String title = result.Title.Replace("", "<b>").Replace("", "</b>");
                        itemCollection.Results.Add(new BingSearchItem()
                                                    {
                                                        Description = desc,
                                                        DisplayUrl = result.DisplayUrl,
                                                        TimeField = result.DateTime,
                                                        Title = title,
                                                        UrlField = result.Url
                                                    });
                        semantics.ProcessContext(title, result.Url, desc);
                    }
                }
            }

            if (offset == 0)
            {
                if (semantics.Category.Equals("movie"))
                {
                    searchResponse = this.ProcessRequest(query + " rottentomatoes", 0, 4);
                    if (searchResponse.Web.Results.Length > 0)
                    {
                        foreach (xsd.WebWebResult result in searchResponse.Web.Results)
                        {
                            String desc = String.IsNullOrEmpty(result.Description) ? String.Empty :
                                                                        result.Description.Replace("", "<b>").Replace("", "</b>");
                            String title = result.Title.Replace("", "<b>").Replace("", "</b>");

                            if (result.Url.StartsWith("http://www.rottentomatoes.com/m") || result.Url.StartsWith("http://rottentomatoes.com/m"))
                            {
                                itemCollection.Results.Add(new BingSearchItem()
                                                            {
                                                                Description = desc,
                                                                DisplayUrl = result.DisplayUrl,
                                                                TimeField = result.DateTime,
                                                                Title = title,
                                                                UrlField = result.Url
                                                            });
                            }
                        }
                    }
                }
            }

            return itemCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        private xsd.SearchResponse ProcessRequest(String query, int offset, int limit)
        {
            String endpoint = String.Format("http://api.bing.net/xml.aspx?AppId={0}&Query={1}&Sources=Web&Version=2.0&Options=EnableHighlighting&Market=en-us&Adult=Moderate&Web.Offset={2}&Web.Count={3}&Web.Options=DisableHostCollapsing", AppID, query, offset, limit);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(xsd.SearchResponse));
            xsd.SearchResponse searchResponse = (xsd.SearchResponse)serializer.Deserialize(response.GetResponseStream());
            return searchResponse;
        }
    }
}