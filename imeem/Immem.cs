using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace com.imeem.www
{
    /// <summary>
    /// 
    /// </summary>
    public class ImmemSearchItem
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }   
        public string Description { get; set; }
        public string Genre { get; set; }
        public Int64 Duration { get; set; }
        public bool IsSample { get; set; }
        public string IconUrl { get; set; }
        public string MusicEmbedUrl { get; set; }
        public string Id { get; set; }
        public string UrlLink { get; set; }
        
        public ImmemSearchItem(xsd.resultItem resItem)
        {
            this.Album = resItem.album;
            this.Artist = resItem.artist;
            this.Type = resItem.type;
            this.Title= resItem.title;
            this.Description = resItem.description;
            this.Genre= resItem.genre;
            this.Duration = resItem.duration;
            this.IsSample = resItem.isSample;
            this.IconUrl = resItem.iconUrl;
            this.MusicEmbedUrl = resItem.musicEmbedUrl;
            this.Id = resItem.id;
            this.UrlLink = resItem.url;
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public class Immem
    {
        private String _secret = "27370d7b-ada5-445e-ac9d-e77e6d5d84fb";
        private String _apiKey = "ce80313c-ede6-4a0b-a0c8-d68ffcc40931";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="type"></param>
        /// <param name="offset"></param>
        /// <param name="numResults"></param>
        public List<ImmemSearchItem> SearchMedia(String query, String mediaType, int offset, int numResults)
        {
            SortedList<String, String> sortedList = new SortedList<string, string>();
            sortedList.Add("query=", query);
            sortedList.Add("mediaType=", mediaType);
            sortedList.Add("version=", "1.0");
            sortedList.Add("offset=", offset.ToString());
            sortedList.Add("numResults=", numResults.ToString());
            sortedList.Add("apiKey=", this._apiKey);
            String concatStr = "mediaSearch";
            foreach (KeyValuePair<String, String> keyVal in sortedList)
                concatStr += keyVal.Key + keyVal.Value;
            concatStr += this._secret;
            
            concatStr = this.GetMD5Str(concatStr);

            String endpoint = String.Format("http://www.imeem.com/api/xml/mediaSearch?&query={0}&mediaType={1}&version=1.0&offset={2}&numResults={3}&apiKey={4}&sig={5}", query, mediaType, offset, numResults, this._apiKey, concatStr);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(xsd.result));
            xsd.result res = (xsd.result)serializer.Deserialize(response.GetResponseStream());
            
            List<ImmemSearchItem> ret = new List<ImmemSearchItem>();
            Array.ForEach(res.items, ritm => ret.Add(new ImmemSearchItem(ritm)));
            
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private String GetMD5Str(String input)
        {
            byte[] inb = System.Text.Encoding.Default.GetBytes(input);
            MD5 md5 = MD5.Create();
            inb = md5.ComputeHash(inb);
            StringBuilder sb = new StringBuilder(inb.Length * 2);
            foreach (byte b in inb)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
    }
}
