using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Snowflake;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 文章标签
    /// </summary>
    public class TagsController : ApiController
    {
        public IArticleTagService ArticleTagBLL { get; set; }


        [HttpPost]
        [Route("tag_api/v1/tag/create")]
        public HttpResponseMessage Create([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "tag_name", "icon", "color", "back_ground" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            string icon = obj_list["icon"].Value<string>();
            string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
            string domainName = URLUtil.CaptureURL(icon, "domainName");
            if (!URLUtil.CheckURL(icon)) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
            }
            if (domainNameServer != domainName) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.LINK_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.LINK_NOT_SUPPORTED)));
            }

            Snowflake.Instance.SnowflakesInit(0, 0);
            article_tag tag = new article_tag {
                tag_id = Snowflake.Instance.NextId().ToString(),
                tag_name = obj_list["tag_name"].Value<string>(),
                color = obj_list["color"].Value<string>(),
                back_ground = obj_list["back_ground"].Value<string>(),
                icon = URLUtil.Parsing_URL(icon),
                ctime = TimeUtil.GetCurrentTimestamp().ToString(),
                mtime = TimeUtil.GetCurrentTimestamp().ToString()
            };

            if (ArticleTagBLL.Query_Name(tag).FirstOrDefault() != null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.TAG_NAME_ALREADY_EXISTS, Descripion.GetDescription(ResultCode.TAG_NAME_ALREADY_EXISTS)));
            }
            ArticleTagBLL.Add(tag);
            ArticleTagBLL.SaveChanges();
            tag.icon = domainNameServer + tag.icon;
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), tag));
        }


        [HttpPost]
        [Route("tag_api/v1/tag/delete")]
        public HttpResponseMessage Delete([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] { "tag_id" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            article_tag tag = new article_tag {
                tag_id = obj_list["tag_id"].Value<string>()
            };
            if (ArticleTagBLL.Query_ID(tag).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.TAG_NOT_EXIST, Descripion.GetDescription(ResultCode.TAG_NOT_EXIST)));
            }
            ArticleTagBLL.Delete(tag);
            ArticleTagBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


        [HttpPost]
        [Route("tag_api/v1/tag/query_tag_list")]
        public HttpResponseMessage Query_tag([FromBody] JObject json)
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
            string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
            //取出参数
            int cursor = obj_list["cursor"].Value<int>(), limit = obj_list["limit"].Value<int>();
            List<article_tag> tag_list = ArticleTagBLL.PagingQuery(cursor, limit);
            if (tag_list.Count != 0) {
                foreach (var item in tag_list) {
                    item.icon = domainNameServer + item.icon;
                }
            }
            int Total = ArticleTagBLL.TotalVolume();
            //是否有下一页
            bool More = true;
            if (tag_list.Count < limit) {
                More = false;
            }
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, (cursor + tag_list.Count).ToString(), tag_list, More));
        }


        [HttpPost]
        [Route("tag_api/v1/tag/update")]
        public HttpResponseMessage Update([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "tag_id", "tag_name", "icon", "color", "back_ground" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            string icon = obj_list["icon"].Value<string>();
            string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
            string domainName = URLUtil.CaptureURL(icon, "domainName");
            if (!URLUtil.CheckURL(icon)) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
            }
            if (domainNameServer != domainName) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.LINK_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.LINK_NOT_SUPPORTED)));
            }

            Snowflake.Instance.SnowflakesInit(0, 0);
            article_tag tag = new article_tag {
                tag_id = obj_list["tag_id"].Value<string>(),
                tag_name = obj_list["tag_name"].Value<string>(),
                color = obj_list["color"].Value<string>(),
                back_ground = obj_list["back_ground"].Value<string>(),
                icon = URLUtil.Parsing_URL(icon),
                mtime = TimeUtil.GetCurrentTimestamp().ToString(),
                ctime = ""
            };

            if (ArticleTagBLL.Query_ID(tag).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.TAG_NOT_EXIST, Descripion.GetDescription(ResultCode.TAG_NOT_EXIST)));
            }
            ArticleTagBLL.Update_Condition(tag, ef_item);
            if (ArticleTagBLL.Query_Name(tag).Count > 1) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.TAG_NAME_ALREADY_EXISTS, Descripion.GetDescription(ResultCode.TAG_NAME_ALREADY_EXISTS)));
            }
            ArticleTagBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), tag));
        }


    }
}