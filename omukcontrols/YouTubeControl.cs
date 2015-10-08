using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using com.youtube.www;

namespace Omuk.OmukControls
{
    /// <summary>
    /// Summary description for YouTubeControl
    /// </summary>
    public class YouTubeControl : OmukControl
    {
        List<YTVideo> videos = null;
        public YouTubeControl()
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
                return "movie:music:news";
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
            get { return "youtube"; }
        }

        public String GetHtmlContent(string searchText, String data, String sense, Object source, Dictionary<String, String> uniqueLinks)
        {
            searchText = HttpUtility.UrlDecode(searchText);
            if (sense == "movie")
                searchText += " trailer";
            data = "1|10";
            if (source == null)
                source = this.GetSource(searchText, data);

            if (source == null)
                throw new ArgumentNullException("Source is NULL");

            this.videos = source as List<YTVideo>;
            if (source == null)
                throw new InvalidCastException("Source is not of correct type");

            bool hasData = false;
            String html = String.Empty;
            html += "   <td id=\"tdYouTube\" style=\"width: 100%;vertical-align: top\">";
            html += "       <table style=\"width: 100%\" cellpadding=\"0\" cellspacing=\"0\">";
            html += "           <tr>";
            html += "               <td style=\"border-bottom: dotted 1px Silver; width: 100%; color: Black; font-family: Calibri;font-size: 16px;\">";
            //html += "                   <b>Y</b>outube&nbsp;";
            html += "                   <a href=\"http://youtube.com\" style=\"font-size:small;color:green;text-decoration:none;font-family:Calibri\">";
            html += "                       source: youtube.com";
            html += "                   </a>";
            html += "               </td>";
            html += "           </tr>";

            for (int index = 0; index < 1; index++)//this.videos.Count
            {
                hasData = true;
                YTVideo video = this.videos[index];
                html += "       <tr>";
                html += "           <td style=\"padding-bottom: 3px;\">";
                if (index == 0)
                {
                    html += String.Format("<object width=\"100%\" height=\"150px\"><param name=\"movie\" value=\"http://www.youtube.com/v/{0}&hl=en&fs=1&\"></param><param name=\"allowFullScreen\" value=\"true\"></param><param name=\"allowscriptaccess\" value=\"always\"></param><embed src=\"http://www.youtube.com/v/{0}&hl=en&fs=1&\" type=\"application/x-shockwave-flash\" allowscriptaccess=\"always\" allowfullscreen=\"true\" width=\"100%\" height=\"150px\"></embed></object><br />", video.Id);
                }

                html += "               <a target=\"_blank\" href=\"" + video.UrlLink + "\" style=\"color:Blue;font-family: Calibri; font-size: medium\">";
                html += video.Title;
                html += "               </a><br />";
                html += "               <span style=\"font-family: Calibri; font-size: small\">";
                //html += video.Description;
                html += "               </span>";
                html += "           </td>";
                html += "       </tr>";
                hasData = true;
            }
//            html += "           <tr><td style=\"height: 3px\">&nbsp;</td></tr>";
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

                YouTube yTube = new YouTube();
                return yTube.SearchVideos(query, Int32.Parse(data.Split('|')[0]), Int32.Parse(data.Split('|')[1]));
            }
            catch { }

            return null;
        }
        #endregion
    }
}