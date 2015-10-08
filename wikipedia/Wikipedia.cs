using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.wikipedia.www
{
    public class Wikipedia
    {
        private WikiQueryEngine queryEngine = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryEngine"></param>
        public Wikipedia(WikiQueryEngine queryEngine)
        {
            this.queryEngine = queryEngine;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public Boolean IsArticleAvailable(String topic)
        {
            SearchSuggestion searchResult = queryEngine.QueryServerSearch("opensearch", topic,1);
            if (searchResult != null && searchResult.Section != null && searchResult.Section.Length > 0)
            {
                return searchResult.Section[0].Text.Equals(topic);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public SearchSuggestion SearchArticles(String topic, int count)
        {
            SearchSuggestion searchResult = queryEngine.QueryServerSearch("opensearch", topic, count);
            return searchResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public ArticleInfo GetArticle(String topic, Boolean getHtml)
        {
            if (this.IsArticleAvailable(topic))
            {
                ArticleInfo articleInfo = new ArticleInfo();

                mediawiki mediaWiki = queryEngine.QueryServerArticle("query", topic);
                String[] articleParts = xsd.XSDConvert.Instance.Convert(mediaWiki);
                articleInfo.Title = articleParts[0];
                articleInfo.Url = articleParts[1];
                articleInfo.ArticleTextRaw = articleParts[2];
                articleInfo.Introduction = articleParts[3];
                if (getHtml)
                {
                    String htmlStr = queryEngine.QueryServerArticleHtml(articleInfo.Url);
                    articleInfo.ArticleHtml = xsd.XSDConvert.Instance.ConvertToHtml(htmlStr);
                }
                return articleInfo;
            }
            return null;
        }
    }
}
