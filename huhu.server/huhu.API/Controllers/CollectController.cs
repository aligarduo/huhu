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
    /// 收藏
    /// </summary>
    public class CollectController : ApiController
    {
        public IArticleService ArticleBLL { get; set; }
        public IUserCollectService UserCollectBLL { get; set; }
        public IUserCollectionService UserCollectionBLL { get; set; }


        [HttpPost]
        [Route("interact_api/v1/collect/add")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Add([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "collection_id", "item_id" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            Snowflake.Instance.SnowflakesInit(0, 0);
            user_collect collect = new user_collect {
                id = Snowflake.Instance.NextId().ToString(),
                user_id = token.get_and_parse(ActionContext).setParams,
                collection_id = obj_list["collection_id"].Value<string>(),
                item_id = obj_list["item_id"].Value<string>(),
                ctime = TimeUtil.GetCurrentTimestamp().ToString()
            };

            user_collection collection = new user_collection {
                user_id = collect.user_id,
                collection_id = collect.collection_id
            };
            if (UserCollectionBLL.Is_Collection_ID(collection).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.COLLECTION_NOT_EXIST, Descripion.GetDescription(ResultCode.COLLECTION_NOT_EXIST)));
            }

            article_all article = new article_all { article_id = collect.item_id };
            if (ArticleBLL.Query_ID(article).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ARTICLE_NOT_EXIST_OR_DELETED, Descripion.GetDescription(ResultCode.ARTICLE_NOT_EXIST_OR_DELETED)));
            }

            if (UserCollectBLL.Is_Collect_Item(collect).FirstOrDefault() != null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.REPETITIVE_OPERATION, Descripion.GetDescription(ResultCode.REPETITIVE_OPERATION)));
            }

            UserCollectBLL.Add(collect);
            UserCollectBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


        [HttpPost]
        [Route("interact_api/v1/collect/delete")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Delete([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "collection_id", "item_id" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            user_collect collect = new user_collect {
                user_id = token.get_and_parse(ActionContext).setParams,
                collection_id = obj_list["collection_id"].Value<string>(),
                item_id = obj_list["item_id"].Value<string>()
            };

            user_collection collection = new user_collection {
                user_id = collect.user_id,
                collection_id = collect.collection_id
            };
            if (UserCollectionBLL.Is_Collection_ID(collection).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.COLLECTION_NOT_EXIST, Descripion.GetDescription(ResultCode.COLLECTION_NOT_EXIST)));
            }

            article_all article = new article_all { article_id = collect.item_id };
            if (ArticleBLL.Query_ID(article).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ARTICLE_NOT_EXIST_OR_DELETED, Descripion.GetDescription(ResultCode.ARTICLE_NOT_EXIST_OR_DELETED)));
            }

            user_collect Coll = UserCollectBLL.Is_Collect_Item(collect).FirstOrDefault();
            if (Coll == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.COLLECTION_NOT_EXIST_ITEM, Descripion.GetDescription(ResultCode.COLLECTION_NOT_EXIST_ITEM)));
            }
            collect.id = Coll.id;
            UserCollectBLL.Delete(collect);
            UserCollectBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


        [HttpPost]
        [Route("interact_api/v1/collect/list")]
        [TokenSecurityFilter]
        public HttpResponseMessage List([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "cursor", "limit", "article_id" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            user_collection collection = new user_collection {
                user_id = token.get_and_parse(ActionContext).setParams,
            };

            int cursor = obj_list["cursor"].Value<int>(), limit = obj_list["limit"].Value<int>();
            List<user_collection> _Collection = UserCollectionBLL.Condition_PagingQuery(cursor, limit, collection);

            List<object> _List = new List<object>();
            foreach (var item in _Collection) {
                var data = EntityUtil.EntityByDic<user_collection>(item);
                user_collect collect = new user_collect {
                    user_id = token.get_and_parse(ActionContext).setParams,
                    collection_id = item.collection_id,
                };
                var g = UserCollectBLL.Query_CollectionID(collect).GroupBy(i => i.item_id);
                var post_article_count = 0;
                var is_has_in = false;
                foreach (var n in g) {
                    var article_id = obj_list["article_id"].Value<string>();
                    post_article_count = n.Count();
                    is_has_in = n.Key == article_id;
                }
                data.Add("post_article_count", post_article_count);
                data.Add("is_has_in", is_has_in);
                _List.Add(data);
            }


            int Total = UserCollectionBLL.Criteria_TotalVolume(collection);
            //是否有下一页
            bool More = true;
            if (_Collection.Count < limit) {
                More = false;
            }
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, (cursor + _Collection.Count).ToString(), _List, More));
        }


    }
}