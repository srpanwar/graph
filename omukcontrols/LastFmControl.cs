using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using last.fm;

namespace Omuk.OmukControls
{
    public class LastFmControl : OmukControl
    {
        protected List<LFMAlbum> albums = new List<LFMAlbum>();

        public LastFmControl()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region OmukControl Members
        public String Sense
        {
            get
            {
                return "music";
            }
        }

        public String Area
        {
            get
            {
                return "right-annotation";
            }
        }
        public String Name
        {
            get { return "lastfm"; }
        }


        public string GetHtmlContent(string searchText, string data, string sense, object source, Dictionary<string, string> uniqueLinks)
        {
            if (source == null)
                source = this.GetSource(searchText, data);

            if (source == null)
                throw new ArgumentNullException("Source is NULL"); 

            this.albums = source as List<LFMAlbum>;
            if (source == null)
                throw new InvalidCastException("Source is not of correct type");

            bool hasData = false;
            String html = String.Empty;
            html += "   <td id=\"tdlastFmAlbums\" style=\"width: 100%; vertical-align: top\">";
            html += "       <table style=\"width: 100%\" cellpadding=\"0\" cellspacing=\"0\">";
            html += "           <tr>";
            html += "               <td style=\"border-bottom: dotted 1px Silver; width: 100%; color: Black; font-family: Calibri;font-size: 16px;\">";
            //html += "                   <b>A</b>lbums &nbsp;";
            html += "                   <span style=\"font-size:small;color:green;text-decoration:none;font-family:Calibri\">";
            html += "                       <a target=\"_blank\" href=\"http://last.fm\" style=\"color:inherit;text-decoration:none\">source: last.fm</a> &nbsp; &amp;";
            html += "                       <a target=\"_blank\" href=\"http://www.musicbrainz.org\" style=\"color:inherit;text-decoration:none\">source: musicbrainz.org</a>";
            html += "                   </span>";
            html += "               </td>";
            html += "           </tr>";
            html += "           <tr>";
            html += "               <td style=\"width:100%\">";
            html += "                   <table cellpadding=\"2\" cellspacing=\"0\" style=\"width:100%\" border=\"0\">";
            for (int index = 0; index < this.albums.Count && index < 8; index++)
            {
                hasData = true;
                if (index % 4 == 0)
                {
                    html += "               <tr>";
                }
                LFMAlbum lfmAlbum = this.albums[index];
                String altTxt = String.Format("{0} {1}", lfmAlbum.Artist, lfmAlbum.Name);
                String imageLink = (String.IsNullOrEmpty(lfmAlbum.ImageLinkSmall)) ? "images/noimage.gif" : lfmAlbum.ImageLinkSmall;
                html += "                       <td style=\"font-family: Calibri; font-size: small;vertical-align:top;width:20%\" >";
                html += "                           <a target=\"_blank\" href=\"" + lfmAlbum.UrlLink + "\" title=\"" + altTxt + "\" style=\"text-decoration:none\" >";
                html += "                               <img style=\"height:50px;width:50px;border:none\" src=\"" + imageLink + "\" alt=\"" + lfmAlbum.Name + "\" />";
                html += "                               <br />" + lfmAlbum.Name;
                html += "                           </a>";
                html += "                       </td>";

                if ((index + 1) % 4 == 0)
                {
                    html += "               </tr>";
                }
            }
            html += "                   </table>";
            html += "               </td>";
            html += "           </tr>";
            html += "       </table>";
            html += "   </td>";

            return hasData ? html : String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private Object GetSource(String query, String data)
        {
            try
            {
                if (String.IsNullOrEmpty(query))
                    return null;

                if (String.IsNullOrEmpty(data))
                    return null;

                Regex regex = new Regex(@"^(artist:)(.*?)(\|album:)(.*?)(\|song:)(.*?)$");
                Console.WriteLine(regex.IsMatch("artist:e|album:1|song:5"));

                if (!regex.IsMatch(data))
                    return null;
                Match match = regex.Match(data);
                String artist = match.Groups[2].Value.Trim();
                String album = match.Groups[4].Value.Trim();
                String song = match.Groups[6].Value.Trim();

                LastFmService lastfm = new LastFmService();
                if (String.IsNullOrEmpty(artist))
                    return null;

                if (String.IsNullOrEmpty(album) && String.IsNullOrEmpty(song))
                {
                    return lastfm.SearchAlbum(artist, 0);
                }
            }
            catch { }

            return null;
        }
        #endregion
    }
}