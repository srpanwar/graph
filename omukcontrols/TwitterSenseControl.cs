using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using com.twitter.www;

namespace Omuk.OmukControls
{
    class TwitterSenseControl: OmukControl
    {
        TMovieTrendsClass tMovieTrendClass = new TMovieTrendsClass();

        #region OmukControl Members
        public String Sense
        {
            get
            {
                return "movie";
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
            get { return "twitter"; }
        }

        public String GetHtmlContent(string searchText, String data, String sense, Object source, Dictionary<String, String> uniqueLinks)
        {
            searchText = HttpUtility.UrlDecode(searchText);
            if (sense == "movie")
                searchText = String.Format("\"{0}\" movie", searchText);

            if (source == null)
                source = this.GetSource(searchText, data);

            if (source == null)
                throw new ArgumentNullException("Source is NULL");

            this.tMovieTrendClass = source as TMovieTrendsClass;
            if (source == null)
                throw new InvalidCastException("Source is not of correct type");

            bool hasData = false;
            String html = String.Empty;
            if (this.tMovieTrendClass.Positive > 30)
            {
                hasData = true;
                //this.tMovieTrendClass.Negative = this.tMovieTrendClass.Negative == 0 ? -1 : this.tMovieTrendClass.Negative;
                this.tMovieTrendClass.Negative = 0 - this.tMovieTrendClass.Negative;
                int total = this.tMovieTrendClass.Positive + this.tMovieTrendClass.Negative;
                html += "   <td id=\"tdtwitterSense\" style=\"width: 100%;vertical-align: top\">";
                html += "       <table style=\"width: 100%\" cellpadding=\"0\" cellspacing=\"0\">";
                html += "           <tr>";
                html += "               <td style=\"border-bottom: dotted 1px Silver; width: 100%; color: Black; font-family: Calibri;font-size: 16px;\">";
                html += "                   <a target=\"_blank\" href=\"http://search.twitter.com/search?q=" + HttpUtility.UrlEncode(searchText) +"\" style=\"font-size:small;color:green;text-decoration:none;font-family:Calibri\">";
                html += "                       Twitter Buzz";
                html += "                   </a>";
                html += "               </td>";
                html += "           </tr>";
                html += "           <tr>";
                html += "               <td align=\"center\">";
                html += "                   <table align=\"center\" style=\"width: 100%;height:50px;font-family:Calibri\" cellpadding=\"0\" cellspacing=\"0\">";
                html += "                       <tr>";
                html += "                           <td style=\"width: 25%;height:50px;\" align=\"center\">";
                html += "                                   <img src=\"images/positive.png\" alt=\"Good\" />";
                html += "                           </td>";
                html += "                           <td style=\"width: 25%;height:50px;\" align=\"center\">";
                html += (int)(this.tMovieTrendClass.Positive * 100.0 / total * 1.0) + "%";
                html += "                           </td>";
                html += "                           <td style=\"width: 25%;height:50px;\" align=\"center\">";
                html += "                                   <img src=\"images/negative.png\" alt=\"Bad\" />";
                html += "                           </td>";
                html += "                           <td style=\"width: 25%;height:50px;\" align=\"center\">";
                html += (int)(this.tMovieTrendClass.Negative * 100.0 / total * 1.0) + "%"; 
                html += "                           </td>";
                html += "                       </tr><tr><td align=\"center\" colspan=\"4\" style=\"font-size:smaller;\">" + searchText + "</td></tr>";
                html += "                   </table>";
                html += "               </td>";
                html += "           </tr>";
                html += "           <tr><td style=\"height: 5px\">&nbsp;</td></tr>";
                html += "       </table>";
                html += "   </td>";
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

                Twitter twitter = new Twitter();
                twitter.GetMovieTrends(query, ref this.tMovieTrendClass, 10);
                return this.tMovieTrendClass;
            }
            catch { }

            return null;
        }
        #endregion
    }
}
