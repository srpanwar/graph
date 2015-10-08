using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace org.wikipedia.www.lang
{
    /// <summary>
    /// 
    /// </summary>
    public class LangCode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="alpha3bib"></param>
        /// <param name="alpha3term"></param>
        /// <param name="alpha2"></param>
        /// <param name="eng"></param>
        /// <param name="french"></param>
        internal LangCode(String alpha3bib, String alpha3term, String alpha2, String eng, String french)
        {
            this.Alpha3Bib = alpha3bib;
            this.Alpha3Term = alpha3term;
            this.Alpha2 = alpha2;
            this.LangEn = eng;
            this.LangFre = french;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parts"></param>
        internal LangCode(String[] parts)
        {
            this.Alpha3Bib = parts[0];
            this.Alpha3Term = parts[1];
            this.Alpha2 = parts[2];
            this.LangEn = parts[3];
            this.LangFre = parts[4];
        }

        public String Alpha3Bib { get; private set; }
        public String Alpha3Term { get; private set; }
        public String Alpha2 { get; private set; }
        public String LangEn { get; private set; }
        public String LangFre { get; private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LangCodeCollection : List<LangCode>
    {
        private static LangCodeCollection instance = null;
        public LangCode this[String language]
        {
            get
            {
                LangCode langCode = this.Find(lc => lc.LangEn.Equals(language, StringComparison.InvariantCultureIgnoreCase));
                return langCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private LangCodeCollection() 
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("org.wikipedia.www.lang.ISO-639-2_utf-8.txt");
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    String entry = reader.ReadLine();
                    if (!String.IsNullOrEmpty(entry))
                    {
                        String[] parts = entry.Split('|');
                        try
                        {
                            this.Add(new LangCode(parts));
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static LangCodeCollection Instance
        {
            get
            {
                if (instance == null)
                    instance = new LangCodeCollection();
                return instance;
            }
        }
    }
}
