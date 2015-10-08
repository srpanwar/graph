using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;

namespace com.grooveshark.www
{
    /// <summary>
    /// 
    /// </summary>
    public class GSSong
    {
        public String TinySongLink { get; set; }
        public String SongId { get; set; }
        public String SongName { get; set; }
        public String ArtistID { get; set; }
        public String ArtistName { get; set; }
        public String AlbumID { get; set; }
        public String AlbumName { get; set; }
        public String GroovesharkLink { set; get; }
        public string musicEmbedUrl = String.Empty;
        public String MusicEmbedUrl 
        {
            get
            {
                if (String.IsNullOrEmpty(this.musicEmbedUrl))
                    this.musicEmbedUrl = Grooveshark.GetMusicEmbedUrl(this.SongId);
                return this.musicEmbedUrl;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class Grooveshark
    {
        public GSSong[] SearchSong(String query, int count)
        {
            List<GSSong> songs = new List<GSSong>();
            String endpoint = String.Format("http://tinysong.com/s/{0}?limit={1}", query, count);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                while (!reader.EndOfStream)
                {
                    String songinfo = reader.ReadLine();
                    String[] songparts = songinfo.Split(';');
                    if (songparts != null && songparts.Length == 8)
                    {
                        songs.Add(new GSSong()
                                    {
                                        TinySongLink = songparts[0].Trim(),
                                        SongId = songparts[1].Trim(),
                                        SongName = songparts[2].Trim(),
                                        ArtistID = songparts[3].Trim(),
                                        ArtistName = songparts[4].Trim(),
                                        AlbumID = songparts[5].Trim(),
                                        AlbumName = songparts[6].Trim(),
                                        GroovesharkLink = songparts[7].Trim()
                                    });
                    }
                }
            }

            return songs.ToArray(); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="songId"></param>
        /// <returns></returns>
        internal static  String GetMusicEmbedUrl(String songId)
        {
            String embedurl = "<object width='100%' height='42px'> <param name='movie' value='http://listen.grooveshark.com/songWidget.swf'></param> <param name='wmode' value='window'></param> <param name='allowScriptAccess' value='always'></param> <param name='flashvars' value='hostname=cowbell.grooveshark.com&widgetID={0}&style=metal&p=0'></param> <embed src='http://listen.grooveshark.com/songWidget.swf' type='application/x-shockwave-flash' width='100%' height='40px' flashvars='hostname=cowbell.grooveshark.com&widgetID={0}&style=metal&p=0' allowScriptAccess='always' wmode='window'></embed></object>";
            String widgetid = String.Empty;

            String endpoint = String.Format("http://widgets.grooveshark.com/make?new&songid={0}&type=1",songId);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            String []queries = response.ResponseUri.Query.Split('&');
            foreach(String query in queries)
            {
                if (query.StartsWith("widgetid"))
                {
                    widgetid = query.Split('=')[1];
                }
            }

            embedurl = String.Format(embedurl, widgetid);
            return embedurl;
        }
    }
}
