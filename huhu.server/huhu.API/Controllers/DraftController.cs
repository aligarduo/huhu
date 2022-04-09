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
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 文章草稿
    /// </summary>
    public class DraftController : ApiController
    {
        public IArticleDraftService ArticleDraftBLL { get; set; }
        public IArticleTagService ArticleTagBLL { get; set; }


        [HttpPost]
        [Route("content_api/v1/article_draft/create")]
        //[SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Create([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] { "title", "brief_content", "mark_content", "cover_image", "tag_ids", "edit_type" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            Snowflake.Instance.SnowflakesInit(0, 0);
            article_draft draft = new article_draft {
                draft_id = Snowflake.Instance.NextId().ToString(),
                user_id = token.get_and_parse(ActionContext).setParams,
                title = obj_list["title"].Value<string>(),
                brief_content = obj_list["brief_content"].Value<string>(),
                mark_content = obj_list["mark_content"].Value<string>(),
                cover_image = obj_list["cover_image"].Value<string>(),
                tag_ids = obj_list["tag_ids"].ToArray()[0].Value<string>(),
                edit_type = obj_list["edit_type"].Value<string>(),
                status = 0,
                ctime = TimeUtil.GetCurrentTimestamp().ToString(),
                mtime = TimeUtil.GetCurrentTimestamp().ToString()
            };
            ArticleDraftBLL.Add(draft);
            ArticleDraftBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Rebuild_Objects(draft)));
        }


        [HttpPost]
        [Route("content_api/v1/article_draft/delete")]
        //[SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Delete([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            if (obj_list.Property("draft_id") == null || obj_list["draft_id"].Value<string>() == "") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            article_draft draft = new article_draft {
                draft_id = obj_list["draft_id"].Value<string>()
            };
            if (ArticleDraftBLL.Query_ID(draft).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.DRAFT_NOT_EXIST, Descripion.GetDescription(ResultCode.DRAFT_NOT_EXIST)));
            }
            ArticleDraftBLL.Delete(draft);
            ArticleDraftBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


        [HttpPost]
        [Route("content_api/v1/article_draft/detail")]
        [TokenSecurityFilter]
        public HttpResponseMessage Detail([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            if (obj_list.Property("draft_id") == null || obj_list["draft_id"].Value<string>() == "") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            article_draft draft = new article_draft {
                draft_id = obj_list["draft_id"].Value<string>()
            };
            article_draft draft_list = ArticleDraftBLL.Query_ID(draft).FirstOrDefault();
            return draft_list == null ?
                JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.DRAFT_NOT_EXIST, Descripion.GetDescription(ResultCode.DRAFT_NOT_EXIST))) :
                JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Rebuild_Objects(draft_list)));
        }


        [HttpPost]
        [Route("content_api/v1/article_draft/query_list")]
        [TokenSecurityFilter]
        public HttpResponseMessage Query_List([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] { "cursor", "limit" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            int cursor = obj_list["cursor"].Value<int>(), limit = obj_list["limit"].Value<int>();
            article_draft draft = new article_draft { user_id = token.get_and_parse(ActionContext).setParams };
            List<article_draft> draft_list = ArticleDraftBLL.Condition_PagingQuery_DESC(cursor, limit, draft);
            int Total = ArticleDraftBLL.CriteriaTotalVolume(draft);
            bool More = true;
            if (draft_list.Count < limit) {
                More = false;
            }
            List<object> list = new List<object>(); ;
            foreach (var item in draft_list) {
                list.Add(Rebuild_Objects(item));
            }
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, (cursor + draft_list.Count).ToString(), list, More));
        }


        [HttpPost]
        [Route("content_api/v1/article_draft/update")]
        //[SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Update([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] { "draft_id", "title", "brief_content", "mark_content", "cover_image", "tag_ids", "edit_type" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            article_draft draft = new article_draft {
                draft_id = obj_list["draft_id"].Value<string>(),
                user_id = token.get_and_parse(ActionContext).setParams,
                title = obj_list["title"].Value<string>(),
                brief_content = obj_list["brief_content"].Value<string>(),
                mark_content = obj_list["mark_content"].Value<string>(),
                cover_image = obj_list["cover_image"].Value<string>(),
                tag_ids = obj_list["tag_ids"].ToArray()[0].Value<string>(),
                edit_type = obj_list["edit_type"].Value<string>(),
                status = 0,
                mtime = TimeUtil.GetCurrentTimestamp().ToString(),
                ctime = ""
            };
            if (ArticleDraftBLL.Query_ID(draft).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.DRAFT_NOT_EXIST, Descripion.GetDescription(ResultCode.DRAFT_NOT_EXIST)));
            }
            ArticleDraftBLL.Update_Condition(draft, ef_item);
            ArticleDraftBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Rebuild_Objects(draft)));
        }



        /// <summary>
        /// 重构对象
        /// </summary>
        /// <param name="draft"></param>
        /// <returns></returns>
        private object Rebuild_Objects(article_draft draft)
        {
            List<string> tag_ids = new List<string>();
            if (draft.tag_ids != "") {
                tag_ids = new List<string>(draft.tag_ids.Split(','));
            }
            return new {
                draft.draft_id,
                draft.user_id,
                draft.title,
                draft.brief_content,
                draft.mark_content,
                draft.cover_image,
                tag_ids,
                draft.edit_type,
                draft.status,
                draft.ctime,
                draft.mtime
            };
        }

    }
}