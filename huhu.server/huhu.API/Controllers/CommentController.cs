using huhu.API.Filters;
using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Snowflake;
using huhu.Commom.Token;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 评论
    /// </summary>
    public class CommentController : ApiController
    {
        public IUserService UserBLL { get; set; }
        public IDiggService DiggBLL { get; set; }
        public IArticleService ArticleBLL { get; set; }
        public IArticleCommentService ArticleCommentBLL { get; set; }
        public IArticleReplyService ArticleReplyBLL { get; set; }


        [HttpPost]
        [Route("interact_api/v1/comment/publish")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Publish([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] { "comment_content", "item_id" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            Snowflake.Instance.SnowflakesInit(0, 0);
            article_comment comment = new article_comment {
                comment_id = Snowflake.Instance.NextId().ToString(),
                user_id = token.get_and_parse(ActionContext).setParams,
                article_id = obj_list["item_id"].Value<string>(),
                comment_content = obj_list["comment_content"].Value<string>(),
                ctime = TimeUtil.GetCurrentTimestamp().ToString()
            };
            article_all article = new article_all { article_id = comment.article_id };
            if (ArticleBLL.Query_ID(article).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ARTICLE_NOT_EXIST_OR_DELETED, Descripion.GetDescription(ResultCode.ARTICLE_NOT_EXIST_OR_DELETED)));
            }

            ArticleCommentBLL.Add(comment);
            ArticleCommentBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), comment));
        }

        [HttpPost]
        [Route("interact_api/v1/comment/delete")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Delete([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null || obj_list.Property("comment_id") == null || obj_list["comment_id"].Value<string>() == "") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            article_comment comment = new article_comment {
                comment_id = obj_list["comment_id"].Value<string>(),
                user_id = token.get_and_parse(ActionContext).setParams
            };
            if (ArticleCommentBLL.Query_ID(comment).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.COMMENTS_NOT_EXIST, Descripion.GetDescription(ResultCode.COMMENTS_NOT_EXIST)));
            }
            ArticleCommentBLL.Delete(comment);
            ArticleCommentBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }

        [HttpPost]
        [Route("interact_api/v1/comment/list")]
        public HttpResponseMessage List([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "cursor", "limit", "item_id" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            //取值
            article_comment comment = new article_comment {
                article_id = obj_list["item_id"].Value<string>()
            };
            int cursor = obj_list["cursor"].Value<int>(), limit = obj_list["limit"].Value<int>();
            //分页查询
            List<article_comment> article_Comment = ArticleCommentBLL.ConditionPagingQuery(cursor, limit, comment);
            var dataVolume = Synthetic_Data(article_Comment, token.is_exist_token(ActionContext).setParams);
            int Total = ArticleCommentBLL.CriteriaTotalVolume(comment);
            bool More = true;
            if (article_Comment.Count <= limit) {
                More = false;
            }
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, (cursor + article_Comment.Count).ToString(), dataVolume, More));
        }



        private object Filter_EF_User(string user_id)
        {
            user_all user = new user_all();
            user.user_id = user_id;
            user_all user_vessel = UserBLL.Query_ID(user).FirstOrDefault();
            if (user_vessel == null) return "";
            if (user_vessel.avatar_large == null || user_vessel.avatar_large == "") {
                user_vessel.avatar_large = ConfigurationManager.AppSettings["MainFileServer"].ToString();
                user_vessel.avatar_large += ConfigurationManager.AppSettings["DefaultUserProfilePicture"].ToString();
            }
            else {
                user_vessel.avatar_large = ConfigurationManager.AppSettings["MainFileServer"].ToString() + user_vessel.avatar_large;
            }
            var fi = new string[] { "user_id", "user_name", "avatar_large", "level", "job_title" };
            return EntityUtil.GainObject<user_all>(user_vessel, fi);
        }

        private object Is_Digg(string reply_id, string User)
        {
            user_digg _Digg = new user_digg();
            _Digg.user_id = User;
            _Digg.digg_id = reply_id;
            _Digg.type = 3;
            Dictionary<string, object> _Interact = new Dictionary<string, object>();
            if (DiggBLL.User_Interact(_Digg).FirstOrDefault() == null)
                _Interact.Add("is_digg", false);
            else
                _Interact.Add("is_digg", true);
            _Interact.Add("user_id", long.Parse(User));
            return _Interact;
        }

        private List<object> Synthetic_Data(List<article_comment> article_comment, string User)
        {
            List<object> FirstFloor_List = new List<object>();
            article_reply _Reply = new article_reply();
            //遍历
            foreach (article_comment firstItem in article_comment) {
                Dictionary<string, object> FirstFloor_Dic = new Dictionary<string, object>();
                FirstFloor_Dic.Add("comment_id", firstItem.comment_id);
                //评论
                FirstFloor_Dic.Add("comment_info", ArticleCommentBLL.Query_ID(firstItem).FirstOrDefault());
                //回复
                List<object> Reply_List = new List<object>();
                _Reply.reply_comment_id = firstItem.comment_id;
                foreach (var R_item in ArticleReplyBLL.Query_Reply_Comment_ID(_Reply)) {
                    Dictionary<string, object> R_Dic = new Dictionary<string, object>();
                    R_Dic.Add("reply_id", R_item.reply_id);
                    //
                    R_Dic.Add("reply_info", ArticleReplyBLL.Query_Reply_ID(R_item).FirstOrDefault());
                    //
                    R_Dic.Add("user_info", Filter_EF_User(R_item.user_id));
                    //
                    R_Dic.Add("reply_user", Filter_EF_User(R_item.reply_to_user_id ?? "0"));
                    //
                    R_Dic.Add("user_interact", Is_Digg(R_item.reply_id, User));
                    Reply_List.Add(R_Dic);
                }
                FirstFloor_Dic.Add("reply_infos", Reply_List);
                //发表者
                FirstFloor_Dic.Add("user_info", Filter_EF_User(firstItem.user_id));
                //交互
                FirstFloor_Dic.Add("user_interact", Is_Digg(firstItem.comment_id, User));
                //集合数据
                FirstFloor_List.Add(FirstFloor_Dic);
            }
            return FirstFloor_List;
        }

    }
}