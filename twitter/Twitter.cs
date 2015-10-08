using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace com.twitter.www
{
    public class TSearchResultCollection
    {
        public String AlternateSearch { get; set; }
        public List<TSearchResult> Items { get; set; }
    }

    public class TSearchResult
    {
        public DateTime PublishedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String TweetLink { get; set; }
        public String ProfileImageLink { get; set; }
        public String AuthorName { get; set; }
        public String AuthorProfileLink { get; set; }
        public String Content { get; set; }
    }

    public class Twitter
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public TSearchResultCollection SearchTwitter(String query, int offset, int limit)
        {
            TSearchResultCollection resultCollection = new TSearchResultCollection();
            resultCollection.Items = new List<TSearchResult>();
            xsd.feed feed = this.ProcessRequest(query, offset, limit);

            xsd.feedLink feedlinkAltSearch = (xsd.feedLink)Array.Find(feed.Items, fd => (fd is xsd.feedLink) && ((xsd.feedLink)fd).rel.Equals("alternate", StringComparison.InvariantCultureIgnoreCase));
            if (feedlinkAltSearch != null)
                resultCollection.AlternateSearch = feedlinkAltSearch.href;

            Object[] feedentries = Array.FindAll(feed.Items, fd => (fd is xsd.feedEntry));
            foreach (Object entryObj in feedentries)
            {
                xsd.feedEntry entry = (xsd.feedEntry)entryObj;
                TSearchResult res = new TSearchResult();
                xsd.feedEntryAuthor author = (xsd.feedEntryAuthor)Array.Find(entry.Items, en => (en is xsd.feedEntryAuthor));
                if (author != null)
                {
                    res.AuthorName = author.name;
                    res.AuthorProfileLink = author.uri;
                }

                int index = Array.FindIndex(entry.ItemsElementName, enn => (enn == xsd.ItemsChoiceType.published));
                if(index != -1)
                    res.PublishedDate = (DateTime)entry.Items[index];
                
                index = Array.FindIndex(entry.ItemsElementName, enn => (enn == xsd.ItemsChoiceType.updated));
                if (index != -1)
                    res.UpdatedDate = (DateTime)entry.Items[index];

                xsd.feedEntryLink tweet = (xsd.feedEntryLink)Array.Find(entry.Items, en => (en is xsd.feedEntryLink && ((xsd.feedEntryLink)en).rel.Equals("alternate", StringComparison.InvariantCultureIgnoreCase)));
                if (tweet != null)
                    res.TweetLink = tweet.href;

                xsd.feedEntryLink image = (xsd.feedEntryLink)Array.Find(entry.Items, en => (en is xsd.feedEntryLink && ((xsd.feedEntryLink)en).rel.Equals("image", StringComparison.InvariantCultureIgnoreCase)));
                if (image != null)
                    res.ProfileImageLink = image.href;

                xsd.feedEntryContent content = (xsd.feedEntryContent)Array.Find(entry.Items, en => (en is xsd.feedEntryContent));
                if (content != null)
                    res.Content = content.Value;

                resultCollection.Items.Add(res);
            }

            return resultCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movie"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public void GetMovieTrends(String movie, ref TMovieTrendsClass mTrends, params Object[] parameters)
        {
            //MovieTrendsClass mTrends = new MovieTrendsClass();

            int total = 10;
            if (parameters.Length > 0)
                total = (int)parameters[0];
            
            TSearchResultCollection searchResColl = this.SearchTwitter(movie, 1, 10);
            for (int index = 2; index < total; index++)
            {
                try
                {
                    searchResColl.Items.AddRange(this.SearchTwitter(movie, index, 10).Items);
                }
                catch { }
            }
                
            mTrends.FormTrend(searchResColl);
            return;// mTrends;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        private xsd.feed ProcessRequest(String query, int offset, int limit)
        {
            if (offset == 0) offset = 1;
            String endpoint = String.Format("http://search.twitter.com/search.atom?q={0}&page={1}", query, offset);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(xsd.feed));

            return (xsd.feed)serializer.Deserialize(response.GetResponseStream());
        }
    }
}
