using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.wikipedia.www
{
    public class ArticleInfo
    {
        public String Url { get; internal set; }
        public String Title { get; internal set; }
        public String Introduction { get; internal set; }
        public String ArticleHtml { get; internal set; }
        public String ArticleTextRaw { get; internal set; }
    }
}
