using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

using org.wikipedia.www;
using com.yahoo.search;
using org.lyricwiki.www.service;
using com.grooveshark.www;
using last.fm;
using com.imeem.www;
using org.musicbrainz.www;
using com.bing.www.search;
using dictionary.edu.princeton;
using com.youtube.www;
using Omuk.OmukEngine;
using com.rottentomatoes.www;
using com.twitter.www;
using com.digg.www;
using com.eventful.www;

namespace mashup
{
    class Program
    {
        static void Main(string[] args)
        {
            //NounCollection coll = NounCollection.Instance;
            //String [] related = null;
            //Console.WriteLine(coll.Nouns["hello"].GetDescription(0, out related));

            //using (StreamWriter writer = new StreamWriter(File.Create(@"\1.txt")))
            //{
            //    for (int index = 0; index < 256; index++)
            //        writer.WriteLine(String.Concat(index, " ", (char)index));  
            //}
            //Console.WriteLine((int)'e');//'é');

            //BingSearch search = new BingSearch(); 
            //search.SearchWeb("apj kalam");

            //Wikipedia wClass = new Wikipedia(new WikiQueryEngine());
            //Console.WriteLine(wClass.IsArticleAvailable("Plain white T's"));
            //SearchSuggestion searchSuggest = wClass.SearchArticles("Plain white T's", 10);
            //foreach (SearchSuggestionItem sItem in searchSuggest.Section)
            //{
            //    Console.WriteLine(sItem.Text + "\n\t" + sItem.Description + "\n" + sItem.Url);
            //}
            //Console.WriteLine(wClass.GetArticle("Seattle", false).Introduction);

            OmukSemantics semantics = new OmukSemantics();
            //YSearch search = new YSearch();
            //YSearchResultCollection resCollection = search.SearchWeb("me genera serotonina listening to Viva la Vida by Coldplay", 0, 10, ref semantics);
            BingSearch bsearch = new BingSearch();
            bsearch.SearchWeb("Am I Dreaming by Xscape", 0, 10, ref semantics);
            semantics.PostSearch();
            semantics.FormContext();
            
            LyricWiki wiki = new LyricWiki();
            LyricsResult lyrics = wiki.getSong("Rihanna", "Umbrella");
            Console.WriteLine(lyrics.lyrics);
            //Console.WriteLine("");
            LastFmService service = new LastFmService();
            //service.GetArtistInfo(
    
            //MusicBrainz mb = new MusicBrainz();
           // mb.GetAlbums

            //Grooveshark gshark = new Grooveshark();
            //GSSong[] song = gshark.SearchSong("Sugar", 2);

            //LyricWiki wiki = new LyricWiki();
            //LyricsResult lyrics = wiki.getSong("R.e.m", "Moon river");
            //lyrics.GetLyricsHtml();
            //SongResult songs = wiki.searchSongs("R.e.m", "Moon river");
            

            //String artist = "Flo rida";
            //String album = "Sugar";
            //int year = 2009;
            //String[] albums = null;
            //String res = wiki.getAlbum(ref artist, ref album, ref year, out albums);

            //AlbumData[] album = (AlbumData[])wiki.getArtist(ref artist);
            //LWLyrics lyrics = wiki.GetLyricsForSong("Flo rida", "sugar");
            //Console.WriteLine(wiki.IsArtist("rihanna"));
            //String[] albums = wiki.GetAlbums("rihanna");


            //LastFmService service = new LastFmService();
            //LFMArtist artist = service.GetArtistInfo("Rihanna");
            //List<LFMArtist> albums = service.SearchArtist("Kid Cudi", 0);

            //Immem immem = new Immem();
            //List<ImmemSearchItem> items = immem.SearchMedia("Rihanna good girl gone bad", "music", 0, 10);

            MusicBrainz mb = new MusicBrainz();
            List<MBAlbum> albums = mb.GetAlbums(String.Empty, "Rihanna", "73e5e69d-3554-40d8-8516-00cb38737a1c", 0, 10);
            List<MBSong> songs = mb.GetSongs("oh Carol", "Neil", "", "", "", 0, 2);

            //YouTube yt = new YouTube();
            //List<YTVideo> videos = yt.SearchVideos("oh carol", 0, 3);

            //RottenTomatoes rt = new RottenTomatoes();
            //String val = rt.GetTomatometerScore("http://www.rottentomatoes.com/m/1009437-heidi/");

            /*
            Twitter twitter = new Twitter();
            ////twitter.SearchTwitter("i gotta feeling", 0, 10);
            TMovieTrendsClass mTrends = new TMovieTrendsClass();
            twitter.GetMovieTrends("The Martian", ref mTrends);
            Console.WriteLine("");
            */

            //twitter.GetMovieTrends("\"the proposal\"",ref mTrends);
            //twitter.GetMovieTrends("\"year one\"", ref mTrends);
            //twitter.GetMovieTrends("\"Terminator Salvation\"", ref mTrends);
            //twitter.GetMovieTrends("\"Next day air\"", ref mTrends);
            //twitter.GetMovieTrends("\"Imagine That\"", ref mTrends);
            //twitter.GetMovieTrends("\"Taking of Pelham 1 2 3", ref mTrends);
            //twitter.GetMovieTrends("Wolverine", ref mTrends);
            //using (StreamWriter writer = new StreamWriter(File.Create("trend.txt")))
            //{
            //    foreach (KeyValuePair<String, int> keyval in mTrends.Trend)
            //    {
            //        if (keyval.Value > 3)
            //            writer.WriteLine(String.Format("{0} {1}", keyval.Key, keyval.Value));
            //    }
            //}

            //TSearchResultCollection twRes = twitter.SearchTwitter("palm pre", 0, 15);


            //Eventful evntful = new Eventful ();
            //EventCollection coll = evntful.SearchEvents("San Jose", 0, 10);
            //Digg digg = new Digg();
            //List<DiggResult> res = digg.SearchDigg("Hello", 0, 10);
            return;
        }
    }
}

