using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using org.lyricwiki.www.service;

namespace org.lyricwiki.www.service
{
    public static class LyricsResultEx
    {
        public static String GetLyricsHtml(this LyricsResult lresult)
        {
            String lyrichtml = String.Empty;
            try
            {
                String endpoint = lresult.url;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    String txt = reader.ReadToEnd();
                    txt = txt.Replace("\r", String.Empty).Replace("\n", String.Empty);
                    Regex regex = new Regex("(<div(\\s+)class=(\"|')lyricbox(\"|')(\\s*)>)(.*?)(<div(\\s*)>)(.*?)(</div>)(.*?)(</div>)");
                    if (regex.IsMatch(txt))
                    {
                        lyrichtml = regex.Matches(txt)[0].Groups[6].Value;
                    }
                }
            }
            catch { }
            return lyrichtml;
        }
    }
}
