using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace org.lyricwiki.www
{
    /// <summary>
    /// 
    /// </summary>
    public class LWLyrics
    {
        public String Artist { get; set; }
        public String Lyrics { get; set; }
        public String Song { get; set; }
        public String SongUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LyricWiki
    {
        public LWLyrics GetLyricsForSong(String artist,String song)
        {
            BasicHttpBinding binding = new BasicHttpBinding();

            service.LyricWikiPortTypeClient wiki = new service.LyricWikiPortTypeClient(binding, new EndpointAddress("http://lyricwiki.org/server.php"));
            wiki.Open();
            service.LyricsResult result = wiki.getSongResult(artist,song);
            if (result != null)
            {
                return new LWLyrics()
                {
                    Artist = result.artist,
                    Lyrics = result.lyrics,
                    Song = result.song,
                    SongUrl = result.url
                };
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Boolean IsArtist(String artist)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            service.LyricWikiPortTypeClient wiki = new service.LyricWikiPortTypeClient(binding, new EndpointAddress("http://lyricwiki.org/server.php"));
            wiki.Open();
            String[] artists = wiki.searchArtists(artist);
            if (artists.Length > 0 && artists[0].Equals(artist, StringComparison.InvariantCultureIgnoreCase))
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artist"></param>
        /// <returns></returns>
        public String[] GetAlbums(String artist)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            service.LyricWikiPortTypeClient wiki = new service.LyricWikiPortTypeClient(binding, new EndpointAddress("http://lyricwiki.org/server.php"));
            wiki.Open();
            String []albums = null;
            wiki.getArtist(ref artist, out albums);
            return albums;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="album"></param>
        /// <returns></returns>
        public String[] GetAlbumSongs(String artist, String album)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            service.LyricWikiPortTypeClient wiki = new service.LyricWikiPortTypeClient(binding, new EndpointAddress("http://lyricwiki.org/server.php"));
            wiki.Open();
            String[] songs = null;
            int year = 0;
            String amazonLink = String.Empty;
            wiki.getAlbum(ref artist, ref album, ref year, out amazonLink, out songs);
            return songs;
        }
    }
}
