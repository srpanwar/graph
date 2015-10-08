using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace com.digg.www
{
    public class DiggResult
    {
        public String StoryLink { get; set; }
        public String DiggLink { get; set; }
        public String Status { get; set; }
        public String MediaType { get; set; }
        public String Description { get; set; }
        public String Title { get; set; }
        public String AuthorName { get; set; }
        public String ProfileLink { get; set; }
        public String Topic { get; set; }
        public String Container { get; set; }
        public String StoryShortUrl { get; set; }
    }

    public class Digg
    {
        public List<DiggResult> SearchDigg(String query, int offset, int limit)
        {
            List<DiggResult> result = new List<DiggResult>();
            xsd.stories stories = this.ProcessRequest(query, offset, limit);
            if (stories == null || stories.story == null || stories.story.Length == 0)
                return result;
            foreach (xsd.storiesStory story in stories.story)
            {
                result.Add(new DiggResult()
                {
                    AuthorName = story.user.name,
                    Container = story.container.name,
                    Description = story.description,
                    DiggLink = story.href,
                    MediaType = story.media,
                    ProfileLink = String.IsNullOrEmpty(story.user.icon) ? "http://digg.com/img/badges/32x32-digg-guy.png" : String.Concat("http://digg.com", story.user.icon),
                    Status = story.status,
                    StoryLink = story.link,
                    StoryShortUrl = story.shorturl.short_url,
                    Title = story.title,
                    Topic = story.topic.name
                });
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        private xsd.stories ProcessRequest(String query, int offset, int limit)
        {
            String endpoint = String.Format("/tools/services?endPoint=%2Fsearch%2Fstories&query={0}&type=xml&appkey=http%3A%2F%2Fexample.com&offset={1}&count={2}", query, offset, limit);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://digg.com" + endpoint);
            request.KeepAlive = false;
            request.UserAgent = "Omuk";
            request.Referer = "http://omuk.cloupapp.net/";
            request.AllowAutoRedirect = true;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(xsd.stories));

            return (xsd.stories)serializer.Deserialize(response.GetResponseStream());
        }
    }
}
