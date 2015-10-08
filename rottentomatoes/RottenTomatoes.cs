using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;

namespace com.rottentomatoes.www
{
    public class RottenTomatoes
    {
        public String GetTomatometerScore(String url)
        {
            String score = String.Empty;
            try
            {
                String endpoint = url;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endpoint);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    String txt = reader.ReadToEnd();
                    txt = txt.Replace("\r", String.Empty).Replace("\n", String.Empty);
                    Regex regex = new Regex("(<div(\\s+)id=\"tomatometer_score\"(.+?)>)(\\s*)(<span(\\s+)class=\"(percent|perfect)\">)(.*?)(</span>)(\\s*)(.+?)(</div>)");
                    if (regex.IsMatch(txt))
                    {
                        score = regex.Matches(txt)[0].Groups[8].Value;
                    }
                }
            }
            catch { }
            return score;
        }
        
    }
}
