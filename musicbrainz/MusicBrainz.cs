using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using ArtistNS = org.musicbrainz.www.xsd.artistns;
using AlbumNS = org.musicbrainz.www.xsd.albumns;
using SongNS = org.musicbrainz.www.xsd.songns;

namespace org.musicbrainz.www
{
    public class MBSong
    {
        public String Title { get; set; }
        public String MBID { get; set; }
        public String ArtistName { get; set; }
        public String ArtistMBID { get; set; }
        public String AlbumName { get; set; }
        public String AlbumMBID { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MBAlbum
    {
        public String Title { get; set; }
        public String MBID { get; set; }
        public String ArtistName { get; set; }
        public String ArtistMBID { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MBArtist
    {
        public String Name { get; set; }
        public String MBID { get; set; }
        public String TType { get; set; }
        public DateTime StartDate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MusicBrainz
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        private Object ProcessSearchQuery(String endpoint, Type typ)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            XmlSerializer serializer = new XmlSerializer(typ);
            return serializer.Deserialize(response.GetResponseStream());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="mbAartist"></param>
        /// <returns></returns>
        public Boolean IsArtist(String query, out MBArtist mbAartist)
        {
            mbAartist = null;
            String endpoint = String.Format("http://musicbrainz.org/ws/1/artist/?type=xml&name={0}&offset=0&limit=2", query);
            Object obj = this.ProcessSearchQuery(endpoint, typeof(ArtistNS.metadata));
            if (obj == null)
                return false;

            ArtistNS.metadata mData = obj as ArtistNS.metadata;
            if (mData == null)
                return false;

            if (mData.artistlist == null || mData.artistlist.artist == null || mData.artistlist.artist.Length == 0)
                return false;

            if (!query.Equals(mData.artistlist.artist[0].name, StringComparison.InvariantCultureIgnoreCase))
                return false;

            mbAartist = new MBArtist()
            {
                Name = mData.artistlist.artist[0].name,
                MBID = mData.artistlist.artist[0].id,
                StartDate = mData.artistlist.artist[0].lifespan != null ? mData.artistlist.artist[0].lifespan.begin : default(DateTime),
                TType = mData.artistlist.artist[0].type
            };
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<MBArtist> GetArtists(String query, int offset, int limit)
        {
            List<MBArtist> artistList = new List<MBArtist>();
            String endpoint = String.Format("http://musicbrainz.org/ws/1/artist/?type=xml&name={0}&offset={1}&limit={2}", query, offset, limit);

            Object obj = this.ProcessSearchQuery(endpoint, typeof(ArtistNS.metadata));
            if (obj == null)
                return null;

            ArtistNS.metadata mData = obj as ArtistNS.metadata;
            if (mData == null)
                return null;

            if (mData.artistlist == null || mData.artistlist.artist == null || mData.artistlist.artist.Length == 0)
                return null;

            foreach (ArtistNS.metadataArtistlistArtist xsdArtist in mData.artistlist.artist)
            {
                MBArtist mbAartist = new MBArtist()
                {
                    Name = xsdArtist.name,
                    MBID = xsdArtist.id,
                    //StartDate = xsdArtist.lifespan.begin,
                    TType = xsdArtist.type
                };
                artistList.Add(mbAartist);
            }

            return artistList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<MBAlbum> GetAlbums(String query,String artist,String artistmbid, int offset, int limit)
        {
            List<MBAlbum> albumList = new List<MBAlbum>();
            String append = String.Empty;

            if (!String.IsNullOrEmpty(query))
                append += String.Format("&title={0}", query);

            if (!String.IsNullOrEmpty(artist))
                append += String.Format("&artist={0}", artist);

            if (!String.IsNullOrEmpty(artistmbid))
                append += String.Format("&artistid={0}", artistmbid);

            String endpoint = String.Format("http://musicbrainz.org/ws/1/release/?type=xml&releasetypes=Album&offset={0}&limit={1}{2}", offset, limit, append);

            Object obj = this.ProcessSearchQuery(endpoint, typeof(AlbumNS.metadata));
            if (obj == null)
                return null;

            AlbumNS.metadata mData = obj as AlbumNS.metadata;
            if (mData == null)
                return null;

            if (mData.releaselist == null || mData.releaselist.release == null || mData.releaselist.release.Length == 0)
                return null;

            foreach (AlbumNS.metadataReleaselistRelease xsdRelease in mData.releaselist.release)
            {
                MBAlbum mbAlbum = new MBAlbum()
                {
                    Title = xsdRelease.title,
                    MBID = xsdRelease.id,
                    ArtistName = xsdRelease.artist.name,
                    ArtistMBID = xsdRelease.artist.id
                };
                albumList.Add(mbAlbum);
            }

            return albumList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="artist"></param>
        /// <param name="album"></param>
        /// <param name="artistmbid"></param>
        /// <param name="albummbid"></param>
        /// <returns></returns>
        public List<MBSong> GetSongs(String query, String artist, String album, String artistmbid, String albumbid, int offset, int limit)
        {
            List<MBSong> songList = new List<MBSong>();
            String append = String.Empty;
            if (!String.IsNullOrEmpty(query))
                append += String.Format("&title={0}", query);

            if (!String.IsNullOrEmpty(artist))
                append += String.Format("&artist={0}", artist);

            if (!String.IsNullOrEmpty(artistmbid))
                append += String.Format("&artistid={0}", artistmbid);

            if (!String.IsNullOrEmpty(album))
                append += String.Format("&release={0}", album);

            if (!String.IsNullOrEmpty(artistmbid))
                append += String.Format("&releaseid={0}", albumbid);

            String endpoint = String.Format("http://musicbrainz.org/ws/1/track/?type=xml&releasetypes=Album&offset={0}&limit={1}{2}", offset, limit, append);

            Object obj = this.ProcessSearchQuery(endpoint, typeof(SongNS.metadata));
            if (obj == null)
                return null;

            SongNS.metadata mData = obj as SongNS.metadata;
            if (mData == null)
                return null;

            if (mData.tracklist == null || mData.tracklist.track == null || mData.tracklist.track.Length == 0)
                return null;

            foreach (SongNS.metadataTracklistTrack xsdTrack in mData.tracklist.track)
            {
                MBSong mbSong = new MBSong()
                {
                    Title = xsdTrack.title,
                    MBID = xsdTrack.id,
                    ArtistName = xsdTrack.artist.name,
                    ArtistMBID = xsdTrack.artist.id,
                    AlbumName = xsdTrack.releaselist.release.title,
                    AlbumMBID = xsdTrack.releaselist.release.id
                };
                songList.Add(mbSong);
            }

            return songList;
        }
    }
}
