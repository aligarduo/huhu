using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class ArticleTagDAL : BaseDAL<article_tag>, IArticleTagDAL
    {
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<article_tag> Query_ID(article_tag n)
        {
            return GetEntities(x => x.tag_id == n.tag_id).ToList();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<article_tag> PagingQuery(int pageIndex, int pageSize)
        {
            return GetConditionPagingQuery(x => x.tag_id.Contains(""), k => k.tag_id.Contains(""), pageIndex, pageSize).ToList();
        }

        /// <summary>
        /// 总数据量
        /// </summary>
        /// <returns></returns>
        public int TotalVolume()
        {
            return Count(x => x.tag_id != null);
        }

        public List<article_tag> Query_Name(article_tag tag)
        {
            return GetEntities(x => x.tag_name == tag.tag_name).ToList();
        }

        public bool Update_Condition(article_tag tag, string[] condition)
        {
            return UpdateCondition(tag, condition);
        }
    }
}
