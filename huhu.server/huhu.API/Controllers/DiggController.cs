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
    /// 点赞
    /// </summary>
    public class DiggController : ApiController
    {
        public IDiggService DiggBLL { get; set; }
        public IArticleService ArticleBLL { get; set; }
        public IArticleReplyService ArticleReplyBLL { get; set; }
        public IArticleCommentService ArticleCommentBLL { get; set; }


        [HttpPost]
        [Route("interact_api/v1/digg/save")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Save([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "item_id", "item_type" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            // 第三步：获取变量值
            Snowflake.Instance.SnowflakesInit(0, 0);
            user_digg digg = new user_digg {
                id = Snowflake.Instance.NextId().ToString(),
                user_id = token.get_and_parse(ActionContext).setParams,
                digg_id = obj_list["item_id"].Value<string>(),
                type = obj_list["item_type"].Value<int>(),
                ctime = TimeUtil.GetCurrentTimestamp().ToString()
            };

            switch (digg.type) {
                case 1: {
                        article_all article = new article_all {
                            article_id = digg.digg_id
                        };
                        if (ArticleBLL.Query_ID(article).FirstOrDefault() == null) {
                            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ARTICLE_NOT_EXIST_OR_DELETED, Descripion.GetDescription(ResultCode.ARTICLE_NOT_EXIST_OR_DELETED)));
                        }
                    }; break;
                case 2: {
                        article_comment comment = new article_comment {
                            comment_id = digg.digg_id
                        };
                        if (ArticleCommentBLL.Query_ID(comment).FirstOrDefault() == null) {
                            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.COMMENTS_NOT_EXIST, Descripion.GetDescription(ResultCode.COMMENTS_NOT_EXIST)));
                        }
                    }; break;
                case 3: {
                        article_reply reply = new article_reply {
                            reply_id = digg.digg_id
                        };
                        if (ArticleReplyBLL.Query_Reply_ID(reply).FirstOrDefault() == null) {
                            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.REPLY_NOT_EXIST, Descripion.GetDescription(ResultCode.REPLY_NOT_EXIST)));
                        }
                    }; break;
                default: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPERATION_TYPE_ERROR, Descripion.GetDescription(ResultCode.OPERATION_TYPE_ERROR)));
            }

            if (DiggBLL.User_Interact(digg).FirstOrDefault() != null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.REPETITIVE_OPERATION, Descripion.GetDescription(ResultCode.REPETITIVE_OPERATION)));
            }
            DiggBLL.Add(digg);
            DiggBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


        [HttpPost]
        [Route("interact_api/v1/digg/cancel")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Cancel([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "item_id", "item_type" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            user_digg digg = new user_digg {
                user_id = token.get_and_parse(ActionContext).setParams,
                digg_id = obj_list["item_id"].Value<string>(),
                type = obj_list["item_type"].Value<int>(),
            };

            switch (digg.type) {
                case 1: {
                        article_all article = new article_all {
                            article_id = digg.digg_id
                        };
                        if (ArticleBLL.Query_ID(article).FirstOrDefault() == null) {
                            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ARTICLE_NOT_EXIST_OR_DELETED, Descripion.GetDescription(ResultCode.ARTICLE_NOT_EXIST_OR_DELETED)));
                        }
                    }; break;
                case 2: {
                        article_comment comment = new article_comment {
                            comment_id = digg.digg_id
                        };
                        if (ArticleCommentBLL.Query_ID(comment).FirstOrDefault() == null) {
                            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.COMMENTS_NOT_EXIST, Descripion.GetDescription(ResultCode.COMMENTS_NOT_EXIST)));
                        }
                    }; break;
                case 3: {
                        article_reply reply = new article_reply {
                            reply_id = digg.digg_id
                        };
                        if (ArticleReplyBLL.Query_Reply_ID(reply).FirstOrDefault() == null) {
                            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.REPLY_NOT_EXIST, Descripion.GetDescription(ResultCode.REPLY_NOT_EXIST)));
                        }
                    }; break;
                default: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPERATION_TYPE_ERROR, Descripion.GetDescription(ResultCode.OPERATION_TYPE_ERROR)));
            }

            user_digg d = DiggBLL.User_Interact(digg).FirstOrDefault();
            if (d == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPERATION_FAILURE, Descripion.GetDescription(ResultCode.OPERATION_FAILURE)));
            }
            digg.id = d.id;
            DiggBLL.Delete(digg);
            DiggBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


    }
}