using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omuk.OmukEngine.Language;
using org.musicbrainz.www;

namespace Omuk.OmukEngine
{
    /// <summary>
    /// 
    /// </summary>
    public static class OmukEngine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<ODimension> GetParsedDimension(String query)
        {
            List<ODimension> dimensions = new List<ODimension>();
            if (String.IsNullOrEmpty(query))
                return null;

            String[] parts = query.Split(',', ';', '|');
            foreach (String part in parts)
            {
                if (String.IsNullOrEmpty(part.Trim()))
                    continue;

                String[] subparts = part.Split(':');
                subparts = subparts.Length == 1 ? OmukEngine.FormContext(part).Split(':') : subparts;

                if (subparts != null && subparts.Length > 0)
                {
                    String subpartKey = subparts[0].Trim();
                    String subpartValue = subparts[1].Trim();

                    String context = Array.Find(OLangKeywords.Constructs, cons => cons.Split(':')[1].Equals(subpartKey, StringComparison.InvariantCultureIgnoreCase));
                    if (!String.IsNullOrEmpty(context))
                    {
                        ODimension dimension = new ODimension();
                        dimension.PrimaryContext = context.Split(':')[1];
                        dimension.Keyword = subpartValue;
                        dimensions.Add(dimension);
                    }
                }
            }

            return dimensions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        private static string FormContext(string part)
        {
            if (OmukEngine.IsArtist(part))
                return String.Format("artist:{0}", part);

            if (part.ToLower().Equals("album") || part.ToLower().Equals("albums"))
                return String.Format("album:{0}", part);

            if (part.ToLower().Equals("song") || part.ToLower().Equals("songs"))
                return String.Format("song:{0}", part);

            return String.Format("skip:{0}", part);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static Boolean IsArtist(String query)
        {
            MusicBrainz mb = new MusicBrainz();
            MBArtist artist = null;
            return mb.IsArtist(query, out artist);
        }
    }
}
