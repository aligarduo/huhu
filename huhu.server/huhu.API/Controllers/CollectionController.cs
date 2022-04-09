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
    /// 收藏集
    /// </summary>
    public class CollectionController : ApiController
    {
        public IUserCollectService UserCollectBLL { get; set; }
        public IUserCollectionService UserCollectionBLL { get; set; }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("interact_api/v1/collection/add")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Add([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null || obj_list.Property("collection_name") == null || obj_list["collection_name"].Value<string>() == "") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }

            Snowflake.Instance.SnowflakesInit(0, 0);
            user_collection collection = new user_collection {
                collection_id = Snowflake.Instance.NextId().ToString(),
                user_id = token.get_and_parse(ActionContext).setParams,
                collection_name = obj_list["collection_name"].Value<string>(),
                mtime = TimeUtil.GetCurrentTimestamp().ToString(),
                ctime = TimeUtil.GetCurrentTimestamp().ToString()
            };

            if (UserCollectionBLL.Is_Collection_Name(collection).FirstOrDefault() != null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.COLLECTION_IDENTICAL, Descripion.GetDescription(ResultCode.COLLECTION_IDENTICAL)));
            }

            UserCollectionBLL.Add(collection);
            UserCollectionBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), collection));
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("interact_api/v1/collection/delete")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Delete([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null || obj_list.Property("collection_id") == null || obj_list["collection_id"].Value<string>() == "") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }

            user_collection collection = new user_collection {
                collection_id = obj_list["collection_id"].Value<string>(),
                user_id = token.get_and_parse(ActionContext).setParams
            };
            if (UserCollectionBLL.Is_Collection_ID(collection).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.COLLECTION_NOT_EXIST, Descripion.GetDescription(ResultCode.COLLECTION_NOT_EXIST)));
            }
            UserCollectionBLL.Delete(collection);
            UserCollectionBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("interact_api/v1/collection/list")]
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
                user_id = token.get_and_parse(ActionContext).setParams
            };
            int cursor = obj_list["cursor"].Value<int>(), limit = obj_list["limit"].Value<int>();
            List<user_collection> _Collection = UserCollectionBLL.Condition_PagingQuery(cursor, limit, collection);

            List<object> _List = new List<object>();
            foreach (var item in _Collection)
            {
                var data = EntityUtil.EntityByDic<user_collection>(item);
                user_collect collect = new user_collect
                {
                    item_id = obj_list["article_id"].Value<string>(),
                    user_id = token.get_and_parse(ActionContext).setParams,
                    collection_id = item.collection_id,
                };

                var c = UserCollectBLL.Is_Collect_Item(collect).FirstOrDefault();
                if (c == null)
                    data.Add("is_collect", false);
                else
                    data.Add("is_collect", true);

                var g = UserCollectBLL.Query_CollectionID(collect).GroupBy(i => i.item_id);
                var post_article_count = 0;
                foreach (var n in g)
                {
                    post_article_count = n.Count();
                }
                data.Add("post_article_count", post_article_count);
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


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("interact_api/v1/collection/update")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Update([FromBody] JObject obj)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "collection_id", "collection_name" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            user_collection collection = new user_collection {
                collection_id = obj_list["collection_id"].Value<string>(),
                user_id = token.get_and_parse(ActionContext).setParams,
                collection_name = obj_list["collection_name"].Value<string>(),
                mtime = TimeUtil.GetCurrentTimestamp().ToString(),
                ctime = ""
            };

            if (UserCollectionBLL.Is_Collection_ID(collection).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.COLLECTION_NOT_EXIST, Descripion.GetDescription(ResultCode.COLLECTION_NOT_EXIST)));
            }
            UserCollectionBLL.Update_Condition(collection, new string[] { "collection_name" });
            UserCollectionBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), collection));
        }


    }
}
