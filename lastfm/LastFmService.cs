using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using ArtistNS = last.fm.xsd.artistns;
using AlbumNS = last.fm.xsd.albumns;
using ArtistNSSearch = last.fm.xsd.artistns.search;
using AlbumNSSearch = last.fm.xsd.albumns.search;

namespace last.fm
{
    /// <summary>
    /// 
    /// </summary>
    public class LFMAlbum
    {
        public String Name { get; set; }
        public String Artist { get; set; }
        public String UrlLink { get; set; }
        public String ImageLinkSmall { get; set; }
        public String ImageLinkMedium { get; set; }
        public String ImageLinkLarge { get; set; }
        public String ImageLinkXLarge { get; set; }

        public String Summary { get; set; }
        public String Content { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LFMArtist
    {
        public String Name { get; set; }
        public String UrlLink { get; set; }
        public String ImageLinkSmall { get; set; }
        public String ImageLinkMedium { get; set; }
        public String ImageLinkLarge { get; set; }

        private List<LFMArtist> similarArtist = new List<LFMArtist>();

        public List<LFMArtist> SimilarArtist
        {
            get { return similarArtist; }
            set { similarArtist = value; }
        }

        public String Summary { get; set; }
        public String Content { get; set; }
    }

    public class LastFmService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public LFMArtist GetArtistInfo(String artist, String mbid)
        {
            artist = System.Web.HttpUtility.UrlEncode(artist);

            String append = String.Empty;
            if (!String.IsNullOrEmpty(mbid))
                append += "&mbid=" + mbid;
            if (!String.IsNullOrEmpty(artist))
                append += "&artist=" + artist;
            
            String endpoint = String.Format("http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&api_key=b25b959554ed76058ac220b7b2e0a026&limit=10{0}", append);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(ArtistNS.lfm));
            ArtistNS.lfm lastfm = (ArtistNS.lfm)serializer.Deserialize(response.GetResponseStream());

            LFMArtist lfmArtist = new LFMArtist()
                                    {
                                        Name = lastfm.artist.name,
                                        UrlLink = lastfm.artist.url,
                                        ImageLinkSmall = Array.Find(lastfm.artist.image, img => img.size.Equals("small")).Value,
                                        ImageLinkMedium = Array.Find(lastfm.artist.image, img => img.size.Equals("medium")).Value,
                                        ImageLinkLarge = Array.Find(lastfm.artist.image, img => img.size.Equals("large")).Value,
                                        Summary = lastfm.artist.bio.summary,
                                        Content = lastfm.artist.bio.content
                                    };

            Array.ForEach(lastfm.artist.similar, sim => lfmArtist.SimilarArtist.Add(
                                    new LFMArtist()
                                    {
                                        Name = sim.name,
                                        UrlLink = sim.url,
                                        ImageLinkSmall = Array.Find(sim.image, img => img.size.Equals("small")).Value,
                                        ImageLinkMedium = Array.Find(sim.image, img => img.size.Equals("medium")).Value,
                                        ImageLinkLarge = Array.Find(sim.image, img => img.size.Equals("large")).Value
                                    }));

            return lfmArtist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="albumName"></param>
        /// <returns></returns>
        public LFMAlbum GetAlbumInfo(String artistName, String albumName, String mbid)
        {
            artistName = System.Web.HttpUtility.UrlEncode(artistName);
            albumName = System.Web.HttpUtility.UrlEncode(albumName);

            String append = String.Empty;
            if (!String.IsNullOrEmpty(mbid))
                append += "&mbid=" + mbid;
            else
                append += "&album=" + albumName;

            String endpoint = String.Format("http://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key=b25b959554ed76058ac220b7b2e0a026&artist={0}&limit=10{1}", artistName, append);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(AlbumNS.lfm));
            AlbumNS.lfm lastfm = (AlbumNS.lfm)serializer.Deserialize(response.GetResponseStream());
            LFMAlbum lfmAlbum = new LFMAlbum()
            {
                Name = lastfm.album.name,
                UrlLink = lastfm.album.url,
                ImageLinkSmall = Array.Find(lastfm.album.image, img => img.size.Equals("small")).Value,
                ImageLinkMedium = Array.Find(lastfm.album.image, img => img.size.Equals("medium")).Value,
                ImageLinkLarge = Array.Find(lastfm.album.image, img => img.size.Equals("large")).Value,
                ImageLinkXLarge = Array.Find(lastfm.album.image, img => img.size.Equals("extralarge")).Value,
                Summary = lastfm.album.wiki.summary,
                Content = lastfm.album.wiki.content
            };

            return lfmAlbum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        public List<LFMArtist> SearchArtist(String query, int page)
        {
            query = System.Web.HttpUtility.UrlEncode(query);
            String endpoint = String.Format("http://ws.audioscrobbler.com/2.0/?method=artist.search&artist={0}&api_key=b25b959554ed76058ac220b7b2e0a026&page={1}&limit=10", query, page);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(ArtistNSSearch.lfm));
            ArtistNSSearch.lfm lastfm = (ArtistNSSearch.lfm)serializer.Deserialize(response.GetResponseStream());

            List<LFMArtist> artistCollection = new List<LFMArtist>();
            Array.ForEach(lastfm.results.artistmatches, amatch => artistCollection.Add(
                                   new LFMArtist()
                                   {
                                       Name = amatch.name,
                                       UrlLink = amatch.url,
                                       ImageLinkSmall = Array.Find(amatch.image, img => img.size.Equals("small")).Value,
                                       ImageLinkMedium = Array.Find(amatch.image, img => img.size.Equals("medium")).Value,
                                       ImageLinkLarge = Array.Find(amatch.image, img => img.size.Equals("large")).Value,
                                       Summary = this.GetArtistInfo(amatch.name, String.Empty).Summary
                                   }));

            return artistCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        public List<LFMAlbum> SearchAlbum(String query, int page)
        {
            query = System.Web.HttpUtility.UrlEncode(query);
            String endpoint = String.Format("http://ws.audioscrobbler.com/2.0/?method=album.search&album={0}&api_key=b25b959554ed76058ac220b7b2e0a026&page={1}&limit=10", query, page);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typeof(AlbumNSSearch.lfm));
            AlbumNSSearch.lfm lastfm = (AlbumNSSearch.lfm)serializer.Deserialize(response.GetResponseStream());

            List<LFMAlbum> albumCollection = new List<LFMAlbum>();
            Array.ForEach(lastfm.results.albummatches, amatch => albumCollection.Add(
                                   new LFMAlbum()
                                   {
                                       Name = amatch.name,
                                       Artist = amatch.artist,
                                       UrlLink = amatch.url,
                                       ImageLinkSmall = Array.Find(amatch.image, img => img.size.Equals("small")).Value,
                                       ImageLinkMedium = Array.Find(amatch.image, img => img.size.Equals("medium")).Value,
                                       ImageLinkLarge = Array.Find(amatch.image, img => img.size.Equals("large")).Value
                                   }));

            return albumCollection;
        }


    }
}
