using huhu.API.Filters;
using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.SensitiveCheck;
using huhu.Commom.Snowflake;
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
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ArticleController : ApiController
    {
        #region 接口
        public IUserService UserBLL { get; set; }
        public IDiggService DiggBLL { get; set; }
        public IArticleService ArticleBLL { get; set; }
        public IArticleTagService ArticleTagBLL { get; set; }
        public IArticleViewService ArticleViewBLL { get; set; }
        public IArticleDraftService ArticleDraftBLL { get; set; }
        public IUserCollectService UserCollectBLL { get; set; }
        public IFollowService FollowBLL { get; set; }
        public IArticleCommentService ArticleCommentBLL { get; set; }

        private string MainFileServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
        #endregion


        [HttpPost]
        [Route("content_api/v1/article/publish")]
        //[SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Publish([FromBody] JObject json) {
            //请求数据体
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "draft_id", "title", "brief_content", "mark_content", "cover_image", "tag_ids", "edit_type" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            //存在标签
            List<string> array = new List<string>();
            article_tag tag = new article_tag();
            foreach (var item in obj_list["tag_ids"].ToArray()) {
                tag.tag_id = item.ToObject<string>();
                if (ArticleTagBLL.Query_ID(tag).FirstOrDefault() == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.TAG_NOT_EXIST, Descripion.GetDescription(ResultCode.TAG_NOT_EXIST)));
                }
                array.Add(item.ToObject<string>());
            }
            //检查封面链接是否可用
            string icon = obj_list["cover_image"].Value<string>();
            if (!icon.Equals(string.Empty)) {
                string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
                string domainName = URLUtil.CaptureURL(icon, "domainName");
                if (!URLUtil.CheckURL(icon)) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
                }
                if (domainNameServer != domainName) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.LINK_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.LINK_NOT_SUPPORTED)));
                }
            }
            //初始化雪花算法
            Snowflake.Instance.SnowflakesInit(0, 0);
            //实体赋值
            article_all article = new article_all {
                article_id = Snowflake.Instance.NextId().ToString(),
                user_id = token.get_and_parse(ActionContext).setParams,
                cover_image = URLUtil.Parsing_URL(icon),
                title = obj_list["title"].Value<string>(),
                brief_content = obj_list["brief_content"].Value<string>(),
                mark_content = obj_list["mark_content"].Value<string>(),
                tag_ids = string.Join(",", array),
                edit_type = obj_list["edit_type"].Value<string>(),
                audit_status = 0,
                ctime = TimeUtil.GetCurrentTimestamp().ToString(),
                mtime = TimeUtil.GetCurrentTimestamp().ToString()
            };


            //检查内容是否包含敏感词
            object article_title = SensitiveMain.SensitiveCheck(article.title);
            object article_brief_content = SensitiveMain.SensitiveCheck(article.brief_content);
            object article_mark_content = SensitiveMain.SensitiveCheck(article.mark_content);

            if (article_title != null || article_brief_content != null || article_mark_content != null) {
                object obj = new {
                    title = article_title,
                    brief_content = article_brief_content,
                    mark_content = article_mark_content,
                };
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.CONTENT_CONTAINS_SENSITIVE_WORDS, Descripion.GetDescription(ResultCode.CONTENT_CONTAINS_SENSITIVE_WORDS), obj));
            }


            //是否存在草稿
            article_draft draft = new article_draft { draft_id = obj_list["draft_id"].Value<string>() };
            if (ArticleDraftBLL.Query_ID(draft).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.DRAFT_NOT_EXIST, Descripion.GetDescription(ResultCode.DRAFT_NOT_EXIST)));
            }
            //设置阅读量
            article_view view = new article_view {
                article_id = article.article_id,
                view_count = 0
            };
            //保存实体数据
            ArticleBLL.Add(article);
            ArticleViewBLL.Add(view);
            ArticleDraftBLL.Delete(draft);
            ArticleBLL.SaveChanges();
            ArticleViewBLL.SaveChanges();
            ArticleDraftBLL.SaveChanges();
            //返回给客户端
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Rebuild_Objects(article)));
        }


        [HttpPost]
        [Route("content_api/v1/article/delete")]
        //[SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Delete([FromBody] JObject json) {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null || obj_list.Property("article_id") == null || obj_list["article_id"].Value<string>() == "") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            article_all article = new article_all {
                article_id = obj_list["article_id"].Value<string>(),
                user_id = token.get_and_parse(ActionContext).setParams
            };
            article_all article_detail = ArticleBLL.Query_ID(article).FirstOrDefault();
            if (article_detail == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ARTICLE_NOT_EXIST_OR_DELETED, Descripion.GetDescription(ResultCode.ARTICLE_NOT_EXIST_OR_DELETED)));
            }
            article_view view = new article_view();
            view.article_id = article.article_id;

            ArticleBLL.Delete(article);
            ArticleViewBLL.Delete(view);
            ArticleBLL.SaveChanges();
            ArticleViewBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


        [HttpPost]
        [Route("content_api/v1/article/detail")]
        public HttpResponseMessage Detail([FromBody] JObject json) {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null || obj_list.Property("article_id") == null || obj_list["article_id"].Value<string>() == "") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            article_all article = new article_all {
                article_id = obj_list["article_id"].Value<string>()
            };
            article_all article_detail = ArticleBLL.Query_ID(article).FirstOrDefault();
            if (article_detail == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ARTICLE_NOT_EXIST_OR_DELETED, Descripion.GetDescription(ResultCode.ARTICLE_NOT_EXIST_OR_DELETED)));
            }

            article_view view = new article_view {
                article_id = article.article_id
            };
            view.view_count = ArticleViewBLL.Reading_Quantity(view).view_count + 1;
            ArticleViewBLL.Update(view);
            ArticleViewBLL.SaveChanges();
            var data = Synthetic_Data(article_detail, view.view_count, token.is_exist_token(ActionContext).setParams);
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), data));
        }



        [HttpPost]
        [Route("content_api/v1/article/update")]
        //[SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Update([FromBody] JObject json) {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "article_id", "title", "brief_content", "mark_content", "cover_image", "tag_ids", "edit_type" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            List<string> array = new List<string>();
            article_tag tag = new article_tag();
            foreach (var item in obj_list["tag_ids"].ToArray()) {
                //存在标签
                tag.tag_id = item.ToObject<string>();
                if (ArticleTagBLL.Query_ID(tag).FirstOrDefault() == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.TAG_NOT_EXIST, Descripion.GetDescription(ResultCode.TAG_NOT_EXIST)));
                }
                array.Add(item.ToObject<string>());
            }

            string icon = obj_list["cover_image"].Value<string>();
            if (icon != "") {
                string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
                string domainName = URLUtil.CaptureURL(icon, "domainName");
                if (!URLUtil.CheckURL(icon)) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
                }
                if (domainNameServer != domainName) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.LINK_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.LINK_NOT_SUPPORTED)));
                }
            }

            article_all article = new article_all {
                article_id = obj_list["article_id"].Value<string>(),
                user_id = token.get_and_parse(ActionContext).setParams,
                cover_image = URLUtil.Parsing_URL(icon),
                title = obj_list["title"].Value<string>(),
                brief_content = obj_list["brief_content"].Value<string>(),
                mark_content = obj_list["mark_content"].Value<string>(),
                tag_ids = string.Join(",", array),
                edit_type = obj_list["edit_type"].Value<string>(),
                audit_status = 0,
                ctime = TimeUtil.GetCurrentTimestamp().ToString(),
                mtime = TimeUtil.GetCurrentTimestamp().ToString()
            };
            article_all article_detail = ArticleBLL.Query_ID(article).FirstOrDefault();
            if (article_detail == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ARTICLE_NOT_EXIST_OR_DELETED, Descripion.GetDescription(ResultCode.ARTICLE_NOT_EXIST_OR_DELETED)));
            }

            ArticleBLL.Update_Condition(article, ef_item);
            ArticleBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Rebuild_Objects(article)));
        }



        [HttpPost]
        [Route("content_api/v1/article/editor")]
        //[SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage EditorCheck([FromBody] JObject json) {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null || obj_list.Property("article_id") == null || obj_list["article_id"].Value<string>() == "") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            article_all article = new article_all {
                article_id = obj_list["article_id"].Value<string>()
            };

            user_all user = new user_all();
            user.user_id = token.get_and_parse(ActionContext).setParams;
            article_all article_detail = ArticleBLL.Query_UserID_ArticleID(user, article).FirstOrDefault();
            if (article_detail == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.CONTENT_IS_NULL, Descripion.GetDescription(ResultCode.CONTENT_IS_NULL)));
            }
            article_detail.cover_image = article_detail.cover_image != "" &&
                article_detail.cover_image != null ? MainFileServer + article_detail.cover_image : "";

            List<string> tag_id = new List<string>(article_detail.tag_ids.Split(','));
            SortedDictionary<string, object> cc = EntityUtil.EntityByDic<article_all>(article_detail);
            List<object> tag_ids = new List<object>();
            foreach (var item in tag_id) {
                if (item != "") {
                    tag_ids.Add(item);
                }
            }
            cc["tag_ids"] = tag_ids;

            Dictionary<string, object> item_info = new Dictionary<string, object>();
            item_info.Add("article_info", cc);

            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), item_info));
        }





        /// <summary>
        /// 重构对象
        /// </summary>
        /// <param name="draft"></param>
        /// <returns></returns>
        private object Rebuild_Objects(article_all article) {
            List<string> tag_ids = new List<string>();
            if (article.tag_ids != "") {
                tag_ids = new List<string>(article.tag_ids.Split(','));
            }
            return new {
                article.article_id,
                article.user_id,
                article.cover_image,
                article.title,
                article.brief_content,
                article.mark_content,
                tag_ids,
                article.edit_type,
                article.audit_status,
                article.ctime,
                article.mtime
            };
        }

        /// <summary>
        /// 整合数据
        /// </summary>
        /// <param name="article">文章列表</param>
        /// <returns></returns>
        private Dictionary<string, object> Synthetic_Data(article_all article, int view, string User) {
            user_all user = new user_all();
            article_tag tag = new article_tag();

            Dictionary<string, object> item_info = new Dictionary<string, object>();
            item_info.Add("article_id", article.article_id);
            List<string> tag_id = new List<string>(article.tag_ids.Split(','));

            SortedDictionary<string, object> cc = EntityUtil.EntityByDic<article_all>(article);
            List<object> tag_ids = new List<object>();
            foreach (var item in tag_id) {
                if (item != "") {
                    tag_ids.Add(item);
                }
            }
            cc["tag_ids"] = tag_ids;
            var cover = article.cover_image == "" || article.cover_image == null ? "" : MainFileServer + article.cover_image;
            cc["cover_image"] = cover;
            //阅读量
            cc.Add("view_count", view);
            //点赞量
            user_digg digg = new user_digg {
                digg_id = article.article_id,
                type = 1
            };
            cc.Add("digg_count", DiggBLL.Digg_Count(digg).Count());
            //评论量
            article_comment comment = new article_comment {
                article_id = article.article_id
            };
            cc.Add("comment_count", ArticleCommentBLL.Query_Article_ID(comment).Count);

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
            //作者信息
            user.user_id = article.user_id;
            user_all user_vessel = UserBLL.Query_ID(user).FirstOrDefault();

            if (user_vessel.avatar_large == null && user.qq_figureurl != null) {
                user_vessel.avatar_large = user_vessel.qq_figureurl;
            } else {
                user_vessel.avatar_large = MainFileServer + user_vessel.avatar_large;
            }
            var fi = new string[] { "user_id", "user_name", "avatar_large", "level", "job_title" };
            var aa = EntityUtil.GainObject<user_all>(user_vessel, fi);
            item_info.Add("author_user_info", aa);

            //用户交互
            Dictionary<string, object> User_Interact = new Dictionary<string, object>();
            //点赞
            digg.user_id = User;
            if (DiggBLL.User_Interact(digg).FirstOrDefault() == null)
                User_Interact.Add("is_digg", false);
            else
                User_Interact.Add("is_digg", true);
            User_Interact.Add("user_id", long.Parse(User));
            item_info.Add("user_interact", User_Interact);
            //收藏
            user_collect collect = new user_collect();
            collect.user_id = User;
            collect.item_id = article.article_id;
            if (UserCollectBLL.Is_Add_Item(collect).FirstOrDefault() == null)
                User_Interact.Add("is_collect", false);
            else
                User_Interact.Add("is_collect", true);
            //关注
            user_follow follow = new user_follow();
            follow.user_id = User;
            follow.follow_id = article.user_id;
            follow.follow_type = 1;
            if (FollowBLL.Is_Follow(follow).FirstOrDefault() == null)
                User_Interact.Add("is_follow", false);
            else
                User_Interact.Add("is_follow", true);

            //返回整体
            return item_info;
        }

    }
}