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
    /// 管理员控制面板
    /// </summary>
    public class AdminControlUserController : ApiController
    {
        public IUserService UserBLL { get; set; }

        [HttpGet]
        [Route("admin_api/v1/control_user/analyse")]
        public HttpResponseMessage Analyse()
        {
            ResultMsg result = new ResultMsg();
            List<user_all> user_info = UserBLL.Query_All();

            List<object> info = new List<object>();
            var g = user_info.GroupBy(i => i.place);
            foreach (var item in g) {
                Dictionary<object, object> dic = new Dictionary<object, object> {
                    { "name", item.Key },
                    { "value", item.Count() }
                };
                info.Add(dic);
            }
            object data = new {
                total_users = user_info.Count,
                dau = "~",
                register = "~",
                warn = "~",
                place = info
            };
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), data));
        }


        [HttpPost]
        [Route("admin_api/v1/control_user/user")]
        public HttpResponseMessage GetInfo([FromBody] JObject obj)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            List<string> enable_changes = new List<string>();
            string[] ef_item = new string[] { "status" };
            foreach (var item in obj_list.Properties()) {
                if (!((IList)ef_item).Contains(item.Name) || item.Value.ToString() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
                enable_changes.Add(item.Name);
            }
            user_all user = new user_all {
                status = obj_list["status"].Value<int>()
            };
            List< user_all> _user = UserBLL.Query_Status(user);
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), _user));
        }


        [HttpPost]
        [Route("admin_api/v1/control_user/query")]
        public HttpResponseMessage Query([FromBody] JObject obj)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            List<string> enable_changes = new List<string>();
            string[] ef_item = new string[] { "key_word", "status" };
            foreach (var item in obj_list.Properties()) {
                if (!((IList)ef_item).Contains(item.Name) || item.Value.ToString() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
                enable_changes.Add(item.Name);
            }
            string key_word = obj_list["key_word"].Value<string>();
            int status = obj_list["status"].Value<int>();
            List<user_all> _user = UserBLL.Search(key_word, status);
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), _user));
        }


        [HttpGet]
        [Route("admin_api/v1/control_user/query_not")]
        public HttpResponseMessage Query_Not()
        {
            ResultMsg result = new ResultMsg();
            List<user_all> _user = UserBLL.Query_ALL_NOTStatus();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), _user));
        }


        [HttpPost]
        [Route("admin_api/v1/control_user/state")]
        public HttpResponseMessage ChangeState([FromBody] JObject obj)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            List<string> enable_changes = new List<string>();
            string[] ef_item = new string[] { "user_id", "status" };
            foreach (var item in obj_list.Properties()) {
                if (!((IList)ef_item).Contains(item.Name) || item.Value.ToString() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
                enable_changes.Add(item.Name);
            }
            switch (obj_list["status"].Value<int>()) {
                case 0: break;
                case 1: break;
                case 2: break;
                case 3: break;
                case 4: break;
                default: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPERATION_TYPE_ERROR, Descripion.GetDescription(ResultCode.OPERATION_TYPE_ERROR)));
            }
            //初始化实例
            user_all user = new user_all();
            //动态给实体赋值
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Type t = user.GetType();
            foreach (PropertyInfo pi in t.GetProperties()) {
                if (pi.PropertyType.Name == "String")
                    dic.Add(pi.Name, "");
                if (pi.PropertyType.Name == "Int")
                    dic.Add(pi.Name, 0);
            }
            user_all next_user = JsonConvert.DeserializeObject<user_all>(JsonConvert.SerializeObject(dic));
            next_user.user_id = obj_list["user_id"].Value<string>();
            next_user.status = obj_list["status"].Value<int>();
            if (UserBLL.Query_ID(next_user).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.USER_NOT_EXIST, Descripion.GetDescription(ResultCode.USER_NOT_EXIST)));
            }
            UserBLL.Update_Condition(next_user, enable_changes.ToArray());
            UserBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }

    }
}
