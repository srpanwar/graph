using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace com.twitter.www
{
    public class TMovieTrendsClass
    {
        public int Scale { get; set; } 
        //public Dictionary<String, int> Trend { get; set; }
        public int Positive { get; set; }
        public int Negative { get; set; }
        private List<TwitterDBScore> dbScoreList = null;
       
        /// <summary>
        /// 
        /// </summary>
        public TMovieTrendsClass()
        {
            this.dbScoreList = TwitterDB.Instance.GetScore("movie", "keywords");
            this.Scale = 200;
            //this.Trend = new Dictionary<string, int>();
            //this.Trend.Add("good", 0);
            //this.Trend.Add("ok", 0);
            //this.Trend.Add("bad", 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private string SimpleMatchEvaluator(Match match)
        {
            if (match.Value.Equals("'"))
                return match.Value;
            return " ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchResColl"></param>
        public void FormTrend(TSearchResultCollection searchResColl)
        {
            if (searchResColl == null || searchResColl.Items == null || searchResColl.Items.Count == 0)
                return;
            Regex regex = new Regex(@"\W");
            foreach (TSearchResult result in searchResColl.Items)
            {
                String text = result.Content.Replace("<b>", string.Empty).Replace("</b>", string.Empty);
                text = regex.Replace(text, this.SimpleMatchEvaluator);
                text = text.ToLowerInvariant();
                foreach (TwitterDBScore dbScore in this.dbScoreList)
                {
                    if (text.Contains(" " + dbScore.PrimaryWord + " "))
                    {
                        int startPosition = text.IndexOf(dbScore.PrimaryWord);
                        bool found = false;
                        int newseek = (startPosition - 15) < 0 ? 0 : startPosition - 15;
                        foreach (KeyValuePair<string, int> keyVal in dbScore.StrenghWords)
                        {
                            if (text.Contains(keyVal.Key) && (text.IndexOf(keyVal.Key, newseek) < startPosition && text.IndexOf(keyVal.Key, newseek) != -1))
                            {
                                found = true;
                                if (keyVal.Value > 0)
                                    this.Positive += keyVal.Value;
                                if (keyVal.Value < 0)
                                    this.Negative += keyVal.Value;
                            }
                        }
                        if (!found)
                        {
                            if (dbScore.PrimaryWordStrength > 0)
                                this.Positive += dbScore.PrimaryWordStrength;
                            if (dbScore.PrimaryWordStrength < 0)
                                this.Negative += dbScore.PrimaryWordStrength;
                        }
                    }
                }
                //String[] parts = text.Split(' ');
                //foreach (String part in parts)
                //{
                //    String tpart = part.ToLowerInvariant().Trim();
                //    if (this.Trend.ContainsKey(tpart))
                //        this.Trend[tpart] += 1;
                //    else
                //        this.Trend.Add(tpart, 1);
                //}
            }
        }
    }
}
