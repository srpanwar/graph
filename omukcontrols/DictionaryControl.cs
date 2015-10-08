using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using dictionary.edu.princeton;

namespace Omuk.OmukControls
{
    /// <summary>
    /// Summary description for DictionaryControl
    /// </summary>
    public class DictionaryControl : OmukControl
    {
        protected Dictionary<String[], String[]> meanings = new Dictionary<string[], string[]>();
        public DictionaryControl()
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
                return "meaning";
            }
        }

        public String Area
        {
            get
            {
                return "above-search-listing";
            }
        }
        public String Name
        {
            get { return "dictionary"; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="source"></param>
        /// <param name="uniqueLinks"></param>
        /// <returns></returns>
        public string GetHtmlContent(string searchText, String data,String sense, object source, Dictionary<string, string> uniqueLinks)
        {
            if (source == null)
                source = this.GetSource(searchText);

            if (source == null)
                return String.Empty;

            this.meanings = source as Dictionary<String[], String[]>;
            if (source == null)
                throw new InvalidCastException("Source is not of correct type");

            String html = String.Empty;

            if (this.meanings.Count > 0)
            {
                html += "<td style=\"font-family:Calibri;font-size:14px; border:dotted 1px silver;border-top:none;\">";
                foreach (KeyValuePair<String[], String[]> keyval in this.meanings)
                {
                    html += "<b style=\"font-size:14px\">" + keyval.Key[0] + "</b>&nbsp;<small>(noun)</small>&nbsp;";
                    for (int indexD = 0; indexD < keyval.Value.Length; indexD++)
                    {
                        String val = keyval.Value[indexD];
                        html += "<span style=\"border-bottom:dotted 1px Grey;font-size:13px\">" + val + "</span>,&nbsp";
                    }
                    html += "<br /><i style=\"font-size:14px\">" + keyval.Key[1] + "</i><br />";
                }
                html += "</td>";
            }
            return html;
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

                Dictionary<String[], String[]> meanings = new Dictionary<string[], string[]>();
                String[] queryparts = query.Split(' ');
                if (queryparts.Length < 3)
                {
                    foreach (String querypart in queryparts)
                    {
                        if (NounCollection.Instance.Nouns.ContainsKey(querypart.ToLower()))
                        {
                            String[] related = null;
                            String desc = NounCollection.Instance.Nouns[querypart.ToLower()].GetDescription(0, out related);
                            meanings.Add(new String[] { querypart, desc }, related);
                        }
                    }
                    return meanings;
                }
            }
            catch { }
            return null;
        }
        #endregion
    }
}