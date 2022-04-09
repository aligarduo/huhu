using huhu.API.Filters;
using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Snowflake;
using huhu.Commom.Token;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 回复评论
    /// </summary>
    public class ReplyController : ApiController
    {
        public IUserService UserBLL { get; set; }
        public IArticleService ArticleBLL { get; set; }
        public IArticleReplyService ArticleReplyBLL { get; set; }
        public IArticleCommentService ArticleCommentBLL { get; set; }


        [HttpPost]
        [Route("interact_api/v1/reply/publish")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Publish([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            // 第一步：判断变量
            string[] con_item = new string[] { "item_id", "reply_content", "reply_to_comment_id", "reply_to_reply_id", "reply_to_user_id" };
            foreach (var item in con_item) {
                if (obj_list.Property(item) == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            // 第二步：判空
            // (1)
            string[] con = new string[] { "item_id", "reply_content", "reply_to_comment_id" };
            foreach (var item in con) {
                if (obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            // (2)
            string type;
            string[] per_item = new string[] { "reply_to_reply_id", "reply_to_user_id" };
            if (obj_list[per_item[0]].Value<string>() == null && obj_list[per_item[1]].Value<string>() == null) {
                type = "reply_to_comment";
            }
            else {
                type = "reply_to_reply";
            }
            // 第三步：获取变量值
            Snowflake.Instance.SnowflakesInit(0, 0);
            article_reply reply = new article_reply {
                reply_id = Snowflake.Instance.NextId().ToString(),
                article_id = obj_list["item_id"].Value<string>(),
                user_id = token.get_and_parse(ActionContext).setParams,
                reply_comment_id = obj_list["reply_to_comment_id"].Value<string>(),
                reply_content = obj_list["reply_content"].Value<string>(),
                reply_to_reply_id = obj_list["reply_to_reply_id"].Value<string>() ?? "",
                reply_to_user_id = obj_list["reply_to_user_id"].Value<string>() ?? "",
                reply_type = type == "reply_to_comment" ? 1 : 2,
                reply_status = 0,
                ctime = TimeUtil.GetCurrentTimestamp().ToString()
            };
            // 第四步：校验被评论项目是否存在
            article_all article = new article_all() { article_id = reply.article_id };
            if (ArticleBLL.Query_ID(article).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ARTICLE_NOT_EXIST_OR_DELETED, Descripion.GetDescription(ResultCode.ARTICLE_NOT_EXIST_OR_DELETED)));
            }
            // 第五步：校验被评论是否存在
            article_comment comment = new article_comment() { comment_id = reply.reply_comment_id };
            if (ArticleCommentBLL.Query_ID(comment).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.COMMENTS_NOT_EXIST, Descripion.GetDescription(ResultCode.COMMENTS_NOT_EXIST)));
            }
            // 第六步：是否为回复的回复
            if (type == "reply_to_reply") {
                if (ArticleReplyBLL.Query_Reply_To_Reply_ID(reply).FirstOrDefault() == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.REPLY_NOT_EXIST, Descripion.GetDescription(ResultCode.REPLY_NOT_EXIST)));
                }
                user_all user = new user_all() { user_id = reply.reply_to_user_id };
                if (UserBLL.Query_ID(user).FirstOrDefault() == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.USER_NOT_EXIST, Descripion.GetDescription(ResultCode.USER_NOT_EXIST)));
                }
            }
            // 第七步：保存数据
            ArticleReplyBLL.Add(reply);
            ArticleReplyBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), reply));
        }


        [HttpPost]
        [Route("interact_api/v1/reply/delete")]
        [TokenSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Delete([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] con_item = new string[] { "comment_id", "reply_id" };
            foreach (var item in con_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            article_reply reply = new article_reply {
                user_id = token.get_and_parse(ActionContext).setParams,
                reply_comment_id = obj_list["comment_id"].Value<string>(),
                reply_id = obj_list["reply_id"].Value<string>()
            };

            article_comment comment = new article_comment { comment_id = reply.reply_comment_id };
            if (ArticleCommentBLL.Query_ID(comment).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.COMMENTS_NOT_EXIST, Descripion.GetDescription(ResultCode.COMMENTS_NOT_EXIST)));
            }
            if (ArticleReplyBLL.Query_Reply_ID(reply).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.REPLY_NOT_EXIST, Descripion.GetDescription(ResultCode.REPLY_NOT_EXIST)));
            }
            ArticleReplyBLL.Delete(reply);
            ArticleReplyBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }

    }
}