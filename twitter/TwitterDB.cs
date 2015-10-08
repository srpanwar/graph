using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace com.twitter.www
{
    public class TwitterDBScore
    {
        public String PrimaryWord { get; set; }
        public int PrimaryWordStrength { get; set; }
        public Dictionary<String, int> StrenghWords { get; set; }

    }

    public class TwitterDB
    {
        private static TwitterDB _twitterDB = null;

        private TwitterDB()
        { }

        public static TwitterDB Instance
        {
            get
            {
                if (_twitterDB == null)
                    _twitterDB = new TwitterDB();
                return _twitterDB;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<TwitterDBScore> GetScore(String category, String bucket)
        {
            List<TwitterDBScore> twdbscoreList = new List<TwitterDBScore>();
            using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(String.Format("com.twitter.www.db.{0}.{1}.txt", category, bucket))))
            {
                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine();
                    String[] parts = line.Split(' ');
                    if (parts.Length < 2) continue;
                    TwitterDBScore tdbScore = new TwitterDBScore();
                    tdbScore.PrimaryWord = parts[0];
                    tdbScore.PrimaryWordStrength = Int32.Parse(parts[1]);
                    tdbScore.StrenghWords = new Dictionary<string,int>();
                    for (int index = 2; index < parts.Length; index++)
                    {
                        if (index % 2 == 0)
                            tdbScore.StrenghWords.Add(parts[index], Int32.Parse(parts[index + 1]));

                    }
                    twdbscoreList.Add(tdbScore);
                }
            }
            return twdbscoreList;
        }
    }
}
