using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace com.youtube.www
{
    /// <summary>
    /// 
    /// </summary>
    public class YTVideo
    {
        public String Id { get; set; }
        public String UrlLink { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class YouTube
    {
        //http://gdata.youtube.com/feeds/api/videos?q=football+soccer&start-index=1&max-results=2

        public List<YTVideo> SearchVideos(String query, int offset, int limit)
        {
            List<YTVideo> videos = new List<YTVideo>();
            xsd.feed ytFeed = ProcessRequest<xsd.feed>(query, offset, limit);
            foreach (xsd.feedEntry entry in ytFeed.entry)
            {
                if (entry.noembed == null)
                {
                    videos.Add(new YTVideo()
                                {
                                    UrlLink = entry.group.player.url,
                                    Title = entry.title.Value,
                                    Description = entry.group.description.Value,
                                    Id = new Uri(entry.group.player.url).Query.Split('=')[1]
                                });
                }
            }
            return videos;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        private T ProcessRequest<T>(String query, int offset, int limit)
        {
            query = query.Replace(' ', '+');
            String endpoint = String.Format("http://gdata.youtube.com/feeds/api/videos?q={0}&start-index={1}&max-results={2}", query, offset > 0 ? offset : 1, limit);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(response.GetResponseStream());
        }
    }


}
