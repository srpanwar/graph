using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using org.lyricwiki.www.service;

namespace Omuk.OmukControls
{
    public class LyricWikiControl: OmukControl
    {
        protected LyricsResult lyricsResult = null;
        public LyricWikiControl()
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
            get { return "lyricwiki"; }
        }

        public String GetHtmlContent(string searchText, String data, String sense, Object source, Dictionary<String, String> uniqueLinks)
        {
            if (source == null)
                source = this.GetSource(searchText, data);

            if (source == null)
                throw new ArgumentNullException("Source is NULL");

            this.lyricsResult = source as LyricsResult;
            if (source == null)
                throw new InvalidCastException("Source is not of correct type");

            bool hasData = false;
            String html = String.Empty;
            if (this.lyricsResult != null && !this.lyricsResult.lyrics.Equals("Not found", StringComparison.InvariantCultureIgnoreCase))
            {
                hasData = true;
                html += " <td id=\"td1\" style=\"width: 100%;vertical-align: top\">";
                html += "     <table style=\"width: 100%\" cellpadding=\"0\" cellspacing=\"0\">";
                html += "         <tr>";
                html += "             <td style=\"border-bottom: solid 1px Silver; width: 100%; color: Black; font-family: Calibri;font-size: 16px;\">";
                //html += "                 <b>L</b>yrics (" + this.lyricsResult.song + ")&nbsp;";
                html += "                 <a href=\"http://www.lyricwiki.org\" style=\"font-size:small;color:green;text-decoration:none;font-family:Calibri\">source: lyricwiki.org";
                html += "                 </a>";
                html += "             </td>";
                html += "         </tr>";
                html += "         <tr>";
                html += "             <td id=\"lyricBox\" title=\"" + this.lyricsResult.url + "\" style=\"padding-bottom: 3px;font-family: Calibri; font-size: small;\">";
                String lyric = this.lyricsResult.lyrics;//this.lyricsResult.GetLyricsHtml();
                html += lyric.Substring(0, lyric.Length > 400 ? 400 : lyric.Length);
                html += "                 <br />";
                html += "                 <a target=\"_blank\" href=\"" + this.lyricsResult.url + "\">full lyrics &raquo;";
                html += "                 </a>";
                html += "             </td>";
                html += "         </tr>";
                html += "         <tr><td style=\"height: 5px\">&nbsp;</td></tr>";
                html += "     </table>";
                html += " </td>";
            }
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

                LyricWiki lyricWiki = new LyricWiki();
                lyricsResult = lyricWiki.getSong(artist, song);
                return lyricsResult;
            }
            catch { }

            return null;
        }
        #endregion
    }
}
