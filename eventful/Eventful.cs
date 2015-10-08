using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace com.eventful.www
{
    /// <summary>
    /// 
    /// </summary>
    public class EventCollection
    {
        public List<EEvent> Events { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EEvent
    {
        public String Title { get; set; }
        public String UrlLink { get; set; }
        public String Description { get; set; }
        public String ImageThumb { get; set; }
        public String StartTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Eventful
    {
        //
        public EventCollection SearchEvents(String query, int offset, int limit)
        {
            EventCollection collection = new EventCollection();
            collection.Events = new List<EEvent>();
            xsd.search srch = this.ProcessRequest(query, offset, limit);
            foreach (xsd.searchEvent evnt in srch.events)
            {
                collection.Events.Add(new EEvent()
                {
                    Description = evnt.description,
                    Title = evnt.title,
                    UrlLink = evnt.url,
                    ImageThumb = (evnt.image != null && evnt.image.thumb != null) ? evnt.image.thumb.url : "http://api.eventful.com/images/powered/eventful_88x31.gif",
                    StartTime = evnt.start_time
                });
            }

            return collection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        private xsd.search ProcessRequest(String query, int offset, int limit)
        {
            if (offset == 0) offset = 1;
            String endpoint = String.Format("http://api.eventful.com/rest/events/search?app_key=MKr7J32RZ5qhqD2P&location={0}&date=This+Week&sort_order=popularity&page_number={1}&page_size={2}&within=100&units=mi", query, offset, limit);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(xsd.search));

            return (xsd.search)serializer.Deserialize(response.GetResponseStream());
        }
    }
}
