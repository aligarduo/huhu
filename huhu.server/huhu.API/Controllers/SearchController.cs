using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Token;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 搜索
    /// </summary>
    public class SearchController : ApiController
    {
        #region 接口
        public IArticleService ArticleBLL { get; set; }
        public IArticleTagService ArticleTagBLL { get; set; }
        public IUserService UserBLL { get; set; }
        public IDiggService DiggBLL { get; set; }
        public IArticleViewService ArticleViewBLL { get; set; }
        public IArticleCommentService ArticleCommentBLL { get; set; }
        public ITopicService TopicBLL { get; set; }

        private string MainFileServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();

        #endregion


        [HttpPost]
        [Route("search_api/v1/search")]
        public HttpResponseMessage Search([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] {
                "key_word",//搜索关键字
                "cursor",//索引
                "limit",//数据量
                "id_type",//搜索类型 文章or话题
                "sort_type"//筛选类型 综合排序or最新优先
            };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            return JsonUtil.ToJson(Search_Types(obj_list));
        }

        /// <summary>
        /// 搜索类型 文章or话题
        /// </summary>
        /// <param name="obj_list"></param>
        /// <returns></returns>
        private object Search_Types(JObject obj_list)
        {
            ResultMsg result = new ResultMsg();
            int id_type = obj_list["id_type"].Value<int>();
            switch (id_type) {
                case 1: {//搜索类型 文章
                        return Filter_Type_Article(obj_list);
                    };
                case 2: {//搜索类型 话题
                        return Filter_Type_Topic(obj_list);
                    };
                default: return result.SetResultMsg((int)ResultCode.OPERATION_TYPE_ERROR, Descripion.GetDescription(ResultCode.OPERATION_TYPE_ERROR));
            }
        }

        /// <summary>
        ///  搜索 文章
        /// </summary>
        /// <param name="obj_list"></param>
        /// <returns></returns>
        private object Filter_Type_Article(JObject obj_list)
        {
            ResultMsg result = new ResultMsg();
            int sort_type = obj_list["sort_type"].Value<int>(),
                cursor = obj_list["cursor"].Value<int>(),
                limit = obj_list["limit"].Value<int>();
            string key_word = obj_list["key_word"].Value<string>();
            List<article_all> dataVolume;
            switch (sort_type) {
                case 1: {//筛选类型 综合排序
                        dataVolume = ArticleBLL.Search_Composite_Ranking(cursor, limit, key_word);
                    }; break;
                case 2: {
                        //筛选类型 最新优先
                        dataVolume = ArticleBLL.Search_Newest_Ranking(cursor, limit, key_word);
                    }; break;
                default: return result.SetResultMsg((int)ResultCode.OPERATION_TYPE_ERROR, Descripion.GetDescription(ResultCode.OPERATION_TYPE_ERROR));
            }
            //总条数
            int Total = ArticleBLL.TotalVolume();
            //是否有下一页
            bool More = true;
            if (dataVolume.Count < limit) {
                More = false;
            }
            object data = Synthetic_Data(dataVolume);
            return result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, (cursor + dataVolume.Count).ToString(), data, More);
        }

        /// <summary>
        /// 搜索话题
        /// </summary>
        /// <param name="obj_list"></param>
        /// <returns></returns>
        private object Filter_Type_Topic(JObject obj_list)
        {
            ResultMsg result = new ResultMsg();
            int sort_type = obj_list["sort_type"].Value<int>(),
                cursor = obj_list["cursor"].Value<int>(),
                limit = obj_list["limit"].Value<int>();
            string key_word = obj_list["key_word"].Value<string>();
            List<topic_all> dataVolume;
            switch (sort_type) {
                case 1: {//筛选类型 综合排序
                        dataVolume = TopicBLL.Search_Composite_Ranking(cursor, limit, key_word);
                    }; break;
                case 2: {
                        //筛选类型 最新优先
                        dataVolume = TopicBLL.Search_Newest_Ranking(cursor, limit, key_word);
                    }; break;
                default: return result.SetResultMsg((int)ResultCode.OPERATION_TYPE_ERROR, Descripion.GetDescription(ResultCode.OPERATION_TYPE_ERROR));
            }
            //总条数
            int Total = TopicBLL.TotalVolume();
            //是否有下一页
            bool More = true;
            if (dataVolume.Count < limit) {
                More = false;
            }
            return result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, (cursor + dataVolume.Count).ToString(), dataVolume, More);
        }

        /// <summary>
        /// 整合数据
        /// </summary>
        /// <param name="article_list"></param>
        /// <returns></returns>
        private List<object> Synthetic_Data(List<article_all> article_list)
        {
            string User = token.is_exist_token(ActionContext).setParams;
            List<object> result = new List<object>();
            user_all user = new user_all();
            article_tag tag = new article_tag();

            foreach (var item in article_list) {
                Dictionary<string, object> feed = new Dictionary<string, object>();
                Dictionary<string, object> item_info = new Dictionary<string, object>();

                article_all article = ObjectUtil.ConvertObject<article_all>(item);
                item_info.Add("article_id", article.article_id);
                //文章
                article.mark_content = "";
                List<string> tag_id = new List<string>(article.tag_ids.Split(','));
                SortedDictionary<string, object> cc = EntityUtil.EntityByDic<article_all>(article);
                cc["tag_id"] = tag_id;
                var cover = article.cover_image == "" || article.cover_image == null ? "" : MainFileServer + article.cover_image;
                cc["cover_image"] = cover;
                //阅读量
                article_view view = new article_view();
                view.article_id = article.article_id;
                cc.Add("got_view_count", ArticleViewBLL.Reading_Quantity(view).view_count);
                //点赞量
                user_digg _Digg = new user_digg();
                _Digg.digg_id = article.article_id;
                _Digg.type = 1;
                cc.Add("got_digg_count", DiggBLL.Digg_Count(_Digg).Count);
                //评论量
                article_comment comment = new article_comment();
                comment.article_id = article.article_id;
                cc.Add("got_comment_count", ArticleCommentBLL.Query_Article_ID(comment).Count);

                item_info.Add("article_info", cc);

                //标签
                List<object> tag_vessel = new List<object>();
                for (int i = 0; i < tag_id.Count; i++) {
                    tag.tag_id = tag_id[Convert.ToInt32(i)];
                    List<article_tag> tag_list = ArticleTagBLL.Query_ID(tag);
                    for (int k = 0; k < tag_list.Count; k++) {
                        tag_list[k].icon = MainFileServer + tag_list[k].icon;
                        tag_vessel.Add(tag_list[k]);
                    }
                }
                item_info.Add("tags", tag_vessel);
                //作者
                user.user_id = article.user_id;
                user_all user_vessel = UserBLL.Query_ID(user).FirstOrDefault();
                if (user_vessel.avatar_large == null || user_vessel.avatar_large == "") {
                    user_vessel.avatar_large = MainFileServer;
                    user_vessel.avatar_large += ConfigurationManager.AppSettings["DefaultUserProfilePicture"].ToString();
                }
                var fi = new string[] { "user_id", "user_name", "avatar_large" };
                var aa = EntityUtil.GainObject<user_all>(user_vessel, fi);
                item_info.Add("author_user_info", aa);

                //用户交互
                Dictionary<string, object> User_Interact = new Dictionary<string, object>();

                //点赞
                _Digg.user_id = User;
                List<user_digg> digg = DiggBLL.User_Interact(_Digg);
                if (digg.Count == 0)
                    User_Interact.Add("is_digg", false);
                else
                    User_Interact.Add("is_digg", true);
                User_Interact.Add("user_id", long.Parse(User));
                item_info.Add("user_interact", User_Interact);

                feed.Add("item_info", item_info);
                feed.Add("item_type", 2);

                result.Add(feed);
            }

            return result;
        }

    }
}