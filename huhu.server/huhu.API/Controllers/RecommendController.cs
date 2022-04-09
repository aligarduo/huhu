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
    /// 推荐文章
    /// </summary>
    public class RecommendController : ApiController
    {
        #region 接口
        public IArticleService ArticleBLL { get; set; }
        public IArticleTagService ArticleTagBLL { get; set; }
        public IUserService UserBLL { get; set; }
        public IDiggService DiggBLL { get; set; }
        public IArticleViewService ArticleViewBLL { get; set; }
        public IArticleCommentService ArticleCommentBLL { get; set; }

        private string MainFileServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();

        #endregion

        [HttpPost]
        [Route("recommend_api/v1/article/recommend_all_feed")]
        public HttpResponseMessage Recommend_All_Feed([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "cursor", "limit" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            //取出参数
            int cursor = obj_list["cursor"].Value<int>(), limit = obj_list["limit"].Value<int>();
            //分页查询
            List<article_all> article_all_list = ArticleBLL.GetRandomSortPagingQuery(limit);
            //总条数
            int Total = ArticleBLL.TotalVolume();
            //整合数据
            object dataVolume = Synthetic_Data(article_all_list, token.is_exist_token(ActionContext).setParams);
            //是否有下一页
            bool More = true;
            if (article_all_list.Count < limit) {
                More = false;
            }
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, (cursor + article_all_list.Count).ToString(), dataVolume, More));
        }

        //[HttpPost]
        //[Route("recommend_api/v1/article/recommend_cate_feed")]
        //public HttpResponseMessage Recommend_Cate_Feed([FromBody] JObject json)
        //{
        //    ResultMsg result = new ResultMsg();
        //    JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
        //    if (obj_list == null) {
        //        return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
        //    }
        //    //校验参数及判空
        //    string[] ef_item = new string[] { "cursor", "limit", "cate_id" };
        //    foreach (var item in ef_item) {
        //        if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
        //            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
        //        }
        //    }
        //    //取出参数
        //    int cursor = obj_list["cursor"].Value<int>(), limit = obj_list["limit"].Value<int>();
        //    string cate_id = obj_list["cate_id"].Value<string>();

        //    article_category category = new article_category();
        //    category.category_id = cate_id;
        //    //按条件分页查询
        //    List<article_all> article_all_list = ArticleBLL.ConditionPagingQuery(cursor, limit, category);
        //    //总条数
        //    int Total = ArticleBLL.CriteriaTotalVolume(category);
        //    //整合数据
        //    object dataVolume = Synthetic_Data(article_all_list, token.is_exist_token(ActionContext).setParams);
        //    //是否有下一页
        //    bool More = true;
        //    if (article_all_list.Count <= limit) {
        //        More = false;
        //    }
        //    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, (cursor + article_all_list.Count).ToString(), dataVolume, More));
        //}

        //[HttpPost]
        //[Route("recommend_api/v1/article/recommend_cate_tag_feed")]
        //public HttpResponseMessage Recommend_Cate_Tag_Feed([FromBody] JObject json)
        //{
        //    ResultMsg result = new ResultMsg();
        //    JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
        //    if (obj_list == null) {
        //        return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
        //    }
        //    //校验参数及判空
        //    string[] ef_item = new string[] { "cursor", "limit", "cate_id", "tag_id" };
        //    foreach (var item in ef_item) {
        //        if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
        //            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
        //        }
        //    }
        //    //取出参数
        //    int cursor = obj_list["cursor"].Value<int>(), limit = obj_list["limit"].Value<int>();
        //    string cate_id = obj_list["cate_id"].Value<string>(), tag_id = obj_list["tag_id"].Value<string>();

        //    article_category category = new article_category();
        //    article_tag tag = new article_tag();
        //    category.category_id = cate_id;
        //    tag.tag_id = tag_id;
        //    //按条件分页查询
        //    List<article_all> article_all_list = ArticleBLL.ConditionPagingQuery(cursor, limit, category, tag);
        //    //总条数
        //    int Total = ArticleBLL.CriteriaTotalVolume(category, tag);
        //    //整合数据
        //    object dataVolume = Synthetic_Data(article_all_list, token.is_exist_token(ActionContext).setParams);
        //    //是否有下一页
        //    bool More = true;
        //    if (article_all_list.Count <= limit) {
        //        More = false;
        //    }
        //    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, (cursor + article_all_list.Count).ToString(), dataVolume, More));
        //}


        [HttpPost]
        [Route("content_api/v1/article/query_list")]
        public HttpResponseMessage Query_List([FromBody] JObject json) {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "cursor", "limit", "user_id" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            //取出参数
            int cursor = obj_list["cursor"].Value<int>(), limit = obj_list["limit"].Value<int>();
            string user_id = obj_list["user_id"].Value<string>();
            user_all user = new user_all();{ user.user_id = user_id; }
            //分页查询
            List<article_all> article_all_list = ArticleBLL.PagingQuery_UserID(user, cursor, limit);
            //总条数
            int Total = ArticleBLL.TotalVolume(user);
            //整合数据
            object dataVolume = Synthetic_Data(article_all_list, user_id);
            //是否有下一页
            bool More = true;
            if (article_all_list.Count < limit) {
                More = false;
            }
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, (cursor + article_all_list.Count).ToString(), dataVolume, More));
        }



        /// <summary>
        /// 整合数据
        /// </summary>
        /// <param name="article_list"></param>
        /// <returns></returns>
        private List<object> Synthetic_Data(List<article_all> article_list, string User)
        {
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
                List<object> tag_ids = new List<object>();
                foreach (var e in tag_id) {
                    if (e != "") {
                        tag_ids.Add(e);
                    }
                }
                cc["tag_ids"] = tag_ids;
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

                var fi = new string[] { "user_id", "user_name", "avatar_large", "level", "job_title" };
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