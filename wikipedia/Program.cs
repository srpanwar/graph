using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace org.wikipedia.www
{
    class Program
    {
        static void Main(string[] args)
        {
            Wikipedia wClass = new Wikipedia(new WikiQueryEngine());
            Console.WriteLine(wClass.IsArticleAvailable("DDLJ"));
            SearchSuggestion searchSuggest = wClass.SearchArticles("DDLJ", 10);
            foreach (SearchSuggestionItem sItem in searchSuggest.Section)
            {
                Console.WriteLine(sItem.Text + "\n\t" + sItem.Description + "\n" + sItem.Url);
            }
            Console.WriteLine(wClass.GetArticle("Seattle",false).Introduction);
            return;
        }
    }
}
