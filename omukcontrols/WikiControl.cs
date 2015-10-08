using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using org.wikipedia.www;

namespace Omuk.OmukControls
{
    /// <summary>
    /// Summary description for WikiControl
    /// </summary>
    public class WikiControl : OmukControl
    {
        private SearchSuggestion suggestion = null;
        public WikiControl()
        {

        }

        #region OmukControl Members
        public String Sense
        {
            get
            {
                return "knowledge:place";
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
            get { return "wikipedia"; }
        }

        public String GetHtmlContent(string searchText, String data,String sense, Object source, Dictionary<String, String> uniqueLinks)
        {
            if (source == null)
                source = this.GetSource(searchText);

            if (source == null)
                throw new ArgumentNullException("Source is NULL");

            this.suggestion = source as SearchSuggestion;
            if (source == null)
                throw new InvalidCastException("Source is not of correct type");

            if (this.suggestion.Section == null)
                throw new ArgumentException("No Data found");

            if (this.suggestion.Section.Length == 0)
                throw new ArgumentException("No Data found");

            bool hasData = false;
            String html = String.Empty;
            html += "<tr>";
            html += "   <td id=\"tdWikipedia\" style=\"width: 100%;vertical-align: top\">";
            html += "       <table style=\"width: 100%\" cellpadding=\"0\" cellspacing=\"0\">";
            html += "           <tr>";
            html += "               <td style=\"border-bottom: solid 1px Silver; width: 100%; color: Black; font-family: Calibri;font-size: 16px;\">";
            html += "                   <b>R</b>elated article(s)&nbsp;";
            html += "                   <a href=\"http://wikipedia.org\" style=\"font-size:small;color:green;text-decoration:none;font-family:Calibri\">";
            html += "                       source: wikipedia.org";
            html += "                   </a>";
            html += "               </td>";
            html += "           </tr>";

            for (int index = 0; index < this.suggestion.Section.Length && index < 5; index++)
            {
                SearchSuggestionItem sugItem = this.suggestion.Section[index];
                if (uniqueLinks.ContainsKey(HttpUtility.UrlDecode(sugItem.Url.ToLower())) || uniqueLinks.ContainsKey(HttpUtility.UrlEncode(sugItem.Url.ToLower())))
                    continue;
                else
                    uniqueLinks.Add(HttpUtility.UrlDecode(sugItem.Url.ToLower()), String.Empty);
                html += "       <tr>";
                html += "           <td style=\"padding-bottom: 3px;\">";
                html += "               <a target=\"_blank\" href=\"" + sugItem.Url + "\" style=\"color:Blue;font-family: Calibri; font-size: medium\">";
                html += sugItem.Text;
                html += "               </a><br />";
                html += "               <span style=\"font-family: Calibri; font-size: small\">";
                html += sugItem.Description;
                html += "               </span>";
                html += "           </td>";
                html += "       </tr>";
                hasData = true;
            }
            html += "       </table>";
            html += "   </td>";
            html += "</tr>";
            html += "<tr><td style=\"height: 5px\">&nbsp;</td></tr>";

            return hasData ? html : String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private Object GetSource(String query)
        {
            try
            {
                if (String.IsNullOrEmpty(query))
                    return null;

                Wikipedia wiki = new Wikipedia(new WikiQueryEngine());
                SearchSuggestion suggestion = wiki.SearchArticles(query, 10);
                WikiControl omukControl = new WikiControl();
                return suggestion;
            }
            catch { }

            return null;
        }
        #endregion
    }
}