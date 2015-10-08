using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omuk.OmukEngine.Language
{
    /// <summary>
    /// 
    /// </summary>
    public static class OLangKeywords
    {
        public static String[] Constructs = new String[] 
                                    {
                                        "music:artist",
                                        "music:album",
                                        "music:song",
                                        "music:lyric",
                                        "social:concert",
                                        "travel:flight",
                                        "travel:direction",
                                        "nocontext:skip"
                                    };
    }

    /// <summary>
    /// 
    /// </summary> 
    public class ODimension
    {
        public String PrimaryContext { get; set; }
        public String secondaryContext { get; set; }
        public String Keyword { get; set; }
        private List<ODimension> siblings = new List<ODimension>();

        public List<ODimension> Siblings
        {
            get { return siblings; }
            set { siblings = value; }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class OmukLanguage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Boolean IsLanguageQuery(ref String query)
        {
            if (String.IsNullOrEmpty(query))
                return false;

            if (query.ToLower().StartsWith("p:"))
            {
                query = query.Substring(0, "p:".Length);
                if (IsSyntaxCorrect(query))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static Boolean IsSyntaxCorrect(String query)
        {
            return true;
        }
    }
}
