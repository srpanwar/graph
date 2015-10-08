using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using com.eventful.www;

namespace Omuk.OmukControls
{
    class EventfulControl:OmukControl
    {
        EventCollection collection = null;

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
            get { return "eventful"; }
        }

        public String GetHtmlContent(string searchText, String data, String sense, Object source, Dictionary<String, String> uniqueLinks)
        {
            searchText = HttpUtility.UrlDecode(searchText);

            if (source == null)
                source = this.GetSource(searchText, data);

            if (source == null)
                throw new ArgumentNullException("Source is NULL");

            this.collection = source as EventCollection;
            if (this.collection == null)
                throw new InvalidCastException("Source is not of correct type");

            bool hasData = false;
            String html = String.Empty;
            if (this.collection.Events.Count > 0)
            {
                hasData = true;
                html += "<td id=\"tdEventful\" style=\"width: 100%;vertical-align: top\">";
                html += "   <table style=\"width: 100%\" cellpadding=\"0\" cellspacing=\"0\">";
                html += "       <tr>";
                html += "           <td style=\"border-bottom: dotted 1px Silver; width: 100%; color: Black; font-family: Calibri;font-size: 16px;\">";
                html += "               <b>E</b>vent(s)&nbsp;<a target=\"_blank\" href=\"http://eventful.com\" style=\"font-size:small;color:green;text-decoration:none;font-family:Calibri\">";
                html += "                   source: eventful";
                html += "               </a>";
                html += "           </td>";
                html += "       </tr>";
                html += "       <tr>";
                html += "           <td align=\"left\">";
                html += "               <table align=\"left\" style=\"width: 100%;height:50px;font-family:Calibri\" cellpadding=\"0\" cellspacing=\"0\">";

                foreach (EEvent evnt in this.collection.Events)
                {
                    DateTime start = DateTime.MaxValue;
                    String title = String.IsNullOrEmpty(evnt.Title) ? evnt.Description : evnt.Title;
                    if (DateTime.TryParse(evnt.StartTime, out start))
                        title += "<span style=\"font-size:smallest\"><b>&nbsp;&nbsp;(" + start.ToString("d MMM, HH:mm") + ")</b></span>"; 
                    html += "               <tr>";
                    html += "                   <td style=\"width: 41px;height:41px;;vertical-align:top;\" align=\"center\">";
                    html += "                       <a href=\"" + evnt.UrlLink + "\" style=\"text-decoration:none\"";
                    html += "                           <img style=\"border:none;width: 40px;height:40px;\" src=\"" + evnt.ImageThumb + "\" alt=\"event\" />";
                    html += "                       </a>";
                    html += "                   </td>";
                    html += "                   <td style=\"width: 5px;\">&nbsp;</td>";
                    html += "                   <td style=\"vertical-align:top;font-family:Calibri;font-size:14px;\" align=\"left\">";
                    html += title;
                    html += "                   </td>";
                    html += "               </tr><tr><td colspan=\"3\" style=\"height:3px;border-top:dotted 1px Silver;\"></td></tr>";
                }

                html += "               </table>";
                html += "            </td>";
                html += "       </tr>";
                html += "       <tr><td align=\"right\"><a href=\"http://eventful.com/" + searchText + "\" style=\"font-size:12px;;font-family:Calibri\">more events</a></td></tr>";
                html += "       <tr><td style=\"height:2px;\"></td></tr>";
                html += "   </table>";
                html += "</td>";
                
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

                Eventful eventful = new Eventful();
                return eventful.SearchEvents(query,0, 6);
            }
            catch { }

            return null;
        }
        #endregion
    }
}
