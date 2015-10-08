using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using org.musicbrainz.www;

namespace Omuk.OmukEngine
{
    // http://www.imdb.com/title
    // http://www.rottentomatoes.com/m

    public enum Context
    {
        Music,
        movie,
        Artist,
        Album,
        Song
    }

    public class OmukSemantics
    {
        Dictionary<String, int> senseRank = new Dictionary<string, int>();
        Dictionary<String, int> miscWords = new Dictionary<string, int>();
        List<String> keyWords = new List<string>();
        List<String> seperators = new List<string>() { "-", "|", "(", ")", "\"", ":", ",", ((char)(8211)).ToString() };
        public List<String> KeyWords
        {
            get { return keyWords; }
            set { keyWords = value; }
        }

        public Dictionary<String, int> MiscWords
        {
            get { return miscWords; }
            set { miscWords = value; }
        }

        private Boolean isPatterned = false;

        public Boolean IsPatterned
        {
            get { return isPatterned; }
            set { isPatterned = value; }
        }
        private String category = String.Empty;

        public String Category
        {
            get 
            {
                if (senseRank["movie"] >= senseRank["music"] &&
                    senseRank["movie"] >= senseRank["news"] && 
                    senseRank["movie"] > 7)
                    return "movie";

                if (senseRank["music"] >= senseRank["movie"] &&
                    senseRank["music"] >= senseRank["news"] && 
                    senseRank["music"] > 7)
                    return "music";

                if (senseRank["news"] >= senseRank["movie"] && 
                    senseRank["news"] >= senseRank["music"] &&
                    senseRank["news"] > 7)
                    return "news";

                return "knowledge";
            }
            set { category = value; }
        }
        Dictionary<String, String> relevantLinks = new Dictionary<string, string>();

        public Dictionary<String, String> RelevantLinks
        {
            get { return relevantLinks; }
            set { relevantLinks = value; }
        }

        public OmukSemantics()
        {
            senseRank["movie"] = 0;
            senseRank["music"] = 0;
            senseRank["knowledge"] = 0;
            senseRank["news"] = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private string SimpleMatchEvaluator(Match match)
        {
            if (this.seperators.Contains(match.Value))
                return String.Format(" {0} ", match.Value);
            return " ";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private string SimpleMatchEvaluatorSpace(Match match)
        {
            return " ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public void ProcessContext(params String[] query)
        {
            if (query.Length != 3)
                return;

            String title = query[0];
            String url = query[1];
            String desc = query[2];
            String[] titleIgnored = OmukDB.Instance.GetIgnored("title", "ignore");
            foreach (String ignoreTxt in titleIgnored)
            {
                if (title.StartsWith(ignoreTxt, StringComparison.InvariantCultureIgnoreCase))
                    title = title.Remove(0, ignoreTxt.Length);
            }

            title = title.Replace("<b>", String.Empty).Replace("</b>", String.Empty);
            desc = desc.Replace("<b>", String.Empty).Replace("</b>", String.Empty);
            Regex regex = new Regex(@"\W");
            title = regex.Replace(title, this.SimpleMatchEvaluator);
            desc = regex.Replace(desc, this.SimpleMatchEvaluator);

            regex = new Regex(@"\s+");
            title = regex.Replace(title, this.SimpleMatchEvaluatorSpace);
            desc = regex.Replace(desc, this.SimpleMatchEvaluatorSpace);

            String[] titleparts = title.Split(' ');
            for (int index = 0; index < titleparts.Length; index++)
            {
                String titlepart = titleparts[index].ToLower();
                if (this.miscWords.ContainsKey(titlepart))
                {
                    this.miscWords[titlepart]++;
                    int count = this.miscWords[titlepart];
                }
                else
                    this.miscWords[titlepart] = 1;
            }
            

            if (!String.IsNullOrEmpty(url))
            {
                senseRank["movie"] += OmukDB.Instance.GetScore("movie", "link", url.ToLower());

                senseRank["movie"] += OmukDB.Instance.GetScore("movie", "keyword", title.ToLower());

                senseRank["movie"] += OmukDB.Instance.GetScore("movie", "keyword", desc.ToLower());

                senseRank["music"] += OmukDB.Instance.GetScore("music", "link", url.ToLower());

                senseRank["music"] += OmukDB.Instance.GetScore("music", "keyword", title.ToLower());

                senseRank["music"] += OmukDB.Instance.GetScore("music", "keyword", desc.ToLower());

            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        public void PostSearch()
        {
            String fullWord = String.Empty;
            foreach (KeyValuePair<String, int> keyValue in this.MiscWords)
            {
                if (OmukDB.Instance.IsIgnored("words", "ignore", keyValue.Key))
                    continue;

                if (keyValue.Value >= 5 && !seperators.Contains(keyValue.Key) && !String.IsNullOrEmpty(keyValue.Key))
                {
                    fullWord += keyValue.Key + " ";
                }
                else
                {
                    if (!String.IsNullOrEmpty(fullWord))
                    {
                        keyWords.Add(fullWord.Trim());
                        fullWord = String.Empty;
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void FormContext()
        {
            if (this.Category.Equals("music"))
            {
                MusicBrainz mb = new MusicBrainz();
                String artist = String.Empty;
                String song = String.Empty;
                string mbid = String.Empty;
                int wStrength = Int32.MaxValue;
                int artistIndex = 0;
                String[] kWArr = this.KeyWords.ToArray();
                this.keyWords.Clear();

                for (int index = 0; index < kWArr.Length; index++)
                {
                    String txt = kWArr[index].ToLower();
                    List<MBArtist> artists = mb.GetArtists(txt, 0, 2);
                    if (artists != null)
                    {
                        foreach (MBArtist mbartist in artists)
                        {
                            String largeTxttmp = mbartist.Name.ToLower();
                            int weight = this.MapReplace(txt, largeTxttmp);

                            if (weight <= 3 && weight <= wStrength)
                            {
                                wStrength = weight;
                                artist = mbartist.Name;
                                mbid = mbartist.MBID;
                                artistIndex = index;
                            }
                        }
                    }
                }

                artistIndex = artistIndex == 0 ? 1 : 0;
                if (!String.IsNullOrEmpty(artist) && artistIndex < kWArr.Length)
                {   
                    List<MBSong> songs = mb.GetSongs(kWArr[artistIndex], artist, String.Empty, String.Empty, String.Empty, 0, 3);


                    if (songs != null && songs.Count > 0)
                        song = songs[0].Title;
                    else
                    {
                        org.lyricwiki.www.service.LyricWiki lw = new org.lyricwiki.www.service.LyricWiki();
                        org.lyricwiki.www.service.LyricsResult lyrics = lw.getSong(artist, kWArr[artistIndex]);
                        if (lyrics != null)
                        {
                            song = lyrics.song;
                        }
                    }
                }

                this.keyWords.Add("artist:" + artist);
                this.keyWords.Add("album:" + String.Empty);
                this.keyWords.Add("song:" + song);
                this.keyWords.Add("mbid:" + mbid);
            }

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="replaceText"></param>
        private int MapReplace(String text1, String text2)
        {
            int w1 = 0;
            int w2 = 0;
            foreach (char c in text1)
                w1 += c;
            foreach (char c in text2)
                w2 += c;

            return (int)(Math.Abs(w1 - w2) * 1.0 / 110);
        }
    }
}
