using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class ArticleTagService : BaseService<article_tag>, IArticleTagService
    {
        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<article_tag> Query_ID(article_tag n)
        {
            return db.ArticleTagDal.Query_ID(n);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<article_tag> PagingQuery(int pageIndex, int pageSize)
        {
            return db.ArticleTagDal.PagingQuery(pageIndex, pageSize);
        }

        /// <summary>
        /// 总数据量
        /// </summary>
        /// <returns></returns>
        public int TotalVolume()
        {
            return db.ArticleTagDal.TotalVolume();
        }

        public List<article_tag> Query_Name(article_tag tag)
        {
            return db.ArticleTagDal.Query_Name(tag);
        }

        public bool Update_Condition(article_tag tag, string[] condition)
        {
            return db.ArticleTagDal.Update_Condition(tag, condition);
        }
    }
}
