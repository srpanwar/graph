using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Omuk.OmukEngine
{
    public class OmukDB
    {
        private static OmukDB _omukDB = null;
        private Dictionary<String, int> dictionary = new Dictionary<string, int>();

        private OmukDB()
        { }

        public static OmukDB Instance
        {
            get
            {
                if (_omukDB == null)
                    _omukDB = new OmukDB();
                return _omukDB;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int GetScore(String category, String bucket, String text)
        {
            String prekey = String.Format("{0}.{1}", category, bucket);
            if (!dictionary.ContainsKey(prekey))
                this.LoadFile(category, bucket);

            //text = String.Format("{0}.{1}", prekey, text);
            String key = Array.Find(this.dictionary.Keys.ToArray(), kys =>kys.StartsWith(prekey) && text.Contains(kys.Remove(0, prekey.Length).TrimStart('.')));
            if (!String.IsNullOrEmpty(key))
                return this.dictionary[key];
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public Boolean IsIgnored(String category, String bucket, String text)
        {
            String prekey = String.Format("{0}.{1}", category, bucket);
            if (!dictionary.ContainsKey(prekey))
                this.LoadFile2(category, bucket);

            //text = String.Format("{0}.{1}", prekey, text);
            String key = Array.Find(this.dictionary.Keys.ToArray(), kys => kys.StartsWith(prekey) && text.Equals(kys.Remove(0, prekey.Length).TrimStart('.'), StringComparison.InvariantCultureIgnoreCase));
            if (!String.IsNullOrEmpty(key) && !key.Equals(prekey))
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public String[] GetIgnored(String category, String bucket)
        {
            String prekey = String.Format("{0}.{1}", category, bucket);
            if (!dictionary.ContainsKey(prekey))
                this.LoadFile2(category, bucket);

            List<String> list = new List<string>();
            Array.ForEach(this.dictionary.Keys.ToArray(), delegate(String ky) { if (ky.StartsWith(prekey)) list.Add(ky.Remove(0, prekey.Length).TrimStart('.')); });
            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        private void LoadFile(String category, String bucket)
        {
            using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(String.Format("OmukEngine.db.{0}.{1}.txt", category, bucket))))
            {
                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine();
                    String[] parts = line.Split(' ');
                    if (parts.Length != 2) continue;
                    dictionary.Add(String.Format("{0}.{1}.{2}", category, bucket, parts[0]), Int32.Parse(parts[1]));
                }
            }
            dictionary.Add(String.Format("{0}.{1}", category, bucket), 0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        private void LoadFile2(String category, String bucket)
        {
            using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(String.Format("OmukEngine.db.{0}.{1}.txt", category, bucket))))
            {
                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine();
                    dictionary.Add(String.Format("{0}.{1}.{2}", category, bucket, line), 0);
                }
            }
            dictionary.Add(String.Format("{0}.{1}", category, bucket), 0);
        }
    }
}
