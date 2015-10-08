using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace dictionary.edu.princeton
{
    /// <summary>
    /// 
    /// </summary>
    public class Noun
    {
        public String Lemma { get; set; }
        public String Pos { get; set; }
        public String SynseCnt { get; set; }
        public String PCnt { get; set; }
        public List<String> PtrSymbol { get; set; }
        public String SenseCnt { get; set; }
        public String TagsenseCnt { get; set; }
        public List<Int64> SynsetOffset { get; set; }

        public String GetDescription(int index, out String[] related)
        {
            related = null;
            List<String> rList = new List<string>();
            String desc = String.Empty;
            using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("dictionary.wordnet.data.noun")))
            {
                reader.BaseStream.Seek(this.SynsetOffset[index], SeekOrigin.Begin);
                String descLine = reader.ReadLine();
                String[] parts = descLine.Split('|');
                if (parts.Length > 0)
                {
                    desc = parts[1];
                    String[] subparts = parts[0].Split(' ');
                    int rcount = Int32.Parse(subparts[3].Trim());
                    for (int ind = 4; rcount > 0; ind += 2, rcount--)
                    {
                        rList.Add(subparts[ind].Replace('_', ' '));
                    }
                }

            }

            related = rList.ToArray();
            return desc;
        }
    }

    public class NounCollection
    {
        private int indexOffset = 29;
        private static NounCollection _nouncollection = null;
        private Dictionary<String, Noun> nouns = new Dictionary<string, Noun>();

        public Dictionary<String, Noun> Nouns
        {
            get { return nouns; }
            set { nouns = value; }
        }

        private NounCollection() { Init(); }

        /// <summary>
        /// 
        /// </summary>
        public static NounCollection Instance
        {
            get
            {
                if (_nouncollection == null)
                    _nouncollection = new NounCollection();
                return _nouncollection;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("dictionary.wordnet.index.noun")))
            {
                int offset = 0;
                while (!reader.EndOfStream && offset++ < this.indexOffset)
                    reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    offset = 0;
                    String wLine = reader.ReadLine();
                    String[] wLineParts = wLine.Split(' ');
                    
                    Noun noun = new Noun();
                    noun.SynsetOffset = new List<Int64>();

                    noun.Lemma = wLineParts[offset++];
                    noun.Pos = wLineParts[offset++];
                    noun.SynseCnt = wLineParts[offset++];
                    noun.PCnt = wLineParts[offset];
                    offset = offset+ Int32.Parse(noun.PCnt);
                    noun.SenseCnt = wLineParts[++offset];
                    noun.TagsenseCnt = wLineParts[++offset];
                    for (int index = ++offset; index < wLineParts.Length; index++)
                    {
                        Int64 synsetOffset = 0;
                        if (Int64.TryParse(wLineParts[index], out synsetOffset))
                            noun.SynsetOffset.Add(synsetOffset);
                    }
                    this.nouns.Add(noun.Lemma, noun);
                }
            }
        }
    }
    public class WordNet
    {
        //private 
    }
}
