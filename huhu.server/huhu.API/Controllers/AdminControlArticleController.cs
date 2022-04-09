using huhu.Commom;
using huhu.Commom.Enums;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 审核文章
    /// </summary>
    public class AdminControlArticleController : ApiController
    {
        public IArticleService ArticleBLL { get; set; }


        [HttpPost]
        [Route("admin_api/v1/control_article/state")]
        public HttpResponseMessage ChangeState([FromBody] JObject obj)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            List<string> enable_changes = new List<string>();
            string[] ef_item = new string[] { "article_id", "audit_status" };
            foreach (var item in obj_list.Properties()) {
                if (!((IList)ef_item).Contains(item.Name) || item.Value.ToString() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
                enable_changes.Add(item.Name);
            }
            switch (obj_list["audit_status"].Value<int>()) {
                case 0: break;
                case 1: break;
                case 2: break;
                default: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPERATION_TYPE_ERROR, Descripion.GetDescription(ResultCode.OPERATION_TYPE_ERROR)));
            }
            //初始化实例
            article_all article = new article_all();
            //动态给实体赋值
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Type t = article.GetType();
            foreach (PropertyInfo pi in t.GetProperties()) {
                if (pi.PropertyType.Name == "String")
                    dic.Add(pi.Name, "");
                if (pi.PropertyType.Name == "Int")
                    dic.Add(pi.Name, 0);
            }
            article_all next_user = JsonConvert.DeserializeObject<article_all>(JsonConvert.SerializeObject(dic));
            next_user.article_id = obj_list["article_id"].Value<string>();
            next_user.audit_status = obj_list["audit_status"].Value<int>();
            if (ArticleBLL.Query_ID(next_user).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ARTICLE_NOT_EXIST_OR_DELETED, Descripion.GetDescription(ResultCode.ARTICLE_NOT_EXIST_OR_DELETED)));
            }
            ArticleBLL.Update_Condition(next_user, enable_changes.ToArray());
            ArticleBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }

    }
}
