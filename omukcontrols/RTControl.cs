using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using com.rottentomatoes.www;

namespace Omuk.OmukControls
{
    /// <summary>
    /// Summary description for RTControl
    /// </summary>
    public class RTControl : OmukControl
    {
        public RTControl()
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
                return "movie";
            }
        }
        public String Area
        {
            get
            {
                return "under-search-item";
            }
        }

        public string Name
        {
            get { return "rottentomatoes"; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="data"></param>
        /// <param name="Source"></param>
        /// <param name="uniqueLinks"></param>
        /// <returns></returns>
        public string GetHtmlContent(string searchText, String data,String sense, object Source, Dictionary<string, string> uniqueLinks)
        {
            if (String.IsNullOrEmpty(data))
                return String.Empty;

            RottenTomatoes rt = new RottenTomatoes();
            data = HttpUtility.UrlDecode(data);
            String score = rt.GetTomatometerScore(data);
            int scoreInt = Int32.Parse(score);
            String html = String.Empty;
            html += "<td style=\"height: 10px; font-family: Trebuchet MS; font-size: 13px\">";
            html += " <table style=\"border:none\" cellpadding=\"0\" cellspacing=\"0\">";
            html += "     <tr>";
            html += "         <td style=\"font-family: Trebuchet MS;font-size:12px;\">";
            html += "             Tomato score&nbsp;: " + scoreInt + "%&nbsp;";
            html += "         </td>";
            html += "         <td>";
            html += "             <table style=\"border:none\" cellpadding=\"0\" cellspacing=\"0\">";
            html += "                 <tr>";
            html += "                     <td style=\"height: 5px; width: " + scoreInt + "px; background-color: #C85F08; border: solid 1px silver\"></td>";
            html += "                     <td style=\"width: " + (100 - scoreInt) + "px; border: solid 1px silver; border-left: none\"></td>";
            html += "                 </tr>";
            html += "             </table>";
            html += "         </td>";
            html += "     </tr>";
            html += " </table>";
            html += "</td>";

            return html;
        }

        #endregion
    }
}